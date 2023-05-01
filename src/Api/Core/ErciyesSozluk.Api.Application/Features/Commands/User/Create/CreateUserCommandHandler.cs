using AutoMapper;
using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.BlazorSozluk.Common;
using ErciyesSozluk.Common.Events.User;
using ErciyesSozluk.Common.Infrastructure;
using ErciyesSozluk.Common.Infrastructure.Exceptions;
using ErciyesSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);
            var existUserName = await userRepository.GetSingleAsync(i => i.UserName == request.UserName);
            
            if (existUser is not null || existUserName is not null)
            {
                throw new DatabaseValidationException("User already exists");
            }


            var dbUser = mapper.Map<Api.Domain.Models.User>(request);
            dbUser.Password = PasswordEncryptor.Encrypt(request.Password);

            var rows = await userRepository.AddAsync(dbUser);

            //email changed or created

            if (rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = request.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName,
                    exchangeType: SozlukConstants.DefaultExchangeType,
                    queueName: SozlukConstants.UserEmailChangedQueueName,
                    obj: @event);
            }

            return dbUser.Id;
        }
    }
}

using AutoMapper;
using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.BlazorSozluk.Common;
using ErciyesSozluk.Common.Events.User;
using ErciyesSozluk.Common.Infrastructure.Exceptions;
using ErciyesSozluk.Common.Infrastructure;
using ErciyesSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }
        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);

            if (dbUser is null)
            {
                throw new DatabaseValidationException("User not found");
            }

            var dbEmailAddress = dbUser.EmailAddress;
            //0 değilse iki değer birbirinden farklı
            var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

            //request'teki değerler dbuser'a yazılır
            mapper.Map(request, dbUser);

            var rows = await userRepository.UpdateAsync(dbUser);

            //TODO: email ve username icin baskasinda olup olmadigini kontrol et

            //check if email changed
            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName,
                    exchangeType: SozlukConstants.DefaultExchangeType,
                    queueName: SozlukConstants.UserEmailChangedQueueName,
                    obj: @event);

                //yeni mail adresi confirm edilmeli
                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}

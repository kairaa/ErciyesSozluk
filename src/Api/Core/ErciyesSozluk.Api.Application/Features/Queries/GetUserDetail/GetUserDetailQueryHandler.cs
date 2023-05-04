using AutoMapper;
using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Features.Queries.GetUserDetail
{
    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mappper;

        public GetUserDetailQueryHandler(IUserRepository userRepository, IMapper mappper)
        {
            this.userRepository = userRepository;
            this.mappper = mappper;
        }

        public async Task<UserDetailViewModel> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            Domain.Models.User dbUser = null;

            if (request.UserId != Guid.Empty)
            {
                dbUser = await userRepository.GetByIdAsync(request.UserId);
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                dbUser = await userRepository.GetSingleAsync(i => i.UserName == request.UserName);
            }

            return mappper.Map<UserDetailViewModel>(dbUser);
        }
    }
}

using AutoMapper;
using ErciyesSozluk.Api.Domain.Models;
using ErciyesSozluk.Common.Models.Queries;
using ErciyesSozluk.Common.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErciyesSozluk.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();

            CreateMap<CreateEntryCommand, Entry>().ReverseMap();
            CreateMap<CreateEntryCommentCommand, EntryComment>().ReverseMap();
        }
    }
}

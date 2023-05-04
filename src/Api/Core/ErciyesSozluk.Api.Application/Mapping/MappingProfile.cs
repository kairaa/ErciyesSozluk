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
            CreateMap<UserDetailViewModel, User>().ReverseMap();

            CreateMap<CreateEntryCommand, Entry>().ReverseMap();
            CreateMap<CreateEntryCommentCommand, EntryComment>().ReverseMap();

            //entry'den getentriesview model'a dönüşüm olur
            //GetEntriesViewModel'daki commentcount'u elde etmek için Entry'nin içindeki
            //EntryComments'in count'u alınır
            CreateMap<Entry, GetEntriesViewModel>()
            .ForMember(x => x.CommentCount, y => y.MapFrom(z => z.EntryComments.Count));
        }
    }
}

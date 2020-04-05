using ApiApp_Male.Models;
using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Models.ResponseDTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIApp.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, AuthorResponseDTO>()
                .ForMember(x=>x.BookCount,
                        op=>op.MapFrom(s=>s.Books.Count()+1));

            CreateMap<AuthorUpdateRequestDTO, Author>();

            CreateMap<AuthorAddRequestDTO, Author>();
        }
    }
}

using AutoMapper;
using bookStore.API.Data;
using bookStore.API.Models;

namespace bookStore.API.Helpers
{
    public class ApplicationMapper : Profile //Profile from AutoMapper
    {
        public ApplicationMapper()
        {
            CreateMap<Books, BookModel>().ReverseMap();
        }
    }
}

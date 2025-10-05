using AutoMapper;

namespace BookManagement.Infrastructure.MapperProfile
{
    public class Entity2Domain : Profile
    {
        public Entity2Domain()
        {
            CreateMap<Entities.Book, Domain.Entities.Book>();

            CreateMap<Entities.Author, Domain.Entities.Author>();

            CreateMap<Domain.Entities.Book, Entities.Book>();
        }
    }
}

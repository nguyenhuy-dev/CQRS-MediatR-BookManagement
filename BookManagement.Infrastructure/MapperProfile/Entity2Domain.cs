using AutoMapper;

namespace BookManagement.Infrastructure.MapperProfile
{
    /// <summary>
    /// Mapper entity supporter.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class Entity2Domain : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity2Domain"/> class.
        /// </summary>
        public Entity2Domain()
        {
            CreateMap<Entities.Book, Domain.Entities.Book>();

            CreateMap<Entities.Author, Domain.Entities.Author>();

            CreateMap<Domain.Entities.Book, Entities.Book>();
        }
    }
}

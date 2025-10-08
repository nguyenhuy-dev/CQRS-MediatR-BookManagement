using BookManagement.Infrastructure.MapperProfile;
using FluentValidation;

namespace BookManagement.Bootstraping
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(cfg => { }, typeof(Entity2Domain).Assembly);

            builder.Configuration.AddJsonFile("/run/secrets/connection_strings", optional: true); // for Docker?

            //builder.Services.AddHostedService<Worker>(); // for Web API

            var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

            builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        }
    }
}

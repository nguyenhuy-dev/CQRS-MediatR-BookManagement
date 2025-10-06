using BookManagement.Application.Behaviors;
using BookManagement.Domain.Repositories;
using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.MapperProfile;
using BookManagement.Infrastructure.Repositories;
using BookManagement.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace BookManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(cfg => { }, typeof(Entity2Domain).Assembly);

            builder.Configuration.AddJsonFile("/run/secrets/connection_strings", optional: true);

            builder.Services.AddDbContext<BookManagementDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("BookManagementDatabase")));

            //builder.Services.AddHostedService<Worker>(); // for Web API

            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddValidatorsFromAssembly(applicationAssembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

using BookManagement.Application.Behaviors;
using BookManagement.Domain.Repositories;
using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.Repositories;
using BookManagement.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Bootstraping
{
    public static class DIServiceExtensions
    {
        public static void AddDIServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BookManagementDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("BookManagementDatabase")));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}

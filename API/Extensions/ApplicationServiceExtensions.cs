using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using API.Helpers;
namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
              services.AddScoped<ITokenService,TokenService>();
              services.AddScoped<IPhotosService,Photoservice>();
              services.AddScoped<LogUserActivity>();
              services.AddScoped<IUserRepository,UserRepository>();
              services.AddScoped<ILikesRepository,LikesRepository>();
              services.AddScoped<IMessageRepository,MessageRepository>();
              services.AddAutoMapper(typeof(AutoMapersProfile).Assembly);
               services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
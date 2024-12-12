using Microsoft.Extensions.DependencyInjection;
using MoveDb.Services.Data.Entities;
using System.Runtime.CompilerServices;

namespace MoveDb.Area.Models {
    public static class AppSettingsMapExtension { 

        public static void AddAppSettingsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"), options => options.BindNonPublicProperties = true);
            services.Configure<AppSettings>(options => configuration.Bind("AppSettings", options));
        }
    }
}

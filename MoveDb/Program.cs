using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using MoveDb;
using MoveDb.Area.Models;
using MoveDb.Services.Data.Entities;
using DotnetGeminiSDK;
using Autofac.Core;

public class Program {
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews()
            .AddRazorOptions(opt =>
            {
                opt.ViewLocationFormats.Clear();
                opt.ViewLocationFormats.Add("/Area/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Area/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Area/Views/{0}" + RazorViewEngine.ViewExtension);

            });

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        var env = builder.Environment;

        builder.Services.AddGeminiClient(cfg =>
        {
            cfg.ApiKey = "AIzaSyCvNM5Qtcaw2FgfbVOEKdnX3wTwciSMx5k";
        });

        IConfigurationBuilder configurationBuilder = builder.Configuration
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

        IConfiguration configuration = configurationBuilder.Build();

        builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ContainerModule(configuration)));

        // Appsettings config
        builder.Services.AddAppSettingsService(configuration);
        builder.Services.AddOptions<AppSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
		{
			OnPrepareResponse = ctx =>
			{
				ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
				ctx.Context.Response.Headers.Append("Pragma", "no-cache");
				ctx.Context.Response.Headers.Append("Expires", "0");
			}
		});

		app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=home}/{id?}/{action=Index}");

        app.Run();
    }
}
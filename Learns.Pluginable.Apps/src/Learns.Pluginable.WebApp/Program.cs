using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace Learns.Pluginable.WebApp
{
internal class Program
{
    private  static ILogger<Program> logger;
    private static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddLogging();

        builder.Services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation();

        builder.Services.TryAddSingleton<PluginHandler>();

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            var serviceProvider = builder.Services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
#pragma warning disable CS8601 // Possible null reference assignment.
            logger = serviceProvider.GetService<ILogger<Program>>();
#pragma warning restore CS8601 // Possible null reference assignment.
                              // logger.LogInformation($"{Directory.GetCurrentDirectory()}");
            /*
            var pluginHandler = serviceProvider.GetService<IPluginHandler>();
            pluginHandler.GetPlugins();
    */
        RegisterPlugins(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapAreaControllerRoute(
            name: "sample", 
            areaName: "Plugins/Sample",
            pattern: "Plugins/Sample/{controller=Home}/{action=Index}/{id?}");
            
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
            
        // app.MapControllerRoute(
        //     name: "sample",
        //     pattern: "Plugins/Sample/{controller=Home}/{action=Index}/{id?}");
        
        app.Run();
    }

        private static void RegisterPlugins(IServiceCollection services)
        {
            var pluginsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
            logger.LogInformation(pluginsFolder);
            var pluginsDirectory = new DirectoryInfo(pluginsFolder);
            var expanders = new List<Type>();
            var pluginAssemblies = new List<Assembly>();
            if(pluginsDirectory.Exists){
                foreach(var folder in pluginsDirectory.GetDirectories())
                {
                    logger.LogInformation($"{folder.FullName}");
                    foreach(var pluginDll in Directory.GetFiles(folder.FullName, "*.dll"))
                    {
                        var plugin = Assembly.LoadFrom(pluginDll);
                        pluginAssemblies.Add(plugin);
                        logger.LogInformation($"{plugin.FullName}");

                        var viewLocationExpanders = plugin.GetTypes().Where(x => x.FullName.EndsWith("Expander"));
                        foreach(var expander in viewLocationExpanders){
                            expanders.Add(expander);
                        }
                    }
                }
            }

            
            services.Configure<RazorViewEngineOptions>(options =>
            {
                foreach(var e in expanders)
                {
                    options.ViewLocationExpanders.Add((IViewLocationExpander)Activator.CreateInstance(e));
                }
            });

            foreach(var plugin in pluginAssemblies)
            {
                services.AddControllersWithViews().AddApplicationPart(plugin);
                var fileProvider = new ManifestEmbeddedFileProvider(plugin, "wwwroot");
                services.Configure<MvcRazorRuntimeCompilationOptions>(options => 
                {
                    options.FileProviders.Add(fileProvider);
                });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Linux.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Linux
{
   
    public class Startup
    {
        static string passwd ;
      public readonly IApiConfigurationSettings _config;
        IHostingEnvironment _env { get; set; }
        private static PhysicalFileProvider _fileProvider =
   new PhysicalFileProvider(System.Environment.GetEnvironmentVariable("passwd"));
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
           
        //}
        public Startup(IHostingEnvironment env)
        {
            //passwd = System.Environment.GetEnvironmentVariable("passwd");
            _config = new ApiConfigurationSettings();
            _config.GroupFiles = System.Environment.GetEnvironmentVariable("groupfiles");
            _config.Passwd = System.Environment.GetEnvironmentVariable("passwd");
            _config.ContentRoot = env.ContentRootPath;
          
            _env = env;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton(typeof(IApiConfigurationSettings), _config);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
       /*     ChangeToken.OnChange(
    () => _fileProvider.Watch(System.Environment.GetEnvironmentVariable("passwd")),
    (state) => Linux.Classes.watcher.MainAsync(),
    env);
            ChangeToken.OnChange(
   () => _fileProvider.Watch(System.Environment.GetEnvironmentVariable("groupfiles")),
   (state) => Linux.Classes.watcher.MainAsync(),
   env);
            */

        }
    }
}

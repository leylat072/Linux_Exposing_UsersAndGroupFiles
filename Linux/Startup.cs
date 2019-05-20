using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private static PhysicalFileProvider _fileProvider =
   new PhysicalFileProvider("C:\\test");
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            services.AddSingleton<IFileProvider>(physicalProvider);
          
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
            ChangeToken.OnChange(
    () => _fileProvider.Watch("test.txt"),
    (state) => Linux.Classes.watcher.MainAsync(),
    env);
            ChangeToken.OnChange(
   () => _fileProvider.Watch("C:\\test\\test.txt"),
   (state) => Linux.Classes.watcher.MainAsync(),
   env);

        }
    }
}

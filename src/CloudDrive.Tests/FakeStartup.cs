using CloudDrive.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CloudDrive.Tests
{
    public class FakeStartup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Action<DbContextOptionsBuilder> db_configuration = x => x.UseInMemoryDatabase(Configuration["Database:TestString"]);
            Configuration["Auth:SecretKey"] = "abncdefdfgd2345_&*(&(^&";
            StartupHelper.ConfigureServices(services, db_configuration, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Context context)
        {
            StartupHelper.Configure(app, env, context);
        }
    }
}

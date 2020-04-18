using CloudDrive.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using FluentValidation.AspNetCore;
using CloudDrive.Models.Validators;
using CloudDrive.Interfaces;
using CloudDrive.Services;
using Microsoft.AspNetCore.Identity;
using CloudDrive.Models;

namespace CloudDrive
{
    public static class StartupHelper
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, Context context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cloud Drive API v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void ConfigureServices(IServiceCollection services, Action<DbContextOptionsBuilder> db_config, IConfiguration configuration)
        {
            services.AddTransient<ITokenService, JwtService>();
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<AccountService>();

            ConfigureJWT(services, configuration);
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterInputValidator>());
            services.AddDbContext<Context>(db_config);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cloud Drive API", Version = "v1" });
            });
        }

        private static void ConfigureJWT(IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["Auth:SecretKey"];

            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("JWT Secret key is not configured.");

            var keyBytes = Encoding.ASCII.GetBytes(key);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                };
            });
        }
    }
}

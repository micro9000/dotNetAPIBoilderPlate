using API_DataAccess.DataAccess;
using API_DataAccess.DataAccess.Core;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.SettingModel;
using System.Text;
using API.Services;
using API_DataAccess.SettingModel;
using API_DataAccess.DataAccess.Contracts;
using EmailService;
using API.Middleware;
//using System.Data.SqlClient;

namespace API
{
    public class Startup
    {
        // https: //andrewlock.net/ihostingenvironment-vs-ihost-environment-obsolete-types-in-net-core-3/
        //IWebHostEnvironment IHostEnvironment
        public Startup(IWebHostEnvironment env)
        {
            //Configuration = configuration;
            Configuration = new ConfigurationBuilder()
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Secret Manager tool
            // dotnet user-secrets set "DbPassword" "pass123" and connectionstring in secret.js in Manage User Secrets
            //var databaseSettingsSection = Configuration
            //                                    .GetSection(nameof(DatabaseSettings))
            //                                    .Get<DatabaseSettings>();
            //var sqlConnStringBuilder = new SqlConnectionStringBuilder(databaseSettingsSection.Main.ConnectionString);
            //sqlConnStringBuilder.Password = Configuration["DbPassword"];
            //databaseSettingsSection.Main.ConnectionString = sqlConnStringBuilder.ConnectionString;
            //services.AddSingleton(databaseSettingsSection);


            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            
            var authenticationSettingsSection = Configuration.GetSection(nameof(AuthenticationSettings));
            services.Configure<AuthenticationSettings>(authenticationSettingsSection);

            //services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            var smtpSettings = Configuration
                                    .GetSection("SmtpSettings")
                                    .Get<SmtpSettings>();
            services.AddSingleton(smtpSettings);

            services.AddOptions();


            var authenticationSettings = authenticationSettingsSection.Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(authenticationSettings.Secret);
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
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
               };
           });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IRoleData, RoleData>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IUserRoleData, UserRoleData>();
            services.AddScoped<IUserRefreshTokenData, UserRefreshTokenData>();

            
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IUserService, UserService>();


            services.AddCors();
            // Enable this if you need to restrict to specific origin
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("default", builder =>
            //    {
            //        builder.WithOrigins("http: //127.0.0.1")
            //        .AllowAnyHeader()
            //        .AllowAnyMethod();
            //    });
            //});
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SecureAPI", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SecureAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // enable this if you enable the addCors section on ConfigureServices
            //app.UseCors("default");


            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

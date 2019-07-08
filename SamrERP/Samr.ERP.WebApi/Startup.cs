using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Services;
using Samr.ERP.Infrastructure.Data;
using Samr.ERP.Infrastructure.Data.Concrete;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Data.Helpers;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;
using Samr.ERP.WebApi.Configurations.AutoMapper;
using Samr.ERP.WebApi.Configurations.Models;
using Samr.ERP.WebApi.Configurations.Swagger;
using Samr.ERP.WebApi.Infrastructure;
using Samr.ERP.WebApi.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace Samr.ERP.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Dependecy Injection

            services.AddDbContext<SamrDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<UserProvider>();
            services.AddScoped<RepositoryFactories, RepositoryFactories>();
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeLockReasonService, EmployeeLockReasonService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsCategoriesService, NewsCategoriesService>();
            services.AddScoped<IEmailSettingService, EmailSettingSettingService>();
            services.AddScoped<IUserLockReasonService, UserLockReasonService>();
            services.AddScoped<IGenderService, GenderService>();


            #endregion

            #region Identity & JWT

            services.AddDefaultIdentity<User>(options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 4;

                    })
                    .AddEntityFrameworkStores<SamrDbContext>();

            services.Configure<AppSettings>(Configuration.GetSection("TokenSettings"));

            var token = Configuration.GetSection("TokenSettings").Get<AppSettings>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

            });

            #endregion

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true
            );

            services.AddAutoMapperSetup();

            services.AddSwaggerDocumentation();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            SetUpCulture();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            //app.UseCors(options => options.WithOrigins("https://localhost:4200"));
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
          
            app.UseMiddleware<UserMiddleware>();

            app.UseSwaggerDocumentation();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private void SetUpCulture()
        {
            var ruCulture = new CultureInfo("RU-ru");
            CultureInfo.CurrentCulture = ruCulture;
            CultureInfo.CurrentUICulture = ruCulture;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SignalR;
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
using Samr.ERP.Infrastructure.SeedData;
using Samr.ERP.WebApi.Configurations;
using Samr.ERP.WebApi.Configurations.AutoMapper;
using Samr.ERP.WebApi.Configurations.Models;
using Samr.ERP.WebApi.Configurations.Swagger;
using Samr.ERP.WebApi.Hub;
using Samr.ERP.WebApi.Infrastructure;
using Samr.ERP.WebApi.Middleware;
using Samr.ERP.WebApi.Services;
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
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Scoped);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<PdfConverterService>();
            services.AddScoped<IHtmlTemplateXService, HtmlTemplateXService>();
            services.AddScoped<UserProvider>();
            services.AddScoped<RepositoryFactories, RepositoryFactories>();
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IActiveUserTokenService, ActiveUserTokenService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeLockReasonService, EmployeeLockReasonService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsCategoriesService, NewsCategoriesService>();
            services.AddScoped<IEmailSettingService, EmailSettingSettingService>();
            services.AddScoped<IUserLockReasonService, UserLockReasonService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<IUsefulLinkCategoryService, UsefulLinkCategoryService>();
            services.AddScoped<IUsefulLinkService, UsefulLinkService>();
            services.AddScoped<IFileCategoryService, FileCategoryService>();
            services.AddScoped<IFileArchiveService, FileArchiveService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<HubEvent.HubEvent>();
            services.AddScoped(typeof(IHistoryService<,>),
                typeof(HistoryService<,>));
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
                    .AddRoles<Role>()
                    //.AddRoleManager<Role>()
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
                x.TokenValidationParameters = TokenAuthenticationService.GetTokenValidationParameters(token);

            });

            #endregion
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new PdfTemplateLocationExpander());
            });
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyOrigin()
                    .WithOrigins("http://localhost:4200", 
                    "http://sumr.evomedia.pro","https://sumr.evomedia.pro",
                    "http://samr.evomedia.pro","http://samr.evomedia.pro")
                    .WithExposedHeaders("Content-Disposition");
            }));
            services.AddSignalR();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true
            );
         
            services.AddAutoMapperSetup();

            services.AddSwaggerDocumentation();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            SetUpCulture();
            LoadPdfNativeLib();
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

            app.UseCors("CorsPolicy");
            //app.UseCors(builder => builder
            //    .AllowAnyOrigin()
            //    .AllowCredentials()
            //    .AllowAnyHeader()
            //    .AllowAnyMethod()
            //    .SetIsOriginAllowed(_ => true)
            //    .WithExposedHeaders("Content-Disposition")
            ////.SetIsOriginAllowed((host) => true)
            //);

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
          
            app.UseMiddleware<UserMiddleware>();
            app.UseMiddleware<TokenManagerMiddleware>();

            app.UseSwaggerDocumentation();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/ReceiveMessage");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

           

            //NotificationService.NotifyMessage += (object sender, EventArgs args) => Debug.WriteLine("Yes it is");
            DbInitializer.AddRolesToSystemUser(userManager,roleManager);
        }

        private void SetUpCulture()
        {
            var ruCulture = new CultureInfo("ru-RU");
            CultureInfo.CurrentCulture = ruCulture;
            CultureInfo.CurrentUICulture = ruCulture;
            CultureInfo.DefaultThreadCurrentCulture = ruCulture;
            CultureInfo.DefaultThreadCurrentUICulture = ruCulture;
        }

        private void LoadPdfNativeLib()
        {
            var processSufix = "32bit";
            if (Environment.Is64BitProcess && IntPtr.Size == 8)
            {
                processSufix = "64bit";
            }
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), $"NativeLibs\\{processSufix}\\libwkhtmltox.dll"));
        }
    }
}

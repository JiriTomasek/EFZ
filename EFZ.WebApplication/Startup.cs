using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using EFCore.DbContextFactory.Extensions;
using EFZ.Core.Database.Dao;
using EFZ.Core.Entities.Dao;
using EFZ.Database.Dao;
using EFZ.Database.DbContext;
using EFZ.Domain.BusinessLogic.Impl;
using EFZ.Domain.BusinessLogic.Interface;
using EFZ.Domain.CustomIdentityProvider;
using EFZ.Entities.Entities;
using EFZ.WebApplication.Extensions;
using EFZ.WebApplication.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EFZ.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static readonly ILoggerFactory ToolLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


            services.AddDbContext<EfzDbContext>(options=> options.EnableSensitiveDataLogging().UseLoggerFactory(ToolLoggerFactory).UseSqlServer(Configuration.GetConnectionString("Development")), ServiceLifetime.Transient);

            
            services.AddScoped(typeof(ICommonDao<>), typeof(CommonDao<>));

            services.AddTransient<IBaseDaoFactory, BaseDaoFactory>();


            services.AddScoped<IUserBlProvider, UserBlProvider>();

            services.AddScoped<ISettingsBlProvider, SettingsBlProvider>();
            services.AddScoped<IOrderBlProvider, OrderBlProvider>();
            services.AddScoped<IDeliveryBlProvider, DeliveryBlProvider>();
            services.AddScoped<IAttachmentBlProvider, AttachmentBlProvider>();

            services.AddScoped<IInvoiceBlProvider, InvoiceBlProvider>();

            services.AddSingleton<IEmailSendGridBlProvider, EmailSendGridBlProvider>();

            services.AddSingleton<IAttachmentScanHelperProvider, AttachmentScanHelperProvider>();

            services.AddSingleton<IJobSchedulerBlProvider, JobSchedulerBlProvider>();


            services.AddScoped<ICompletionBlProvider, CompletionBlProvider>();

            services.AddSession(options =>
            {
                // 1 hour base
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.Name = "efz.session.v101";
            });
            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();


           
            services.AddScoped<BaseControllerFilter>();


            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                .AddCookie(
                    opts => new CookieAuthenticationOptions
                    {
                        ExpireTimeSpan = TimeSpan.FromMinutes(60),
                        SlidingExpiration = true
                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            app.ApplicationServices.GetService<IJobSchedulerBlProvider>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCookiePolicy();
            app.UseSession();


            var cultureInfo = new CultureInfo("cs-CZ");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseRequestLocalization();

            var requestOpt = new RequestLocalizationOptions();
            requestOpt.SupportedCultures = new List<CultureInfo>
            {
                new CultureInfo("cs-CZ")
            };
            requestOpt.SupportedUICultures = new List<CultureInfo>
            {
                new CultureInfo("en-US")
            };
            requestOpt.RequestCultureProviders.Clear();
            requestOpt.RequestCultureProviders.Add(new SingleCultureProvider());

            app.UseRequestLocalization(requestOpt);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    public class SingleCultureProvider : IRequestCultureProvider
    {
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            return Task.Run(() => new ProviderCultureResult("cs-CZ", "en-US"));
        }
    }
}

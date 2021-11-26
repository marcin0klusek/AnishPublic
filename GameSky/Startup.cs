using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;

namespace GameSky
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
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                    })
                );

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            //Jobs scheduler - Hangfire
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseSerializerSettings(new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
            .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));
            services.AddHangfireServer();

            //Real-time execution code
            services.AddSignalR();

            //Toast maker
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomLeft; });


            services.AddMvc();
            services.AddControllersWithViews();
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/playergenerator");
                options.Conventions.AuthorizePage("/ticket/create", "RequireUserRole");
                options.Conventions.AuthorizePage("/hangfire", "RequireAdminRole");
                options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            })
                .AddRazorRuntimeCompilation();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUserRole",
                        policy => policy.RequireRole("User"));
                options.AddPolicy("RequireAdminRole",
                     policy => policy.RequireRole("Admin"));
            });

            services.Configure<RouteOptions>(option =>
            {
                option.LowercaseUrls = true;
                option.LowercaseQueryStrings = true;
            });
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Errors/{0}");
                app.UseStatusCodePagesWithReExecute("/Errors/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard();


            app.UseRouting();

            ResultsHub.Current = app.ApplicationServices.GetService<IHubContext<ResultsHub>>();
            TicketHub.Current = app.ApplicationServices.GetService<IHubContext<TicketHub>>();
            MatchesHub.Current = app.ApplicationServices.GetService<IHubContext<MatchesHub>>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard("/hangfire");
                endpoints.MapHub<ResultsHub>("resultsHub");
                endpoints.MapHub<TicketHub>("ticketHub");
                endpoints.MapHub<MatchesHub>("match/matchesHub");
            });
        }
    }
}

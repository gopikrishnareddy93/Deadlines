using System;
using System.Linq;
using Deadlines.Data.Context;
using Deadlines.Data.Interfaces;
using Deadlines.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Config;
using NLog.Targets;
using Microsoft.EntityFrameworkCore;

namespace Deadlines
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static readonly Logger m_Logger = LogManager.GetLogger("ContactCenter");
        private static string connectionString;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            connectionString = Environment.GetEnvironmentVariable("appDBString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Couldn't find appDBString variable in the environment variables");
            }

            services.AddSingleton(Configuration);
            services.AddDbContext<DeadlineDBContext>(options => options
                    .UseMySql(connectionString)
            );

            services.AddTransient<IDeadlineRepository, DeadlineRepository>();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            ConfigureNlog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
            });

            var contact_center_connection_array = connectionString?.Split(";");

            app.UseMvc();

            var dbContext = (DeadlineDBContext)serviceProvider.GetService(typeof(DeadlineDBContext));

            if (!CheckDatabaseConnection(dbContext))
            {
                throw new Exception("Couldn't Connect to ContactCenterDatabase");
            }

            m_Logger.Info("ContactCenterAPI started!!!");
            m_Logger.Info("ContactCenter Connection Info -- {0}, {1}", contact_center_connection_array?.FirstOrDefault(x => x.Contains("database")), contact_center_connection_array?.FirstOrDefault(x => x.Contains("server")));

        }

        private static bool CheckDatabaseConnection(DbContext dbContext)
        {
            if (dbContext == null)
            {
                return false;
            }

            try
            {
                var relationalDatabaseCreator = (dbContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);

                return relationalDatabaseCreator?.Exists() ?? false;
            }
            catch (Exception ex)
            {
                m_Logger.Info(string.Format("Error in Startup.CheckDatabaseConnection: {0}", ex));
                return false;
            }
        }

        private void ConfigureNlog()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("logfile")
            {
                FileName = "${basedir}/Logs/${logger}.log",
                Layout = "${longdate}: ${message}",
                ArchiveAboveSize = 20000000,
                KeepFileOpen = true
            };

            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);



            LogManager.Configuration = config;
            LogManager.ThrowExceptions = true;
        }

        private void SendEmail(string message, IDeadlineRepository svc)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            try
            {
                //var emailSettings = repo.GetEmailSettings();

                //if (emailSettings == null)
                //{
                //    m_Logger.Info("Startup.SendEmail: Cannot find email settings!!!");
                //    return;
                //}

                //var emailSubscribers = repo.GetDefaultEmailSubscribers();

                //if (emailSubscribers == null || emailSubscribers.Any() == false)
                //{
                //    m_Logger.Info("Startup.SendEmail: New customer email subscribers is empty");
                //    return;
                //}

                //Task.Run(() => EmailSender.Send(emailSettings, emailSubscribers, "Coredial voiceaxis database connection Failure", message));
            }
            catch (Exception ex)
            {
                m_Logger.Info(string.Format("Error in Startup.SendEmail: {0}", ex));
            }
        }
    }
}

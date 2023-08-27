using Engine;
using Engine.Services;
using Hangfire;
using Hangfire.Console;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configuration from appsettings.{env.EnvironmentName}.json, environment variables and appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get the connection string from the configuration file
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // HangFire configuration
            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connectionString);
                config.UseColouredConsoleLogProvider();
                config.UseConsole();

            });

            builder.Services.AddHangfireServer();

            #region Add services to the container

            // Add services to the container.
            builder.Services.AddScoped<IHangFireJobs, HangFireJobs>();
            builder.Services.AddScoped<ISqlServices, SqlServices>();
            builder.Services.AddScoped<IScheduleJobs, ScheduleJobs>();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // HangFire dashboard configuration
            app.MapHangfireDashboard("/dashboard", new DashboardOptions()
            {
                DarkModeEnabled = true,
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });

            // call the HangFireJobs class to configure the jobs
            using (var scope = app.Services.CreateScope())
            {
                var hangFireJobs = scope.ServiceProvider.GetRequiredService<IHangFireJobs>();
                hangFireJobs.ConfigureJobs();
                hangFireJobs.ConfigureRecurringJobs();
            }

            app.Run();
        }
    }
}
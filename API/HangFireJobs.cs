using Engine;
using Engine.Services;
using Hangfire;

namespace API
{
    internal class HangFireJobs : IHangFireJobs
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ISqlServices _sqlServices;
        private readonly IScheduleJobs _scheduleJobs;
        public HangFireJobs(IServiceProvider serviceProvider)
        {
            _backgroundJobClient = serviceProvider.GetService<IBackgroundJobClient>() ?? throw new Exception("BackgroundJobClient is null");
            _sqlServices = serviceProvider.GetRequiredService<ISqlServices>() ?? throw new Exception("ISqlServices is null");
            _scheduleJobs = serviceProvider.GetService<IScheduleJobs>() ?? throw new Exception("IScheduleJobs is null");
        }
        public void ConfigureJobs()
        {

            // Add your jobs here
            _backgroundJobClient.Enqueue(() => Console.WriteLine("Hello world from HangFire!"));
        }

        public void ConfigureRecurringJobs()
        {
            // Recurring jobs
            RecurringJob.AddOrUpdate("ChangePassword", () => _sqlServices.ChangePassword("username1", "NewPassword"), Cron.Minutely);
            RecurringJob.AddOrUpdate("ChangePasswordCron",() => _sqlServices.ChangePassword("username2", "NewPassword"), "0 * * ? * *");

            // Recurring jobs
            RecurringJob.AddOrUpdate("ConfigureSqlJobs", () => _scheduleJobs.ConfigureSqlJobs("username3","NewPassword"), Cron.Minutely);
            RecurringJob.AddOrUpdate("ConfigureSqlJobsCron", () => _scheduleJobs.ConfigureSqlJobs("username4", "NewPassword"), "0 * * ? * *");

        }
    }
}
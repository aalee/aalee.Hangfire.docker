using Engine.Services;

namespace Engine
{
    public class ScheduleJobs : IScheduleJobs
    {
        private readonly ISqlServices _sqlServices;

        public ScheduleJobs(ISqlServices sqlServices)
        {
            _sqlServices = sqlServices;
        }

        public void ConfigureSqlJobs(string username, string password)
        {
            _sqlServices.ChangePassword(username, password);
        }

    }
}
namespace API;

internal interface IHangFireJobs
{
    void ConfigureJobs();
    void ConfigureRecurringJobs();
}
using Cbms.Kms.Application.Integration;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Cbms.Kms.Web.Configuration
{
    public static class BackgroundJobExtentions
    {
        public static void EnqueueJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<SyncDataJob>("sync_data", (c) => c.RunAsync(), Cron.Daily);
        }
    }
}
using SsisUp.Builders;

namespace SsisUp.ScriptProviders.Scripts
{
    public class EnableJob : OrderedSqlScript
    {
        public EnableJob(int order, JobConfiguration configuration)
        {
            Order = order;
            Name = "Enable Job";

            Contents = @"
USE [msdb]
GO

exec msdb..sp_update_job @job_name = '@@JobName@@', @enabled = @@enabled@@
GO
"
                .Replace("@@JobName@@", configuration.JobName)
                .Replace("@@enabled@@", configuration.IsEnabled ? "1" : "0");

        }
    }
}
using SsisUp.Builders;

namespace SsisUp.ScriptProviders.Scripts
{
    public class CreateJob : OrderedSqlScript
    {
        public CreateJob(int order, JobConfiguration configuration)
        {
            Order = order;
            Name = "Create Job";

            Contents = @"
USE [msdb]
GO

DECLARE @jobId BINARY(16)
EXEC  msdb.dbo.sp_add_job @job_name='@@JobName@@', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=2, 
		@notify_level_page=2, 
		@delete_level=0, 
		@description='@@JobDescription@@', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name='@@SSISOwner@@', @job_id = @jobId OUTPUT
select @jobId
GO
EXEC msdb.dbo.sp_add_jobserver @job_name='@@JobName@@', @server_name = '@@SSISServer@@'
GO"
                .Replace("@@JobName@@", configuration.JobName)
                .Replace("@@JobDescription@@", configuration.JobDescription)
                .Replace("@@SSISServer@@", configuration.SsisServer)
                .Replace("@@SSISOwner@@", configuration.SsisOwner);
        }
    }
}
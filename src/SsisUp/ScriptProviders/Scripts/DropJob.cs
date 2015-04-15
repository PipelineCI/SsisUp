using SsisUp.Builders;

namespace SsisUp.ScriptProviders.Scripts
{
    public class DropJob : OrderedSqlScript
    {
        public DropJob(int order, JobConfiguration configuration)
        {
            Order = order;
            Name = "Drop Job";

            Contents = @"
USE [msdb]
GO

IF  EXISTS (SELECT * FROM msdb.dbo.sysjobs_view WHERE name = '@@JobName@@')
BEGIN
	DECLARE @JobID nvarchar(50)
	SELECT @JobID = (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = '@@JobName@@')
	EXEC msdb.dbo.sp_delete_job @job_id=@JobID, @delete_unused_schedule=1
END

GO"
                .Replace("@@JobName@@", configuration.JobName);
        }
    }
}
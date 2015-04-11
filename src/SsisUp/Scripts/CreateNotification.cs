using SsisUp.Configurations;

namespace SsisUp.Scripts
{
    public class CreateNotification : OrderedSqlScript
    {
        public CreateNotification(int order, JobConfiguration configuration)
        {
            Order = order;
            Name = "Create Notification";

            Contents = @"
USE [msdb]
GO

IF  NOT EXISTS (SELECT name FROM msdb.dbo.sysoperators WHERE name = N'@@OperatorName@@')

    EXEC msdb.dbo.sp_add_operator 
	    @name=N'@@OperatorName@@', 
	    @enabled=1, 
	    @weekday_pager_start_time=0, 
	    @weekday_pager_end_time=235959, 
	    @saturday_pager_start_time=0, 
	    @saturday_pager_end_time=235959, 
	    @sunday_pager_start_time=0, 
	    @sunday_pager_end_time=235959, 
	    @pager_days=127, 
	    @email_address=N'@@OperatorEmail@@',
	    @pager_address=N'@@OperatorEmail@@' 
GO

EXEC msdb.dbo.sp_update_job 
	@job_name=N'@@JobName@@', 
	@notify_level_email=2, 
	@notify_level_netsend=2, 
	@notify_level_page=3, 
	@notify_email_operator_name=N'@@OperatorName@@',
	@notify_page_operator_name=N'@@OperatorName@@'
GO
"
                .Replace("@@JobName@@", configuration.JobName)
                .Replace("@@OperatorName@@", configuration.Notification.Operator)
                .Replace("@@OperatorEmail@@", configuration.Notification.OperatorEmail);
        }
    }
}
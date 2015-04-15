using SsisUp.Builders;

namespace SsisUp.ScriptProviders.Scripts
{
    public class CreateSchedule : OrderedSqlScript
    {
        public CreateSchedule(int order, JobConfiguration jobConfiguration, ScheduleConfiguration scheduleConfiguration)
        {
            Order = order;
            Name = "Create Schedule";

            Contents = @"
USE [msdb]
GO
DECLARE @schedule_id int
EXEC msdb.dbo.sp_add_jobschedule @job_name='@@JobName@@', @name='@@ScheduleName@@', 
		@enabled=1, 
		@freq_type=8, 
		@freq_interval='@@FrequencyInterval@@', 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=1, 
		@active_start_date='@@ActiveDate@@', 
		@active_end_date=@@DeactiveDate@@, 
		@active_start_time='@@StartTime@@', 
		@active_end_time=235959, @schedule_id = @schedule_id OUTPUT
select @schedule_id

GO
"
                .Replace("@@JobName@@", jobConfiguration.JobName)
                .Replace("@@ScheduleName@@", scheduleConfiguration.ScheduleName)
                .Replace("@@FrequencyInterval@@", scheduleConfiguration.FrequencyInterval.ToString())
                .Replace("@@ActiveDate@@", scheduleConfiguration.ActiveDate)
                .Replace("@@DeactiveDate@@", scheduleConfiguration.DeactiveDate)
                .Replace("@@StartTime@@", scheduleConfiguration.StartTime);
        }
    }
}
using SsisUp.Configurations;

namespace SsisUp.Scripts
{
    public class CreateStep : OrderedSqlScript
    {
        public CreateStep(int order, JobConfiguration jonConfiguration, StepConfiguration stepConfiguration)
        {
            Order = order;
            Name = "Create Step";

            Contents = @"
IF '@@RunAs@@' = 'EMPTY'
BEGIN
	EXEC msdb.dbo.sp_add_jobstep @job_name='@@JobName@@', @step_name='@@StepName@@', 
			@step_id=@@StepId@@, 
			@cmdexec_success_code=0, 
			@on_success_action='@@SuccessActionId@@', 
			@on_fail_action='@@FailureActionId@@', 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem='@@SubSystem@@', 
			@command='@@Command@@', 
			@database_name=N'master', 
			@flags=0	
END 
ELSE 
BEGIN
	EXEC msdb.dbo.sp_add_jobstep @job_name='@@JobName@@', @step_name='@@StepName@@', 
			@step_id=@@StepId@@, 
			@cmdexec_success_code=0, 
			@on_success_action='@@SuccessActionId@@', 
			@on_fail_action='@@FailureActionId@@', 
			@retry_attempts=0, 
			@retry_interval=0, 
			@os_run_priority=0, @subsystem='@@SubSystem@@', 
			@command='@@Command@@', 
			@database_name=N'master', 
			@flags=0,
			@proxy_name=N'@@RunAs@@'
END
GO"
                .Replace("@@JobName@@", jonConfiguration.JobName)
                .Replace("@@StepName@@", stepConfiguration.StepName)
                .Replace("@@StepId@@", stepConfiguration.StepId.ToString())
                .Replace("@@SuccessActionId@@", ((int)stepConfiguration.SuccessAction).ToString())
                .Replace("@@FailureActionId@@", ((int)stepConfiguration.FailureAction).ToString())
                .Replace("@@SubSystem@@", stepConfiguration.SubSystem)
                .Replace("@@Command@@", stepConfiguration.Command)
                .Replace("@@RunAs@@", stepConfiguration.RunAs);                
        }
    }
}
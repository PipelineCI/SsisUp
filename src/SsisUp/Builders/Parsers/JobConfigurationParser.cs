using SsisUp.Configurations;

namespace SsisUp.Builders
{
    public interface IJobConfigurationParser
    {
        void Parse(JobConfiguration jobConfiguration);
        void Parse(StepConfiguration stepConfiguration);
        void Parse(ScheduleConfiguration scheduleConfiguration);
    }

    public class JobConfigurationParser : IJobConfigurationParser
    {
        public void Parse(JobConfiguration jobConfiguration)
        {
            if (string.IsNullOrEmpty(jobConfiguration.JobName))
                throw new JobConfigurationParserException("Job Name is missing from Job Configuration, please check your configuration setup");
            
            if (string.IsNullOrEmpty(jobConfiguration.SsisServer))
                throw new JobConfigurationParserException("SSIS Server is missing from Job Configuration, please check your configuration setup");
            
            if (string.IsNullOrEmpty(jobConfiguration.SsisOwner))
                throw new JobConfigurationParserException("SSIS Owner is missing from Job Configuration, please check your configuration setup");

            if (jobConfiguration.Steps != null && jobConfiguration.Steps.Count > 0)
            {
                foreach (var stepConfiguration in jobConfiguration.Steps)
                {
                    Parse(stepConfiguration);
                }
            }

            if (jobConfiguration.Schedules != null && jobConfiguration.Schedules.Count > 0)
            {
                foreach (var scheduleConfiguration in jobConfiguration.Schedules)
                {
                    Parse(scheduleConfiguration);
                }
            }
        }

        public void Parse(StepConfiguration stepConfiguration)
        {
            if (string.IsNullOrEmpty(stepConfiguration.StepName))
                throw new JobConfigurationParserException("Step Name is missing from Step Configuration, please check your configuration setup");
            
            if (string.IsNullOrEmpty(stepConfiguration.Command))
                throw new JobConfigurationParserException("Command is missing from Step Configuration, please check your configuration setup");
            
            if (stepConfiguration.StepId == 0)
                throw new JobConfigurationParserException("Step Id is missing from Step Configuration, please check your configuration setup");
            
            if (string.IsNullOrEmpty(stepConfiguration.SubSystem))
                throw new JobConfigurationParserException("Sub System is missing from Step Configuration, please check your configuration setup");

            if (stepConfiguration.SuccessAction == 0)
                throw new JobConfigurationParserException("Success Action is missing from Step Configuration, please check your configuration setup");

            if (stepConfiguration.FailureAction == 0)
                throw new JobConfigurationParserException("Failure Action is missing from Step Configuration, please check your configuration setup");

            if (stepConfiguration.SubSystem == SsisSubSystem.IntegrationServices)
            {
                
                if (string.IsNullOrEmpty(stepConfiguration.DtsxFile))
                    throw new JobConfigurationParserException("Sub System is missing from Step Configuration, please check your configuration setup");
                
                if (string.IsNullOrEmpty(stepConfiguration.DtsxConfigurationFile))
                    throw new JobConfigurationParserException("Sub System is missing from Step Configuration, please check your configuration setup");
                
                if (string.IsNullOrEmpty(stepConfiguration.DtsxFileDestination))
                    throw new JobConfigurationParserException("Dtsx File Destination is missing from Step Configuration, please check your configuration setup");
            }
        }

        public void Parse(ScheduleConfiguration scheduleConfiguration)
        {
            if (string.IsNullOrEmpty(scheduleConfiguration.ScheduleName))
                throw new JobConfigurationParserException("Schedule Name is missing from Schedule Configuration, please check your configuration setup");

            if (scheduleConfiguration.FrequencyInterval == 0)
                throw new JobConfigurationParserException("Set a fequency inteval for the Schedule Configuration, please check your configuration setup");

            if (string.IsNullOrEmpty(scheduleConfiguration.ActiveDate))
                throw new JobConfigurationParserException("Activate Date is missing from Schedule Configuration, please check your configuration setup");
            
            if (string.IsNullOrEmpty(scheduleConfiguration.DeactiveDate))
                throw new JobConfigurationParserException("Activate Date is missing from Schedule Configuration, please check your configuration setup");
            
            if (string.IsNullOrEmpty(scheduleConfiguration.StartTime))
                throw new JobConfigurationParserException("Activate Date is missing from Schedule Configuration, please check your configuration setup");
        }
    }
}
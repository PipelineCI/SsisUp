using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using SsisUp.Configurations;
using SsisUp.Scripts;

namespace SsisUp.Builders
{
    public interface ISqlScriptBuilder
    {
        IEnumerable<SqlScript> Build(JobConfiguration jobConfiguration);
    }

    public class SqlScriptBuilder : ISqlScriptBuilder
    {
        private readonly IJobConfigurationParser jobConfigurationParser;

        public SqlScriptBuilder(IJobConfigurationParser jobConfigurationParser)
        {
            if (jobConfigurationParser == null) throw new ArgumentNullException("jobConfigurationParser");
            this.jobConfigurationParser = jobConfigurationParser;
        }

        public IEnumerable<SqlScript> Build(JobConfiguration jobConfiguration)
        {
            jobConfigurationParser.Parse(jobConfiguration);

            var output = new List<SqlScript>();

            AddDropJobScript(jobConfiguration, output);
            AddCreateJobScript(jobConfiguration, output);
            AddScheduleScripts(jobConfiguration, output);
            AddStepScripts(jobConfiguration, output);
            AddNotificationScript(jobConfiguration, output);
            AddEnableJobScript(jobConfiguration, output);

            return output;
        }

        private static void AddEnableJobScript(JobConfiguration jobConfiguration, List<SqlScript> output)
        {
            var enableJob = new EnableJob(output.Count + 1, jobConfiguration);
            output.Add(new SqlScript(enableJob.FullName, enableJob.Contents));
        }

        private static void AddNotificationScript(JobConfiguration jobConfiguration, List<SqlScript> output)
        {
            if (jobConfiguration.Notification != null)
            {
                var notification = new CreateNotification(output.Count + 1, jobConfiguration);
                output.Add(new SqlScript(notification.FullName, notification.Contents));
            }
        }

        private static void AddStepScripts(JobConfiguration jobConfiguration, List<SqlScript> output)
        {
            foreach (
                var step in
                    jobConfiguration.Steps.Select(
                        stepConfiguration => new CreateStep(output.Count + 1, jobConfiguration, stepConfiguration)))
            {
                output.Add(new SqlScript(step.FullName, step.Contents));
            }
        }

        private static void AddScheduleScripts(JobConfiguration jobConfiguration, List<SqlScript> output)
        {
            foreach (
                var schedule in
                    jobConfiguration.Schedules.Select(
                        scheduleConfiguration => new CreateSchedule(output.Count + 1, jobConfiguration, scheduleConfiguration)))
            {
                output.Add(new SqlScript(schedule.FullName, schedule.Contents));
            }
        }

        private static void AddCreateJobScript(JobConfiguration jobConfiguration, List<SqlScript> output)
        {
            var createJob = new CreateJob(output.Count + 1, jobConfiguration);
            output.Add(new SqlScript(createJob.FullName, createJob.Contents));
        }

        private void AddDropJobScript(JobConfiguration jobConfiguration, List<SqlScript> scripts)
        {
            var dropJob = new DropJob(scripts.Count + 1, jobConfiguration);
            scripts.Add(new SqlScript(dropJob.FullName, dropJob.Contents));
        }
    }
}

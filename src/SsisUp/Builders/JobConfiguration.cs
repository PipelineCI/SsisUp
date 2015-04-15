using System.Collections.Generic;

namespace SsisUp.Builders
{
    public class JobConfiguration
    {
        public static JobConfiguration Configuration { get; private set; }

        public bool IsEnabled { get; private set; }
        public string JobName { get; private set; }
        public string SsisOwner { get; private set; }
        public string SsisServer { get; private set; }
        public string JobDescription { get; private set; }

        public List<StepConfiguration> Steps { get; private set; }
        public List<ScheduleConfiguration> Schedules { get; private set; }
        public NotificationConfiguration Notification { get; private set; }

        public static JobConfiguration Create()
        {
            Configuration = new JobConfiguration();
            return Configuration;
        }
        
        public JobConfiguration WithName(string name)
        {
            JobName = name;
            return Configuration;
        }

        public JobConfiguration WithDescription(string description)
        {
            JobDescription = description;
            return Configuration;
        }

        public JobConfiguration WithSsisOwner(string owner)
        {
            SsisOwner = owner;
            return Configuration;
        }

        public JobConfiguration WithSsisServer(string server)
        {
            SsisServer = server;
            return Configuration;
        }

        public JobConfiguration WithSchedule(ScheduleConfiguration configuration)
        {
            if (Configuration.Schedules == null)
                Configuration.Schedules = new List<ScheduleConfiguration>();

            Configuration.Schedules.Add(configuration);

            return Configuration;
        }

        public JobConfiguration WithStep(StepConfiguration configuration)
        {
            if (Configuration.Steps == null)
                Configuration.Steps = new List<StepConfiguration>();

            Configuration.Steps.Add(configuration);

            return Configuration;
        }

        public JobConfiguration WithNotification(NotificationConfiguration configuration)
        {
            Configuration.Notification = configuration;

            return Configuration;
        }

        public JobConfiguration Enabled()
        {
            Configuration.IsEnabled = true;
            return Configuration;
        }

        public JobConfiguration Disabled()
        {
            Configuration.IsEnabled = false;
            return Configuration;
        }
        
    }
}
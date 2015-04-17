using System.Collections.Generic;

namespace SsisUp.Builders
{
    public class JobConfiguration
    {
        private static JobConfiguration Configuration { get; set; }

        public bool IsEnabled { get; private set; }
        public string JobName { get; private set; }
        public string SsisOwner { get; private set; }
        public string SsisServer { get; private set; }
        public string JobDescription { get; private set; }

        public List<StepConfiguration> Steps { get; private set; }
        public List<ScheduleConfiguration> Schedules { get; private set; }
        public NotificationConfiguration Notification { get; private set; }

        /// <summary>
        /// Create a JobConfiguration. You must call this static method first. PLEASE NOTE: Job is enabled by default.
        /// </summary>
        /// <returns>JobConfiguration</returns>
        public static JobConfiguration Create()
        {
            Configuration = new JobConfiguration {IsEnabled = true};
            return Configuration;
        }
        
        /// <summary>
        /// Used to give the MS SQL Server Job a name.
        /// </summary>
        /// <param name="name">Give the job a name.</param>
        /// <returns>JobConfiration</returns>
        public JobConfiguration WithName(string name)
        {
            JobName = name;
            return Configuration;
        }

        /// <summary>
        /// Used to give the MS SQL Server Job a description.
        /// </summary>
        /// <param name="description">Give the job a description.</param>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration WithDescription(string description)
        {
            JobDescription = description;
            return Configuration;
        }

        /// <summary>
        /// Used to give the MS SQL Server Job an owner. This can be any Login that has the correct permissions on that MS SQL Server instance.
        /// </summary>
        /// <param name="owner">Give the job an owner. i.e. 'sa'</param>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration WithSsisOwner(string owner)
        {
            SsisOwner = owner;
            return Configuration;
        }

        /// <summary>
        /// Used to specify the server in which th MS SQL Server Job runs on.
        /// </summary>
        /// <param name="server">specify the machine name</param>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration WithSsisServer(string server)
        {
            SsisServer = server;
            return Configuration;
        }

        /// <summary>
        /// Used to Configure the Schedule for the Job. This is required if you would like to have the job run on a schedule.
        /// </summary>
        /// <param name="configuration">Create a new ScheduleConfiguration</param>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration WithSchedule(ScheduleConfiguration configuration)
        {
            if (Configuration.Schedules == null)
                Configuration.Schedules = new List<ScheduleConfiguration>();

            Configuration.Schedules.Add(configuration);

            return Configuration;
        }

        /// <summary>
        /// Used to Configure the Steps of the Job. At least one step is required. Only Cmd, Sql and Integration Services steps are currently supported.
        /// </summary>
        /// <param name="configuration">Create a new StepConfiguration</param>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration WithStep(StepConfiguration configuration)
        {
            if (Configuration.Steps == null)
                Configuration.Steps = new List<StepConfiguration>();

            Configuration.Steps.Add(configuration);

            return Configuration;
        }

        /// <summary>
        /// Used to Configure the Steps of the Job. At least one step is required. Only Cmd, Sql and Integration Services steps are currently supported.
        /// </summary>
        /// <param name="configuration">Create a new NotificationConfiguration</param>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration WithNotification(NotificationConfiguration configuration)
        {
            Configuration.Notification = configuration;

            return Configuration;
        }

        /// <summary>
        /// This will mark the job as Disabled. PLEASE NOTE: Job is enabled by default.
        /// </summary>
        /// <returns>JobConfiguration</returns>
        public JobConfiguration Disabled()
        {
            Configuration.IsEnabled = false;
            return Configuration;
        }
        
    }
}
 using System;
 using SsisUp.Builders.References;

namespace SsisUp.Builders
{
    public class ScheduleConfiguration
    {
        private static ScheduleConfiguration Configuration { get; set; }

        public string StartTime { get; private set; }
        public string ActiveDate { get; private set; }
        public string DeactiveDate { get; private set; }
        public string ScheduleName { get; private set; }
        public int FrequencyInterval { get; private set; }

        /// <summary>
        /// Create a ScheduleConfiguration. This is used to create a schedule for the Job. i.e. If you would like the job to execute every Saturday at 10am.
        /// </summary>
        /// <returns>ScheduleConfiguration</returns>
        public static ScheduleConfiguration Create()
        {
            Configuration = new ScheduleConfiguration();
            return Configuration;
        }

        /// <summary>
        /// Used to give the job step a name.
        /// </summary>
        /// <param name="name">Name the step</param>
        /// <returns>ScheduleConfiguration</returns>
        public ScheduleConfiguration WithName(string name)
        {
            Configuration.ScheduleName = name;
            return Configuration;
        }
        
        /// <summary>
        /// Used to specify which days you would like the job to run on. Can be called multiple times. i.e. RunOn(FrequencyDay.Monday).RunOn(FrequencyDay.Tuesday)
        /// </summary>
        /// <param name="day">specify the date from available enum values</param>
        /// <returns>ScheduleConfiguration</returns>
        public ScheduleConfiguration RunOn(FrequencyDay day)
        {
            Configuration.FrequencyInterval += (int)day;
            return Configuration;
        }

        /// <summary>
        /// Used to specify what time you would like the job to start at.
        /// </summary>
        /// <param name="time">Specify the time as a TimeSpan</param>
        /// <returns>ScheduleConfiguration</returns>
        public ScheduleConfiguration StartingAt(TimeSpan time)
        {
            Configuration.StartTime = time.ToString("hhmmss");
            return Configuration;
        }

        /// <summary>
        /// Used to specify when you would like the job to be activated from.
        /// </summary>
        /// <param name="date">Specify the date as a DateTime</param>
        /// <returns>ScheduleConfiguration</returns>
        public ScheduleConfiguration ActivatedFrom(DateTime date)
        {
            ActiveDate = date.ToString("yyyyMMdd");
            return Configuration;
        }

        /// <summary>
        /// Used to specify when you would like the job to be deactivated.
        /// </summary>
        /// <param name="date">Specify the date as a DateTime</param>
        /// <returns>ScheduleConfiguration</returns>
        public ScheduleConfiguration ActivatedUntil(DateTime date)
        {
            DeactiveDate = date.ToString("yyyyMMdd");
            return Configuration;
        }

    }
}
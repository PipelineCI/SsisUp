 using System;
 using SsisUp.Builders.References;

namespace SsisUp.Builders
{
    public class ScheduleConfiguration
    {
        public static ScheduleConfiguration Configuration { get; private set; }

        public string ScheduleName { get; set; }
        public int FrequencyInterval { get; set; }
        public string ActiveDate { get; set; }
        public string DeactiveDate { get; set; }
        public string StartTime { get; set; }

        public static ScheduleConfiguration Create()
        {
            Configuration = new ScheduleConfiguration();
            return Configuration;
        }

        public ScheduleConfiguration WithName(string name)
        {
            Configuration.ScheduleName = name;
            return Configuration;
        }
        
        public ScheduleConfiguration RunOn(FrequencyDay day)
        {
            Configuration.FrequencyInterval += (int)day;
            return Configuration;
        }

        public ScheduleConfiguration StartingAt(TimeSpan time)
        {
            Configuration.StartTime = time.ToString("hhmmss");
            return Configuration;
        }

        public ScheduleConfiguration ActivatedFrom(DateTime date)
        {
            ActiveDate = date.ToString("yyyyMMdd");
            return Configuration;
        }

        public ScheduleConfiguration ActivatedUntil(DateTime date)
        {
            DeactiveDate = date.ToString("yyyyMMdd");
            return Configuration;
        }

    }
}
namespace SsisUp.Builders
{
    public class NotificationConfiguration
    {
        public static NotificationConfiguration Configuration { get; private set; }

        public string OperatorEmail { get; set; }
        public string Operator { get; set; }
        
        public static NotificationConfiguration Create()
        {
            Configuration = new NotificationConfiguration();
            return Configuration;
        }

        public NotificationConfiguration WithOperator(string name)
        {
            Configuration.Operator = name;
            return Configuration;
        }

        public NotificationConfiguration WithOperatorEmail(string email)
        {
            Configuration.OperatorEmail = email;
            return Configuration;
        }
    }
}
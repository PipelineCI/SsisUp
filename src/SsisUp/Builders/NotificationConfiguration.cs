namespace SsisUp.Builders
{
    public class NotificationConfiguration
    {
        private static NotificationConfiguration Configuration { get; set; }

        public string Operator { get; private set; }
        public string OperatorEmail { get; private set; }
        
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
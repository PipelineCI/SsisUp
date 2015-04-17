namespace SsisUp.Builders
{
    public class NotificationConfiguration
    {
        private static NotificationConfiguration Configuration { get; set; }

        public string Operator { get; private set; }
        public string OperatorEmail { get; private set; }
        
        /// <summary>
        /// Create a NotificationConfiguration. You must call this static method first. PLEASE NOTE: Job is enabled by default.
        /// </summary>
        /// <returns>NotificationConfiguration</returns>
        public static NotificationConfiguration Create()
        {
            Configuration = new NotificationConfiguration();
            return Configuration;
        }

        /// <summary>
        /// Used to select the notification operator for the Job. Please ensure this exists on the MS SQL Server instance.
        /// </summary>
        /// <param name="name">The name of the operator</param>
        /// <returns>NotificationConfiguration</returns>
        public NotificationConfiguration WithOperator(string name)
        {
            Configuration.Operator = name;
            return Configuration;
        }

        /// <summary>
        /// Used to select the notification operator email for the Job. Please ensure this exists on the MS SQL Server instance.
        /// </summary>
        /// <param name="email">The email address of the operator</param>
        /// <returns>NotificationConfiguration</returns>
        public NotificationConfiguration WithOperatorEmail(string email)
        {
            Configuration.OperatorEmail = email;
            return Configuration;
        }
    }
}
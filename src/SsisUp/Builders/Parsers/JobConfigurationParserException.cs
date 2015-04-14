using System;

namespace SsisUp.Builders
{
    public class JobConfigurationParserException : Exception
    {
        public JobConfigurationParserException(string message) : base(message)
        {
        }
    }
}
using System;

namespace SsisUp.Builders.Parsers
{
    public class JobConfigurationParserException : Exception
    {
        public JobConfigurationParserException(string message) : base(message)
        {
        }
    }
}
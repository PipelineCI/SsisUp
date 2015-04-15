using System;

namespace SsisUp.Services
{
    public class DeploymentResult
    {
        public Exception Error { get; private set; }
        public bool Successful { get; private set; }
        public Type ExecutingType { get; private set; }

        public DeploymentResult(Exception error, bool successful, Type executingType)
        {
            this.Error = error;
            this.Successful = successful;
            this.ExecutingType = executingType;
        }

        public override string ToString()
        {
            return string.Format("DEPLOYMENT RESULT: {0} === EXECUTING TYPE: {2} === ERROR: {3}",
                Successful.ToString(),
                ExecutingType.Name,
                Error);
        }
    }
}

using SsisUp.Builders.References;

namespace SsisUp.Builders
{
    public class StepConfiguration
    {
        private static StepConfiguration Configuration { get; set; }

        public int StepId { get; private set; }
        public string RunAs { get; private set; }
        public string Command { get; private set; }
        public string DtsxFile { get; private set; }
        public string StepName { get; private set; }
        public string SubSystem { get; private set; }
        public JobAction SuccessAction { get; private set; }
        public JobAction FailureAction { get; private set; }
        public string DtsxFileDestination { get; private set; }
        public string DtsxConfigurationFile { get; private set; }


        public static StepConfiguration Create()
        {
            Configuration = new StepConfiguration();
            SetDefaults();
            return Configuration;
        }

        private static void SetDefaults()
        {
            Configuration.RunAs = "";
            Configuration.FailureAction = JobAction.QuitWithFailure;
        }

        public StepConfiguration WithName(string name)
        {
            StepName = name;
            return Configuration;
        }

        public StepConfiguration WithId(int id)
        {
            Configuration.StepId = id;
            return Configuration;
        }

        public StepConfiguration WithSubSystem(string subSystem)
        {
            Configuration.SubSystem = subSystem;
            return Configuration;
        }

        public StepConfiguration OnSuccess(JobAction onSuccess)
        {
            Configuration.SuccessAction = onSuccess;
            return Configuration;
        }

        public StepConfiguration OnFailure(JobAction onFailure)
        {
            Configuration.FailureAction = onFailure;
            return Configuration;
        }

        public StepConfiguration RunStepAs(string userName)
        {
            Configuration.RunAs = userName;
            return Configuration;
        }

        public StepConfiguration ExecuteCommand(string command)
        {
            Configuration.Command = command;
            return Configuration;
        }

        public StepConfiguration WithDtsxFile(string fileName)
        {
            Configuration.DtsxFile = fileName;
            return Configuration;
        }

        public StepConfiguration WithDtsConfigurationFile(string configFileName)
        {
            Configuration.DtsxConfigurationFile = configFileName;
            return Configuration;
        }

        public StepConfiguration WithDtsxFileDestination(string destination)
        {
            Configuration.DtsxFileDestination = destination;
            return Configuration;
        }
    }
}
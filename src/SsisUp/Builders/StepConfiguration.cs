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


        /// <summary>
        /// Create a StepConfiguration. This is used to create a step for the Job.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Create a name for the Step
        /// </summary>
        /// <param name="name">Name the step</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration WithName(string name)
        {
            StepName = name;
            return Configuration;
        }

        /// <summary>
        /// The Step needs an Id, this needs to be an integer.
        /// </summary>
        /// <param name="id">Give the step an Id</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration WithId(int id)
        {
            Configuration.StepId = id;
            return Configuration;
        }

        /// <summary>
        /// Specify the SubSystem the Step will use. IntegrationService, Cmd or Sql
        /// </summary>
        /// <param name="subSystem"></param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration WithSubSystem(string subSystem)
        {
            Configuration.SubSystem = subSystem;
            return Configuration;
        }

        /// <summary>
        /// Specify the action to take if the step is successful.
        /// </summary>
        /// <param name="onSuccess">JobAction Enum to specify next steps</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration OnSuccess(JobAction onSuccess)
        {
            Configuration.SuccessAction = onSuccess;
            return Configuration;
        }

        /// <summary>
        /// Specify the action to take if the step fails.
        /// </summary>
        /// <param name="onFailure">JobAction Enum to specify next steps</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration OnFailure(JobAction onFailure)
        {
            Configuration.FailureAction = onFailure;
            return Configuration;
        }

        /// <summary>
        /// Specify a user to run the step as. There may be a specific step that requires an elevated user account in order to run it.
        /// </summary>
        /// <param name="userName">Specify a user name</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration RunStepAs(string userName)
        {
            Configuration.RunAs = userName;
            return Configuration;
        }

        /// <summary>
        /// This is the command the Step will execute
        /// </summary>
        /// <param name="command">Please specify the command</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration ExecuteCommand(string command)
        {
            Configuration.Command = command;
            return Configuration;
        }

        /// <summary>
        /// If this is an Integration Services Step, then you must specify the Dtsx file
        /// </summary>
        /// <param name="fileName">Dtsx file location</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration WithDtsxFile(string fileName)
        {
            Configuration.DtsxFile = fileName;
            return Configuration;
        }

        /// <summary>
        /// Use this if your SSIS project uses package configurations
        /// </summary>
        /// <param name="configFileName">dtsConfig file location</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration WithDtsConfigurationFile(string configFileName)
        {
            Configuration.DtsxConfigurationFile = configFileName;
            return Configuration;
        }

        /// <summary>
        /// Please specify the File Destination for the Dtsx and dtsConfig files
        /// </summary>
        /// <param name="destination">Specify the destination of the file(s)</param>
        /// <returns>StepConfiguration</returns>
        public StepConfiguration WithDtsxFileDestination(string destination)
        {
            Configuration.DtsxFileDestination = destination;
            return Configuration;
        }
    }
}
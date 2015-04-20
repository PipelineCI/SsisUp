using System;
using DbUp.Engine;
using DbUp.Helpers;
using SsisUp.Builders.Parsers;
using SsisUp.Helpers;
using SsisUp.ScriptProviders;
using SsisUp.Services;

namespace SsisUp.Builders
{
    public class DeploymentConfiguration
    {
        private static DeploymentConfiguration Configuration { get; set; }

        public bool Debug { get; private set; }
        public string ConnectionString { get; set; }
        public JobConfiguration JobConfiguration { get; private set; }

        public IFileService FileService { get; private set; }
        public ISqlScriptProvider SqlScriptProvider { get; private set; }
        public IDeploymentService DeploymentService { get; private set; }
        public ISqlExecutionService SqlExecutionService { get; private set; }
        public IJobConfigurationParser JobConfigurationParser { get; private set; }

        /// <summary>
        /// Create a deployment configuration to deploy the Job
        /// </summary>
        /// <returns>DeploymentConfiguration</returns>
        public static DeploymentConfiguration Create()
        {
            Configuration = new DeploymentConfiguration
            {
                Debug = false,
                FileService = new IntegrationServicesFileService(new IoWrapper()),
                SqlExecutionService = new SqlExecutionService(),
                JobConfigurationParser = new JobConfigurationParser()
            };

            Configuration.SqlScriptProvider =
                new SqlScriptProvider(Configuration.JobConfigurationParser);
            
            Configuration.DeploymentService = new DeploymentService(Configuration.JobConfigurationParser,
                Configuration.SqlScriptProvider, Configuration.FileService, Configuration.SqlExecutionService);
            
            return Configuration;
        }

        /// <summary>
        /// Specify the connection string to the master database on the server you would like to deploy the Job too
        /// </summary>
        /// <param name="connectionString">Connection String to master database</param>
        /// <returns>DeploymentConfiguration</returns>
        public DeploymentConfiguration ToDatabase(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
            return Configuration;
        }

        /// <summary>
        /// Specify the jobConfiguration you would like to deploy
        /// </summary>
        /// <param name="jobConfiguration">Specify the jobConfiguration</param>
        /// <returns>DeploymentConfiguration</returns>
        public DeploymentConfiguration WithJobConfiguration(JobConfiguration jobConfiguration)
        {
            if (jobConfiguration == null) throw new ArgumentNullException("jobConfiguration");
            Configuration.JobConfiguration = jobConfiguration;
            return Configuration;
        }

        /// <summary>
        /// Enabling logging will log more info to the console out
        /// </summary>
        /// <returns>DeploymentConfiguration</returns>
        public DeploymentConfiguration EnableVerboseLogging()
        {
            Configuration.Debug = true;
            return Configuration;
        }

        /// <summary>
        /// Will deploy the configuration
        /// </summary>
        /// <returns>DeploymentResult</returns>
        public DeploymentResult Deploy()
        {
            return DeploymentService.Execute(Configuration);
        }
    }
}
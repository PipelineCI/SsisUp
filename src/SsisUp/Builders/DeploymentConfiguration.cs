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
        public IJournal DbUpJournal { get; private set; }
        public JobConfiguration JobConfiguration { get; private set; }

        public IFileService FileService { get; private set; }
        public ISqlScriptProvider SqlScriptProvider { get; private set; }
        public IDeploymentService DeploymentService { get; private set; }
        public ISqlExecutionService SqlExecutionService { get; private set; }
        public IJobConfigurationParser JobConfigurationParser { get; private set; }

        public static DeploymentConfiguration Create()
        {
            Configuration = new DeploymentConfiguration
            {
                DbUpJournal = new NullJournal(),
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

        public DeploymentConfiguration WithCustomFileService(IFileService fileService)
        {
            Configuration.FileService = fileService;
            return Configuration;
        }

        public DeploymentConfiguration WithCustomSqlExecutionService(ISqlExecutionService sqlExecutionService)
        {
            Configuration.SqlExecutionService = sqlExecutionService;
            return Configuration;
        }

        public DeploymentConfiguration WithCustomJobConfigurationParser(IJobConfigurationParser jobConfigurationParser)
        {
            Configuration.JobConfigurationParser = jobConfigurationParser;
            return Configuration;
        }

        public DeploymentConfiguration WithCustomDeploymentService(IDeploymentService deploymentService)
        {
            Configuration.DeploymentService = deploymentService;
            return Configuration;
        }

        public DeploymentConfiguration WithCustomSqlScriptProvider(ISqlScriptProvider sqlScriptProvider)
        {
            Configuration.SqlScriptProvider = sqlScriptProvider;
            return Configuration;
        }

        public DeploymentConfiguration ToDatabase(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
            return Configuration;
        }

        public DeploymentConfiguration WithJobConfiguration(JobConfiguration jobConfiguration)
        {
            if (jobConfiguration == null) throw new ArgumentNullException("jobConfiguration");
            Configuration.JobConfiguration = jobConfiguration;
            return Configuration;
        }

        public DeploymentConfiguration WithJournal(IJournal journal)
        {
            if (journal == null) throw new ArgumentNullException("journal");
            Configuration.DbUpJournal = journal;
            return Configuration;
        }

        public DeploymentConfiguration EnableVerboseLogging()
        {
            Configuration.Debug = true;
            return Configuration;
        }

        public int Deploy()
        {
            return DeploymentService.Execute(Configuration);
        }
    }
}
using System;
using DbUp.Engine;
using DbUp.Helpers;
using SsisUp.Builders;
using SsisUp.Configurations;
using SsisUp.Helpers;

namespace SsisUp.Services
{
    public class Deployment
    {
        private ISqlExecutionService _sqlExecutionService;
        private IFileService _fileService;
        private IJobConfigurationParser _jobConfigurationParser;
        private ISqlScriptBuilder _sqlScriptBuilder;

        private static Deployment DeploymentConfiguration { get; set; }

        public JobConfiguration JobConfiguration { get; set; }
        public IJournal DbUpJournal { get; set; }
        public bool Debug { get; set; }
        public string ConnectionString { get; set; }

        public static Deployment Create()
        {
            DeploymentConfiguration = new Deployment {DbUpJournal = new NullJournal(), Debug = false};
            return DeploymentConfiguration;
        }

        public int Deploy()
        {
            InstallDependencies();

            _jobConfigurationParser.Parse(DeploymentConfiguration.JobConfiguration);

            var scripts = _sqlScriptBuilder.Build(DeploymentConfiguration.JobConfiguration);
            var exitCode = _sqlExecutionService.Execute(DeploymentConfiguration.ConnectionString, scripts, DeploymentConfiguration.Debug);

            if (exitCode != 0)
                return exitCode;

            exitCode = _fileService.Deploy(DeploymentConfiguration.JobConfiguration);

            return exitCode;
        }

        private void InstallDependencies()
        {
            var ioWrapper = new IoWrapper();
            _fileService = new IntegrationServicesFileService(ioWrapper);
            _jobConfigurationParser = new JobConfigurationParser();
            _sqlExecutionService = new SqlExecutionService();
            _sqlScriptBuilder = new SqlScriptBuilder(_jobConfigurationParser);
        }

        public Deployment ToDatabase(string connectionString)
        {
            DeploymentConfiguration.ConnectionString = connectionString;
            return DeploymentConfiguration;
        }

        public Deployment WithJobConfiguration(JobConfiguration jobConfiguration)
        {
            if (jobConfiguration == null) throw new ArgumentNullException("jobConfiguration");
            DeploymentConfiguration.JobConfiguration = jobConfiguration;
            return DeploymentConfiguration;
        }

        public Deployment WithJournal(IJournal journal)
        {
            if (journal == null) throw new ArgumentNullException("journal");
            DeploymentConfiguration.DbUpJournal = journal;
            return DeploymentConfiguration;
        }

        public Deployment EnableVerboseLogging()
        {
            DeploymentConfiguration.Debug = true;
            return DeploymentConfiguration;
        }

    }
}
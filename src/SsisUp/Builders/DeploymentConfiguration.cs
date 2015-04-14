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
        private static Deployment DeploymentConfiguration { get; set; }

        public JobConfiguration JobConfiguration { get; set; }
        public IJournal DbUpJournal { get; set; }
        public bool Debug { get; set; }
        public string ConnectionString { get; set; }

        public IFileService FileService { get; set; }
        public ISqlExecutionService SqlExecutionService { get; set; }
        public IJobConfigurationParser JobConfigurationParser { get; set; }


        public static Deployment Create()
        {
            DeploymentConfiguration = new Deployment
            {
                DbUpJournal = new NullJournal(),
                Debug = false,
                FileService = new IntegrationServicesFileService(new IoWrapper()),
                SqlExecutionService = new SqlExecutionService(),
                JobConfigurationParser = new JobConfigurationParser(),
            };

            DeploymentConfiguration.SqlScriptProvider =
                new SqlScriptProvider(DeploymentConfiguration.JobConfigurationParser);
            
            return DeploymentConfiguration;
        }

        public SqlScriptProvider SqlScriptProvider { get; set; }

        public Deployment WithCustomFileService(IFileService fileService)
        {
            DeploymentConfiguration.FileService = fileService;
            return DeploymentConfiguration;
        }

        public Deployment WithCustomSqlExecutionService(ISqlExecutionService sqlExecutionService)
        {
            DeploymentConfiguration.SqlExecutionService = sqlExecutionService;
            return DeploymentConfiguration;
        }

        public Deployment WithCustomJobConfigurationParser(IJobConfigurationParser jobConfigurationParser)
        {
            DeploymentConfiguration.JobConfigurationParser = jobConfigurationParser;
            return DeploymentConfiguration;
        }


        public int Deploy()
        {
            JobConfigurationParser.Parse(DeploymentConfiguration.JobConfiguration);

            var scripts = SqlScriptProvider.Build(DeploymentConfiguration.JobConfiguration);
            var exitCode = SqlExecutionService.Execute(DeploymentConfiguration.ConnectionString, scripts, DeploymentConfiguration.Debug);

            if (exitCode != 0)
                return exitCode;

            exitCode = FileService.Deploy(DeploymentConfiguration.JobConfiguration);

            return exitCode;
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
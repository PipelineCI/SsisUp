using System;
using SsisUp.Builders;
using SsisUp.Builders.Parsers;
using SsisUp.ScriptProviders;

namespace SsisUp.Services
{
    public interface IDeploymentService
    {
        int Execute(DeploymentConfiguration configuration);
    }

    public class DeploymentService : IDeploymentService
    {
        private readonly IJobConfigurationParser _jobConfigurationParser;
        private readonly ISqlScriptProvider _sqlScriptProvider;
        private readonly IFileService _fileService;
        private readonly ISqlExecutionService _sqlExecutionService;

        public DeploymentService(IJobConfigurationParser jobConfigurationParser,
            ISqlScriptProvider sqlScriptProvider,
            IFileService fileService,
            ISqlExecutionService sqlExecutionService
            )
        {
            if (jobConfigurationParser == null) throw new ArgumentNullException("jobConfigurationParser");
            if (sqlScriptProvider == null) throw new ArgumentNullException("sqlScriptProvider");
            if (fileService == null) throw new ArgumentNullException("fileService");
            if (sqlExecutionService == null) throw new ArgumentNullException("sqlExecutionService");
            _jobConfigurationParser = jobConfigurationParser;
            _sqlScriptProvider = sqlScriptProvider;
            _fileService = fileService;
            _sqlExecutionService = sqlExecutionService;
        }

        public int Execute(DeploymentConfiguration configuration)
        {
            _jobConfigurationParser.Parse(configuration.JobConfiguration);

            var scripts = _sqlScriptProvider.Build(configuration.JobConfiguration);
            var exitCode = _sqlExecutionService.Execute(configuration.ConnectionString, scripts, configuration.Debug);

            if (exitCode != 0)
                return exitCode;

            exitCode = _fileService.Execute(configuration.JobConfiguration);

            return exitCode;
        }
    }
}
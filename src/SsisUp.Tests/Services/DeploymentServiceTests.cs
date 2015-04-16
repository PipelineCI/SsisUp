using System;
using System.Collections.Generic;
using DbUp.Engine;
using Moq;
using NUnit.Framework;
using SsisUp.Builders;
using SsisUp.Builders.Parsers;
using SsisUp.ScriptProviders;
using SsisUp.Services;

namespace SsisUp.Tests.Services
{
    [TestFixture]
    public class DeploymentServiceTests
    {
        private Mock<IJobConfigurationParser> _mockJobConfigurationParser;
        private Mock<ISqlExecutionService> _mockSqlExecutionService;
        private Mock<IFileService> _mockFileService;
        private Mock<ISqlScriptProvider> _mockSqlScriptProvider;
        private DeploymentService _deploymentService;

        [SetUp]
        public void SetUp()
        {
            _mockJobConfigurationParser = new Mock<IJobConfigurationParser>();
            _mockSqlExecutionService = new Mock<ISqlExecutionService>();
            _mockFileService = new Mock<IFileService>();
            _mockSqlScriptProvider = new Mock<ISqlScriptProvider>();
            _deploymentService = new DeploymentService(_mockJobConfigurationParser.Object,
                _mockSqlScriptProvider.Object,
                _mockFileService.Object,
                _mockSqlExecutionService.Object);
        }

        [Test]
        public void Verify_that_DeploymentService_Execute_returns_unsuccessful_result_when_SqlExecutionService_fails_and_does_not_call_FileService()
        {
            _mockJobConfigurationParser.Setup(x => x.Parse(It.IsAny<JobConfiguration>()));
            _mockSqlScriptProvider.Setup(x => x.Build(It.IsAny<JobConfiguration>()));
            _mockSqlExecutionService.Setup(
                x => x.Execute(It.IsAny<string>(), It.IsAny<List<SqlScript>>(), It.IsAny<bool>()))
                .Returns(new DeploymentResult(new Exception("Test"), false, typeof (SqlExecutionService)));

            
            _deploymentService.Execute(new DeploymentConfiguration());

            _mockJobConfigurationParser.Verify(x => x.Parse(It.IsAny<JobConfiguration>()), Times.Once());
            _mockSqlScriptProvider.Verify(x => x.Build(It.IsAny<JobConfiguration>()),Times.Once);
            _mockSqlExecutionService.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<List<SqlScript>>(), It.IsAny<bool>()),Times.Once);
            _mockFileService.Verify(x => x.Execute(It.IsAny<JobConfiguration>()), Times.Never);
        }

        [Test]
        public void Verify_that_DeploymentService_Execute_returns_successful_result_when_SqlExecutionService_and_FileService_does_not_fail()
        {
            _mockJobConfigurationParser.Setup(x => x.Parse(It.IsAny<JobConfiguration>()));
            _mockSqlScriptProvider.Setup(x => x.Build(It.IsAny<JobConfiguration>()));
            _mockSqlExecutionService.Setup(
                x => x.Execute(It.IsAny<string>(), It.IsAny<List<SqlScript>>(), It.IsAny<bool>()))
                .Returns(new DeploymentResult(null, true, typeof(SqlExecutionService)));
            _mockFileService.Setup(x => x.Execute(It.IsAny<JobConfiguration>()))
                .Returns(new DeploymentResult(null, true, typeof(IntegrationServicesFileService)));

            _deploymentService.Execute(new DeploymentConfiguration());

            _mockJobConfigurationParser.Verify(x => x.Parse(It.IsAny<JobConfiguration>()), Times.Once());
            _mockSqlScriptProvider.Verify(x => x.Build(It.IsAny<JobConfiguration>()), Times.Once);
            _mockSqlExecutionService.Verify(x => x.Execute(It.IsAny<string>(), It.IsAny<List<SqlScript>>(), It.IsAny<bool>()), Times.Once);
            _mockFileService.Verify(x => x.Execute(It.IsAny<JobConfiguration>()), Times.Once);
        }
    }
}

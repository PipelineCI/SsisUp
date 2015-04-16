using Moq;
using NUnit.Framework;
using SsisUp.Builders;
using SsisUp.Builders.References;
using SsisUp.Helpers;
using SsisUp.Services;

namespace SsisUp.Tests.Services
{
    [TestFixture]
    public class IntegrationServicesFileServiceTests
    {
        [Test]
        public void FileService_Execute_should_return_successful_result_if_the_job_step_is_not_an_integration_services_job()
        {
            var jobConfiguration =
                JobConfiguration.Create()
                    .WithName("Test Job")
                    .WithStep(StepConfiguration.Create()
                        .WithName("Test Step")
                        .WithSubSystem(SsisSubSystem.CommandExecutable));

            var mockIoWrapper = new Mock<IIoWrapper>();

            var fileService = new IntegrationServicesFileService(mockIoWrapper.Object);

            var result = fileService.Execute(jobConfiguration);

            Assert.That(result.Successful, Is.EqualTo(true));
        }

        [Test]
        public void FileService_Execute_should_return_unsuccessful_result_if_the_step_is_an_integration_services_job_step_and_an_exception_is_thrown()
        {
            var jobConfiguration =
                JobConfiguration.Create()
                    .WithName("Test Job")
                    .WithStep(StepConfiguration.Create()
                        .WithName("Test Step")
                        .WithSubSystem(SsisSubSystem.IntegrationServices));

            var fileService = new IntegrationServicesFileService(new IoWrapper());

            var result = fileService.Execute(jobConfiguration);

            Assert.That(result.Successful, Is.EqualTo(false));
        }

        [Test]
        public void FileService_Execute_should_return_sucessful_result_if_the_step_is_an_integration_services_job_step_and_NO_exceptions_are_thrown()
        {
            var jobConfiguration =
                JobConfiguration.Create()
                    .WithName("Test Job")
                    .WithStep(StepConfiguration.Create()
                        .WithName("Test Step")
                        .WithSubSystem(SsisSubSystem.IntegrationServices));

            var mockIoWrapper = new Mock<IIoWrapper>();
            
            var fileService = new IntegrationServicesFileService(mockIoWrapper.Object);

            var result = fileService.Execute(jobConfiguration);

            Assert.That(result.Successful, Is.EqualTo(true));
            mockIoWrapper.Verify(x => x.CreateDirectoryIfNotExists(It.IsAny<string>()), Times.Once());
            mockIoWrapper.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}

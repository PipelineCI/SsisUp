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
        public void Deploy_should_return_0_if_the_package_is_not_an_integration_services_job()
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
        public void Deploy_should_return_negative_1_if_the_package_is_an_integration_services_job_and_an_exception_is_thrown()
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
        public void Deploy_should_return_0_if_the_package_is_an_integration_services_job_and_NO_exceptions_are_thrown()
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

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

            var result = fileService.Deploy(jobConfiguration);

            Assert.That(result, Is.EqualTo(0));
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

            var result = fileService.Deploy(jobConfiguration);

            Assert.That(result, Is.EqualTo(-1));
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

            var result = fileService.Deploy(jobConfiguration);

            Assert.That(result, Is.EqualTo(0));
            mockIoWrapper.Verify(x => x.CreateDirectoryIfNotExists(It.IsAny<string>()), Times.Once());
            mockIoWrapper.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }
    }
}

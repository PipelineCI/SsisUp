using System;
using SsisUp.Configurations;
using SsisUp.Helpers;

namespace SsisUp.Services
{
    public interface IFileService
    {
        int Deploy(JobConfiguration jobConfiguration);
    }

    public class IntegrationServicesFileService : IFileService
    {
        private readonly IIoWrapper ioWrapper;

        public IntegrationServicesFileService(IIoWrapper ioWrapper)
        {
            if (ioWrapper == null) throw new ArgumentNullException("ioWrapper");
            this.ioWrapper = ioWrapper;
        }

        public int Deploy(JobConfiguration jobConfiguration)
        {
            foreach (var stepConfiguration in jobConfiguration.Steps)
            {
                if (stepConfiguration.SubSystem != SsisSubSystem.IntegrationServices) 
                    continue;

                try
                {
                    ioWrapper.CreateDirectoryIfNotExists(stepConfiguration.DtsxFileDestination);
                    ioWrapper.CopyFile(stepConfiguration.DtsxFile, stepConfiguration.DtsxFileDestination);
                    ioWrapper.CopyFile(stepConfiguration.DtsxConfigurationFile, stepConfiguration.DtsxFileDestination);
                }
                catch(Exception ex)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}

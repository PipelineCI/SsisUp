using System;
using SsisUp.Builders;
using SsisUp.Builders.References;
using SsisUp.Helpers;

namespace SsisUp.Services
{
    public interface IFileService
    {
        int Execute(JobConfiguration jobConfiguration);
    }

    public class IntegrationServicesFileService : IFileService
    {
        private readonly IIoWrapper _ioWrapper;

        public IntegrationServicesFileService(IIoWrapper ioWrapper)
        {
            if (ioWrapper == null) throw new ArgumentNullException("ioWrapper");
            this._ioWrapper = ioWrapper;
        }

        public int Execute(JobConfiguration jobConfiguration)
        {
            foreach (var stepConfiguration in jobConfiguration.Steps)
            {
                if (stepConfiguration.SubSystem != SsisSubSystem.IntegrationServices) 
                    continue;

                try
                {
                    _ioWrapper.CreateDirectoryIfNotExists(stepConfiguration.DtsxFileDestination);
                    _ioWrapper.CopyFile(stepConfiguration.DtsxFile, stepConfiguration.DtsxFileDestination);
                    _ioWrapper.CopyFile(stepConfiguration.DtsxConfigurationFile, stepConfiguration.DtsxFileDestination);
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

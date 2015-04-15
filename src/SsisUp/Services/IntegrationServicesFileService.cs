using System;
using System.Linq;
using System.Runtime.InteropServices;
using SsisUp.Builders;
using SsisUp.Builders.References;
using SsisUp.Helpers;

namespace SsisUp.Services
{
    public interface IFileService
    {
        DeploymentResult Execute(JobConfiguration jobConfiguration);
    }

    public class IntegrationServicesFileService : IFileService
    {
        private readonly IIoWrapper _ioWrapper;

        public IntegrationServicesFileService(IIoWrapper ioWrapper)
        {
            if (ioWrapper == null) throw new ArgumentNullException("ioWrapper");
            this._ioWrapper = ioWrapper;
        }

        public DeploymentResult Execute(JobConfiguration jobConfiguration)
        {
            DeploymentResult result;

            foreach (var stepConfiguration in jobConfiguration.Steps.Where(stepConfiguration => stepConfiguration.SubSystem == SsisSubSystem.IntegrationServices))
            {
                try
                {
                    _ioWrapper.CreateDirectoryIfNotExists(stepConfiguration.DtsxFileDestination);
                    _ioWrapper.CopyFile(stepConfiguration.DtsxFile, stepConfiguration.DtsxFileDestination);
                    _ioWrapper.CopyFile(stepConfiguration.DtsxConfigurationFile, stepConfiguration.DtsxFileDestination);
                }
                catch(Exception ex)
                {
                    result = new DeploymentResult(ex, false, GetType());
                    return result;
                }
            }
            
            result = new DeploymentResult(null, true, GetType());
            return result;
        }
    }
}

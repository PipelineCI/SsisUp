using System.IO;

namespace SsisUp.Helpers
{
    public interface IIoWrapper
    {
        void CopyFile(string sourceFile, string destionation);
        void CreateDirectoryIfNotExists(string directory);
    }

    public class IoWrapper : IIoWrapper
    {
        public void CreateDirectoryIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public void CopyFile(string sourceFile, string destionation)
        {
            var fileinfo = new FileInfo(sourceFile);
            File.Copy(sourceFile, string.Format(@"{0}\{1}", destionation, fileinfo.Name), true);
        }
    }
}
using RaceBoard.Common.Extensions;
using RaceBoard.Common.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;
using RaceBoard.FileStorage.Interfaces;

namespace RaceBoard.Common.Helpers
{
    public class FileHelper : IFileHelper
    {
        #region Private Members

        private readonly IFileStorageProvider _fileStorageProvider;

        #endregion

        public FileHelper(IConfiguration configuration, IFileStorageProvider fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;

            string currentWorkingPath = AppDomain.CurrentDomain.BaseDirectory; // Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _fileStorageProvider.SetCurrentDirectory(currentWorkingPath);
        }

        #region IFileHelper implementation

        public string SaveFile(string path, string directoryName, string fileName, byte[] fileContent)
        {
            string workDirectory = Path.Combine(path, directoryName);

            _fileStorageProvider.CreateDirectory(workDirectory);
            _fileStorageProvider.SetCurrentDirectory(workDirectory);

            string fileFullPath = _fileStorageProvider.SaveFile(fileContent, fileName);

            fileFullPath = fileFullPath.Replace(fileName, string.Empty);
            fileFullPath = fileFullPath.RemoveLastInstanceOfString("\\");

            return fileFullPath;
        }

        public bool DeleteFile(string path, string directoryName, string fileName)
        {
            string workDirectory = Path.Combine(path, directoryName);

            _fileStorageProvider.SetCurrentDirectory(workDirectory);
            
            return _fileStorageProvider.DeleteFile(fileName);
        }

        public bool DeleteFile(string fullFilePath)
        {
            return _fileStorageProvider.DeleteFile(fullFilePath);
        }

        #endregion
    }
}

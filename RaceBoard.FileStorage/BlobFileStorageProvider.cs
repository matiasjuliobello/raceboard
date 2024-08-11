using RaceBoard.FileStorage.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RaceBoard.FileStorage
{
    public class BlobFileStorageProvider : IFileStorageProvider
    {
        private readonly string _url = "";
        private readonly string _username = "";
        private readonly string _password = "";

        public BlobFileStorageProvider(IConfiguration configuration)
        {
            _url = configuration["BlobFileStorageUrl"];
            _username = configuration["BlobFileStorageUsername"];
            _password = configuration["BlobFileStoragePassword"];
        }

        #region IFileStorageProvider implementation

        public string GetRoot()
        {
            throw new NotImplementedException();
        }

        public void SetCurrentDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void CopyFiles(string[] sourceFiles)
        {
            throw new NotImplementedException();
        }

        public string CreateDirectory(string name)
        {
            throw new NotImplementedException();
        }


        public bool DeleteCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public bool DeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public string SaveFile(byte[] data, string fileName)
        {
            throw new NotImplementedException();
        }
        public FileStream CreateFileStream(string fileName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}

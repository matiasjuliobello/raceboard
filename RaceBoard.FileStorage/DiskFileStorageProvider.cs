using RaceBoard.FileStorage.Interfaces;

namespace RaceBoard.FileStorage
{
    public class DiskFileStorageProvider : IFileStorageProvider
    {
        private string _directory = "";

        public DiskFileStorageProvider()
        {
        }

        #region IFileStorageProvider implementation

        public string GetRoot()
        {
            return _directory; 
        }

        public void SetCurrentDirectory(string path)
        {
            _directory = Path.Combine(_directory, path);
        }

        public void CopyFiles(string[] sourceFiles)
        {
            throw new NotImplementedException();
        }

        public string CreateDirectory(string name)
        {
            string fullPath = GetFullPath(name);

            DirectoryInfo directoryInfo = Directory.CreateDirectory(fullPath);

            return directoryInfo.FullName;
        }

        public bool DeleteCurrentDirectory()
        {
            bool success = true;

            try
            {
                Directory.Delete(_directory, true);
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public bool DeleteDirectory(string path)
        {
            bool success = true;

            bool isFullFilePath = path.StartsWith(_directory);

            string fullPath = isFullFilePath ? path : GetFullPath(path);

            try
            {
                Directory.Delete(fullPath, true);
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public bool DeleteFile(string filePath)
        {
            bool success = true;

            bool isFullFilePath = filePath.StartsWith(_directory);

            string fullPath = isFullFilePath ? filePath : GetFullPath(filePath);

            try
            {
                File.Delete(fullPath);
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public string SaveFile(byte[] data, string fileName)
        {
            string fullPath = GetFullPath(fileName);

            File.WriteAllBytes(fullPath, data);

            return fullPath;
        }

        public FileStream CreateFileStream(string fileName)
        {
            string fullPath = GetFullPath(fileName);

            return new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        }

        #endregion

        #region Private Methods

        private string GetFullPath(string path)
        {
            return Path.Combine(_directory, path);
        }

        #endregion
    }
}

namespace RaceBoard.FileStorage.Interfaces
{
    public interface IFileStorageProvider
    {
        string GetRoot();
        void SetCurrentDirectory(string path);
        string CreateDirectory(string path);
        void CopyFiles(string[] sourceFiles);
        bool DeleteCurrentDirectory();
        bool DeleteDirectory(string path);
        bool DeleteFile(string filePath);
        string SaveFile(byte[] data, string fileName);
        FileStream CreateFileStream(string fileName);
    }
}

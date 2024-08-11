namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IFileHelper
    {
        string GetFileExtension(string filename);
        void RunExecutable(string fileName, string arguments = null, Action<string> onExecutedCallback = null);

        string GetSafeFilename(string filePath, string filename);

        string RemoveIllegalCharsFromFilename(string fileName);
        string SaveFile(Stream stream, string path, string filename);

        string SaveFile(byte[] content, string path, string filename);

        DirectoryInfo CreateDirectory(string path);

        void DeleteDirectory(string path);

        Stream ReadFileInfoStream(string filePath);

        byte[] ReadBytesFromStream(Stream stream);
        void CopyFiles(string sourcePath, string targetPath, string[] files = null);

    }
}

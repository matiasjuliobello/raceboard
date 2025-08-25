namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IFileHelper
    {
        string GetBasePath();
        string RemoveBasePath(string fullFilePath);
        string SaveFile(string path, string directoryName, string fileName, byte[] fileContent);
        bool DeleteFile(string path, string directoryName, string fileName);
        bool DeleteFile(string fullFilePath);
    }
}

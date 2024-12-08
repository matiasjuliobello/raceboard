namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IFileHelper
    {
        string SaveFile(string path, string directoryName, string fileName, byte[] fileContent);
        bool DeleteFile(string path, string directoryName, string fileName);
        bool DeleteFile(string fullFilePath);
    }
}

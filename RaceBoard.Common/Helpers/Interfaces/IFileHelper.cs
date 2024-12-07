namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IFileHelper
    {
        string SaveFile(string path, string directoryName, string fileName, byte[] fileContent);
    }
}

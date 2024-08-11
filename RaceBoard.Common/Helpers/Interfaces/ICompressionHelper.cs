namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface ICompressionHelper
    {
        MemoryStream CreateZipFile(string[] fileNames, string directory = null);
    }
}

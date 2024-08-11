namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface ICryptographyHelper
    {
        string ComputeHash(string text);

        bool VerifyHash(string hash, string text);
    }
}

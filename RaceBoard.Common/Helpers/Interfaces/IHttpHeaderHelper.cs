namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IHttpHeaderHelper
    {
        RequestContext GetContext();
        void SetContext();
    }
}
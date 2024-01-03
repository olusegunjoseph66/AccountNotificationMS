namespace Shared.ExternalServices.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendMessage(string message, string[] recipients);
    }
}
namespace Shared.ExternalServices.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendMessage(string emailTemplateId, string[] recipients, List<Dictionary<string, string>> emailValues, CancellationToken cancellationToken = default);
    }
}
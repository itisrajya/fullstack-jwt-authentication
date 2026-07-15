namespace server.authentication.application.IService;
public interface ICheckDnsService
{
    Task<IPAddress[]> GetHostEntryAsync(string domain);
}

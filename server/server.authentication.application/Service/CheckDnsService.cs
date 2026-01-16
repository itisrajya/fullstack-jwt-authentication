using server.authentication.application.IService;
using System.Net;

namespace server.authentication.application.Service
{
    public class CheckDnsService : ICheckDnsService
    {
        public async Task<IPAddress[]> GetHostEntryAsync(string domain)
        {
            var hostEntry = await Dns.GetHostEntryAsync(domain);
            return hostEntry.AddressList;
        }
    }
}

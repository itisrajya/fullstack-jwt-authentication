using server.authentication.application.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.Service
{
    public class DnsService : IDnsService
    {
        public async Task<IPAddress[]> GetHostEntryAsync(string domain)
        {
            var hostEntry = await Dns.GetHostEntryAsync(domain);
            return hostEntry.AddressList;
        }
    }
}

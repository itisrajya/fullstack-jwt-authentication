using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.IService
{
    public interface IDnsService
    {
        Task<IPAddress[]> GetHostEntryAsync(string domain);
    }
}

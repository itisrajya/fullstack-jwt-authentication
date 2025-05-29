using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.IService
{
    public interface IValidEmailService
    {
        bool IsValidEmailFormat(string email);
        Task<bool> HasValidMxRecords(string email);
    }
}
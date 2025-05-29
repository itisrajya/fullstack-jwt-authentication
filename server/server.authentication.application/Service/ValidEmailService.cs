using server.authentication.application.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace server.authentication.application.Service
{
    public class ValidEmailService : IValidEmailService
    {
        private readonly IDnsService _dnsService;
        public ValidEmailService(IDnsService dnsService)
        {
            _dnsService = dnsService;
        }

        public bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var domainName = match.Groups[2].Value;
                    return match.Groups[1].Value + domainName;
                }

                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public async Task<bool> HasValidMxRecords(string email)
        {
            try
            {
                string domain = email.Split('@')[1];
                var mxRecords = await _dnsService.GetHostEntryAsync(domain);
                return mxRecords.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}

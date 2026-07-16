namespace server.authentication.application.Service;

public class ValidEmailService : IValidEmailService
{
    private readonly ICheckDnsService _dnsService;
    public ValidEmailService(ICheckDnsService dnsService)
    {
        _dnsService = dnsService;
    }

    public bool IsValidEmailFormat(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        else
            return true;
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

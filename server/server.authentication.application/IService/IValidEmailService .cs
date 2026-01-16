namespace server.authentication.application.IService
{
    public interface IValidEmailService
    {
        bool IsValidEmailFormat(string email);
        Task<bool> HasValidMxRecords(string email);
    }
}
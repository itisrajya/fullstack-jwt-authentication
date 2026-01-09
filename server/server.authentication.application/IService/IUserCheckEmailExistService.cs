namespace server.authentication.application.IService
{
    public interface IUserCheckEmailExistService
    {
        Task<bool> IsEmailTaken(string email);
    }
}

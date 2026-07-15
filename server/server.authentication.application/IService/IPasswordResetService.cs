using System.Threading.Tasks;

namespace server.authentication.application.IService
{
    public interface IPasswordResetService
    {
        /// <summary>
        /// Initiates a password reset for the given email, if the user exists.
        /// Always completes successfully regardless of whether the email is registered,
        /// to avoid leaking account existence.
        /// </summary>
        Task RequestPasswordReset(string email);

        /// <summary>
        /// Validates the supplied token and, if valid, updates the user's password.
        /// Throws <see cref="System.ArgumentException"/> when the token/email/password combination is invalid.
        /// </summary>
        Task ResetPassword(string email, string token, string newPassword);
    }
}

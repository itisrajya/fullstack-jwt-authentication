using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using server.authentication.application.IService;
using server.authentication.data.DatabaseConnection;
using server.authentication.data.Entities;

namespace server.authentication.application.Service
{
    public class PasswordResetService : IPasswordResetService
    {
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);

        private readonly UserDataContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public PasswordResetService(UserDataContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        private static bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                   password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^&*()".Contains(ch));
        }

        private static string GenerateRawToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Base64UrlEncode(bytes);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string HashToken(string rawToken)
        {
            var bytes = Encoding.UTF8.GetBytes(rawToken);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task RequestPasswordReset(string email)
        {
            // TODO: Add rate limiting / abuse protection (e.g. IP or email based throttling) before issuing new tokens.
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            // Do not reveal whether the email exists - always behave as if the request succeeded.
            if (user == null)
                return;

            var rawToken = GenerateRawToken();
            var tokenHash = HashToken(rawToken);
            var now = DateTime.UtcNow;

            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                TokenHash = tokenHash,
                CreatedAtUtc = now,
                ExpiresAtUtc = now.Add(TokenLifetime),
                UsedAtUtc = null
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // NOTE: Update "PasswordReset:ClientResetUrl" in appsettings.json to point at the actual frontend reset page.
            var clientResetUrl = _configuration["PasswordReset:ClientResetUrl"] ?? "https://<client-app>/reset-password";
            var encodedEmail = Uri.EscapeDataString(user.Email);
            var encodedToken = Uri.EscapeDataString(rawToken);
            var resetLink = $"{clientResetUrl}?email={encodedEmail}&token={encodedToken}";

            var subject = "Reset your password";
            var body = $"""
                <p>Hello {WebUtility.HtmlEncode(user.Username)},</p>
                <p>We received a request to reset your password. Click the link below to choose a new password. This link expires in {TokenLifetime.TotalMinutes} minutes.</p>
                <p><a href="{resetLink}">Reset your password</a></p>
                <p>If you did not request a password reset, you can safely ignore this email.</p>
                """;

            await _emailSender.SendEmailAsync(user.Email, subject, body);
        }

        public async Task ResetPassword(string email, string token, string newPassword)
        {
            if (!IsPasswordValid(newPassword))
                throw new ArgumentException("Password must be at least 8 characters long, contain an uppercase letter, lowercase letter, number, and special character.");

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new ArgumentException("Invalid or expired reset token.");

            var tokenHash = HashToken(token);
            var now = DateTime.UtcNow;

            var resetToken = await _context.PasswordResetTokens
                .Where(t => t.UserId == user.UserId && t.TokenHash == tokenHash)
                .OrderByDescending(t => t.CreatedAtUtc)
                .FirstOrDefaultAsync();

            if (resetToken == null || resetToken.UsedAtUtc != null || resetToken.ExpiresAtUtc < now)
                throw new ArgumentException("Invalid or expired reset token.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            resetToken.UsedAtUtc = now;

            await _context.SaveChangesAsync();
        }
    }
}



using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string fullName, string email, string password);
        Task<AuthResponseDto?> LoginAsync(string email, string password);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}

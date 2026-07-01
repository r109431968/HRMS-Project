using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {

        private readonly HRMSDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(HRMSDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(string fullName, string email, string password)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
                return "User already exists with this email.";

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "Employee",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<AuthResponseDto?> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
                return null;

            // Access Token generate karo
            var accessToken = _jwtService.GenerateToken(user);

            // Refresh Token generate karo aur save karo
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            // Database mein Refresh Token dhundo
            var tokenEntity = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            // Validate karo
            if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiryDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            // Purana token revoke karo (Token Rotation)
            tokenEntity.IsRevoked = true;

            // Naya Refresh Token banao
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = tokenEntity.UserId,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            // Naya Access Token generate karo
            var newAccessToken = _jwtService.GenerateToken(tokenEntity.User!);

            return (newAccessToken, newRefreshToken);
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var trimmedToken = refreshToken.Trim();

            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == trimmedToken);

            if (tokenEntity == null || tokenEntity.IsRevoked)
                return false;

            tokenEntity.IsRevoked = true;
            await _context.SaveChangesAsync();

            return true;
        }

        // Random secure string generate karta hai Refresh Token ke liye
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}

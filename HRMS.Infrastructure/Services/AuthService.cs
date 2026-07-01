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

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
                return null;

            return _jwtService.GenerateToken(user);
        }

        //private readonly HRMSDbContext _context;
        //private readonly IConfiguration _configuration;

        //public AuthService(HRMSDbContext context, IConfiguration configuration)
        //{
        //    _context = context;
        //    _configuration = configuration;
        //}

        //public async Task<string> RegisterAsync(string fullName, string email, string password)
        //{
        //    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //    if (existingUser != null)
        //        return "User already exists with this email.";

        //    var user = new User
        //    {
        //        FullName = fullName,
        //        Email = email,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        //        Role = "Employee",
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return "User registered successfully.";
        //}

        //public async Task<string?> LoginAsync(string email, string password)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //    if (user == null)
        //        return null;

        //    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        //    if (!isPasswordValid)
        //        return null;

        //    return GenerateJwtToken(user);
        //}

        //private string GenerateJwtToken(User user)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.Name, user.FullName),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
        //        signingCredentials: credentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Configuration;
using Services.User.Application.DTOs;
using Services.User.Domain.Entities;
using Services.User.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserEntity = Services.User.Domain.Entities.User;

namespace Services.User.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        // Kullanıcıyı email ile bul
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Geçersiz email veya şifre");

        // Şifre kontrolü (gerçek uygulamada hash karşılaştırması yapılmalı)
        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Geçersiz email veya şifre");

        // Kullanıcı aktif mi?
        if (!user.IsActive)
            throw new UnauthorizedAccessException("Hesabınız devre dışı bırakılmış");

        // Token'ları oluştur
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        // Refresh token'ı veritabanına kaydet
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);
        await _userRepository.UpdateAsync(user);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes),
            User = MapToUserDto(user)
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // Email kontrolü
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Bu email adresi zaten kullanımda");

        // Yeni kullanıcı oluştur
        var user = new UserEntity
        {
            Email = registerDto.Email,
            PasswordHash = HashPassword(registerDto.Password), //BCrypt kullan
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber,
            DateOfBirth = registerDto.DateOfBirth ?? default(DateTime),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        // Login işlemi yap
        return await LoginAsync(new LoginDto 
        { 
            Email = registerDto.Email, 
            Password = registerDto.Password 
        });
    }

    public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshTokenDto.RefreshToken);
        
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Geçersiz refresh token");

        // Yeni token'lar oluştur
        var accessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken();

        // Refresh token'ı güncelle
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);
        await _userRepository.UpdateAsync(user);

        return new RefreshTokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes)
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
        if (user == null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userRepository.UpdateAsync(user);

        return true;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateAccessToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new("userId", user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string HashPassword(string password)
    {
        // Bu sadece demo amaçlı basit hash
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt"));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    private UserDto MapToUserDto(UserEntity user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}

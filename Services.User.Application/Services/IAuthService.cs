using Services.User.Application.DTOs;

namespace Services.User.Application.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<LoginResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task<bool> LogoutAsync(string refreshToken);
    Task<bool> ValidateTokenAsync(string token);
}

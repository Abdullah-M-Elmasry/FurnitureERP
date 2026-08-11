using FurnitureERP.Application.Security.DTOs;
using FurnitureERP.Domain.Entities.Security;

namespace FurnitureERP.Application.Security.Interfaces;

public interface IAuthService
{
    Task<UserDto> Register(RegisterRequestdDTO request);
    Task<LoginResponseDto> Login(LoginRequestDto request);
}

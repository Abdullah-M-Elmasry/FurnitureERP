using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Security.DTOs;
using FurnitureERP.Application.Security.Interfaces;
using FurnitureERP.Application.Users.Interfaces;
using FurnitureERP.Domain.Entities.Security;

namespace FurnitureERP.Application.Security.Services;

public class AuthService : IAuthService
{
    private IUserRepository _repo;
    private IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(
     IUserRepository repo,
     IUnitOfWork unitOfWork,
     IPasswordService passwordService,
     IJwtService jwtService)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<UserDto> Register(RegisterRequestdDTO request)
    {
        if (request == null)
            throw new ValidationExceptionApp("Request is required");

        if (await _repo.UsernameExists(request.Username))
            throw new ConflictExceptionApp("Username already exists");

        var passwordHash = _passwordService.HashPassword(request.Password);

        var user = new User(
        request.Username,
        passwordHash,
        request.FullName);

        await _repo.Add(user);

        await _unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName
        };

    }


    public async Task<LoginResponseDto> Login(LoginRequestDto request)
    {
        if (request == null)
            throw new ValidationExceptionApp("Request is required");

        var user = await _repo.GetByUsername(request.UserName);

        if (user == null)
            throw new UnauthorizedExceptionApp("Invalid username or password");

        var isValid = _passwordService.VerifyPassword(
        user.PasswordHash,
        request.Password);

        if (!isValid)
            throw new UnauthorizedExceptionApp("Invalid username or password");

        user.LastLoginAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();


        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Token = token

        };

    }


}
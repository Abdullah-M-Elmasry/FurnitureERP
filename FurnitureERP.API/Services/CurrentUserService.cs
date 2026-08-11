using FurnitureERP.Application.Common.Interfaces;
using System.Security.Claims;

namespace FurnitureERP.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var userIdValue =
                _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(
                    ClaimTypes.NameIdentifier);

            return int.TryParse(
                userIdValue,
                out var userId)
                ? userId
                : null;
        }
    }

    public string? UserName
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(
                    ClaimTypes.Name);
        }
    }

    public IReadOnlyList<string> Permissions
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                .User
                .FindAll("Permission")
                .Select(x => x.Value)
                .ToList()
                ?? [];
        }
    }
}
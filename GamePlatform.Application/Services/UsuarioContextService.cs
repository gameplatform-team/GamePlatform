using GamePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GamePlatform.Application.Services;

public class UsuarioContextService : IUsuarioContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUsuarioId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userId, out var id) ? id : throw new UnauthorizedAccessException("ID do usuário inválido.");
    }

    public string GetRole()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }

    public bool UsuarioEhAdmin()
    {
        return GetRole().Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }
}
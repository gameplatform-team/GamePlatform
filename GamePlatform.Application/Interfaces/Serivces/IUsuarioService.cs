using GamePlatform.Application.DTOs;

namespace GamePlatform.Application.Interfaces.Serivces;

public interface IUsuarioService
{
    Task<(bool sucesso, string mensagem)> RegistrarAsync(RegistrarUsuarioDto dto);

    Task<(bool sucesso, string? token, string mensagem)> LoginAsync(LoginDto dto);
}

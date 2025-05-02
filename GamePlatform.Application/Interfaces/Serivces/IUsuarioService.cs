using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Usuario;

namespace GamePlatform.Application.Interfaces.Serivces;

public interface IUsuarioService
{
    Task<UsuarioDto?> ObterPorIdAsync(Guid id);
    Task<bool> AtualizarAsync(Guid id, AtualizarUsuarioDto dto);
    Task<bool> ExcluirAsync(Guid id);
    Task<IEnumerable<UsuarioDto>> ListarTodosAsync();
    Task<bool> PromoverParaAdminAsync(Guid id);
    Task<(bool sucesso, string mensagem)> RegistrarAsync(RegistrarUsuarioDto dto);
    Task<(bool sucesso, string? token, string mensagem)> LoginAsync(LoginDto dto);
}

using GamePlatform.Domain.Entities;

namespace GamePlatform.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    void Remover(Usuario usuario);
    Task<IEnumerable<Usuario>> ListarTodosAsync();
    Task<bool> ExisteEmailAsync(string email);
    Task AdicionarAsync(Usuario usuario);
    Task SalvarAsync();
    Task<bool> EmailJaExisteAsync(string email, Guid? id = null);
}

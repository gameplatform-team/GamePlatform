using System.Linq.Expressions;
using GamePlatform.Domain.Entities;

namespace GamePlatform.Domain.Interfaces;

public interface IJogoRepository
{
    public Task<bool> ExisteTituloAsync(string titulo);
    public Task AdicionarAsync(Jogo jogo);
    public Task<Jogo?> ObterPorIdAsync(Guid id);
    public Task<IEnumerable<Jogo>> ObterTodosAsync(Expression<Func<Jogo, bool>>? filtro = null);
    public Task AtualizarAsync(Jogo jogoExistente);
}
using GamePlatform.Domain.Entities;

namespace GamePlatform.Domain.Interfaces;

public interface IJogoRepository
{
    public Task<bool> ExisteTituloAsync(string titulo);
    public Task AdicionarAsync(Jogo jogo);
}
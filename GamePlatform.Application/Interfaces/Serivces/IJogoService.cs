using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Jogo;
using GamePlatform.Domain.Entities;

namespace GamePlatform.Application.Interfaces.Serivces;

public interface IJogoService
{
    public Task<BaseResponseDto> CadastrarAsync(CadastrarJogoDto jogoDto);
    public Task<JogoResponseDto> ObterPorIdAsync(Guid id);
    public Task<IEnumerable<Jogo>> ObterTodosAsync(string? titulo = null, decimal? precoMinimo = null, decimal? precoMaximo = null);
}
using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Jogo;
using GamePlatform.Domain.Entities;

namespace GamePlatform.Application.Interfaces.Serivces;

public interface IJogoService
{
    public Task<BaseResponseDto> CadastrarAsync(CadastrarJogoDto jogoDto);
    public Task<BaseResponseDto> ObterPorIdAsync(Guid id);
    public Task<ResultadoPaginadoDto<JogoDto>> ObterTodosAsync(
        string? titulo = null,
        decimal? precoMinimo = null,
        decimal? precoMaximo = null,
        int numeroPagina = 1,
        int tamanhoPagina = 10);
    public Task<BaseResponseDto> AtualizarAsync(AtualizarJogoDto jogoDto);
    public Task<BaseResponseDto> RemoverAsync(Guid id);
}
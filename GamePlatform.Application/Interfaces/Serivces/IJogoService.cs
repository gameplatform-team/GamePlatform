using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Jogo;

namespace GamePlatform.Application.Interfaces.Serivces;

public interface IJogoService
{
    public Task<BaseResponseDto> CadastrarAsync(CadastrarJogoDto jogoDto);
}
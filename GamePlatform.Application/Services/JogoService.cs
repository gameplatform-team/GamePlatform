using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Jogo;
using GamePlatform.Application.Interfaces.Serivces;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;

namespace GamePlatform.Application.Services;

public class JogoService : IJogoService
{
    private readonly IJogoRepository _jogoRepository;

    public JogoService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task<BaseResponseDto> CadastrarAsync(CadastrarJogoDto jogoDto)
    {
        if (await _jogoRepository.ExisteTituloAsync(jogoDto.Titulo))
            return new BaseResponseDto(false, "Jogo já cadastrado");

        var jogo = new Jogo(jogoDto.Titulo, jogoDto.Preco);
        await _jogoRepository.AdicionarAsync(jogo);
        
        return new BaseResponseDto(true, "Jogo cadastrado com sucesso");
    }
}
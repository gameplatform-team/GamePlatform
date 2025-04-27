using System.Linq.Expressions;
using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Jogo;
using GamePlatform.Application.Interfaces.Serivces;
using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<BaseResponseDto> ObterPorIdAsync(Guid id)
    {
        var jogo = await _jogoRepository.ObterPorIdAsync(id);
        
        if (jogo == null)
            return new BaseResponseDto(false, "Jogo não encontrado");

        var jogoDto = new JogoDto
        {
            Id = jogo.Id,
            Titulo = jogo.Titulo,
            Preco = jogo.Preco
        };
        
        return new DataResponseDto<JogoDto>(true, string.Empty, jogoDto);
    }

    public async Task<ResultadoPaginadoDto<JogoDto>> ObterTodosAsync(
        string? titulo = null,
        decimal? precoMinimo = null,
        decimal? precoMaximo = null,
        int numeroPagina = 1,
        int tamanhoPagina = 10)
    {
        Expression<Func<Jogo, bool>>? filtro = null;

        if (!string.IsNullOrWhiteSpace(titulo) || precoMinimo.HasValue || precoMaximo.HasValue)
        {
            filtro = jogo =>
                (string.IsNullOrWhiteSpace(titulo) || EF.Functions.Like(jogo.Titulo.ToLower(), $"%{titulo.ToLower()}%")) &&
                (!precoMinimo.HasValue || jogo.Preco >= precoMinimo.Value) &&
                (!precoMaximo.HasValue || jogo.Preco <= precoMaximo.Value);
        }
        
        var (jogos, totalDeItens) = await _jogoRepository.ObterTodosPaginadoAsync(filtro, numeroPagina, tamanhoPagina);
        
        var result = new ResultadoPaginadoDto<JogoDto>()
        {
            Itens = jogos.Select(jogo => new JogoDto
            {
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Preco = jogo.Preco
            }),
            NumeroPagina = numeroPagina,
            TamanhoPagina = tamanhoPagina,
            TotalDeItens = totalDeItens
        };
        
        return result;
    }

    public async Task<BaseResponseDto> AtualizarAsync(AtualizarJogoDto jogoDto)
    {
        var jogoExistente = await _jogoRepository.ObterPorIdAsync(jogoDto.Id);
    
        if (jogoExistente == null)
            return new BaseResponseDto(false, "Jogo não encontrado");
    
        var jogosComMesmoTitulo = await _jogoRepository.ObterTodosAsync(
            j => j.Titulo.ToLower() == jogoDto.Titulo.ToLower() && j.Id != jogoDto.Id);
    
        if (jogosComMesmoTitulo.Any())
            return new BaseResponseDto(false, "Já existe outro jogo com este título");

        jogoExistente.Atualizar(jogoDto.Titulo, jogoDto.Preco);
    
        await _jogoRepository.AtualizarAsync(jogoExistente);
    
        return new BaseResponseDto(true, "Jogo atualizado com sucesso");
    }

    public async Task<BaseResponseDto> RemoverAsync(Guid id)
    {
        var jogoExistente = await _jogoRepository.ObterPorIdAsync(id);
    
        if (jogoExistente == null)
            return new BaseResponseDto(false, "Jogo não encontrado");
        
        await _jogoRepository.RemoverAsync(jogoExistente);
        
        return new BaseResponseDto(true, "Jogo removido com sucesso");
    }
}
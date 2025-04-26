using GamePlatform.Application.DTOs;
using GamePlatform.Application.DTOs.Jogo;
using GamePlatform.Application.Interfaces.Serivces;
using GamePlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers;

[ApiController]
[Route("api/jogos")]
[ProducesResponseType(500)]
public class JogoController : ControllerBase
{
    private readonly IJogoService _jogoService;

    public JogoController(IJogoService jogoService)
    {
        _jogoService = jogoService;
    }

    /// <summary>
    /// Cadastra um novo jogo na plataforma
    /// </summary>
    /// <param name="jogo"></param>
    /// <response code="201">Jogo cadastrado com sucesso</response>
    /// <response code="400">Ocorreu um erro ao cadastrar o jogo</response>
    [ProducesResponseType(typeof(BaseResponseDto), 201)]
    [ProducesResponseType(typeof(BaseResponseDto), 400)]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PostAsync(CadastrarJogoDto jogo)
    {
        var resultado = await _jogoService.CadastrarAsync(jogo);
        
        return !resultado.Sucesso ? BadRequest(resultado) : StatusCode(201, resultado);
    }

    /// <summary>
    /// Obtém lista de jogos cadastrados na plataforma
    /// </summary>
    /// <param name="titulo">Filtrar jogos que o título contenha o texto informado</param>
    /// <param name="precoMinimo">Filtrar jogos por valor mínimo</param>
    /// <param name="precoMaximo">Filtrar jogos por valor máximo</param>
    /// <response code="200">Lista de jogos cadastrados</response>
    /// <response code="204">Nenhum jogo encontrado</response>
    [ProducesResponseType(typeof(IEnumerable<Jogo>), 200)]
    [ProducesResponseType(typeof(BaseResponseDto), 404)]
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] string? titulo = null,
        [FromQuery] decimal? precoMinimo = null,
        [FromQuery] decimal? precoMaximo = null)
    {
        var resultado = await _jogoService.ObterTodosAsync(titulo, precoMinimo, precoMaximo);
        
        return !resultado.Any() ? NoContent() : Ok(resultado);
    }

    /// <summary>
    /// Obtém um jogo pelo ID
    /// </summary>
    /// <param name="id">ID do jogo</param>
    /// <response code="200">Jogo encontrado com sucesso</response>
    /// <response code="404">Jogo não encontrado</response>
    [ProducesResponseType(typeof(JogoResponseDto), 200)]
    [ProducesResponseType(typeof(BaseResponseDto), 404)]
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var resultado = await _jogoService.ObterPorIdAsync(id);
        
        return !resultado.Sucesso ? NotFound(resultado) : Ok(resultado);
    }
    
    /// <summary>
    /// Atualiza um jogo
    /// </summary>
    /// <param name="jogoDto">Dados do jogo</param>
    /// <response code="200">Jogo atualizado com sucesso</response>
    /// <response code="400">Ocorreu um erro ao atualizar o jogo</response>
    [ProducesResponseType(typeof(BaseResponseDto), 200)]
    [ProducesResponseType(typeof(BaseResponseDto), 400)]
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutAsync(AtualizarJogoDto jogoDto)
    {
        var resultado = await _jogoService.AtualizarAsync(jogoDto);
        
        return !resultado.Sucesso ? BadRequest(resultado) : Ok(resultado);
    }
}
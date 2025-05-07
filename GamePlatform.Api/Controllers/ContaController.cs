using GamePlatform.Application.DTOs;
using GamePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers;

[ApiController]
[Route("api/conta")]
public class ContaController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public ContaController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    /// <param name="dto">Dados do usuário para registro</param>
    /// <response code="200">Usuário registrado com sucesso</response>
    /// <response code="400">Erro ao registrar o usuário (ex: e-mail já em uso ou dados inválidos)</response>
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto dto)
    {
        var resultado = await _usuarioService.RegistrarAsync(dto);

        if (!resultado.Sucesso)
            return BadRequest(new { sucesso = false, resultado.Mensagem });

        return Ok(new { sucesso = true, resultado.Mensagem });
    }

    /// <summary>
    /// Realiza o login de um usuário no sistema.
    /// </summary>
    /// <param name="dto">Credenciais do usuário (e-mail e senha)</param>
    /// <response code="200">Login realizado com sucesso. Token JWT retornado.</response>
    /// <response code="401">Credenciais inválidas</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (sucesso, token, mensagem) = await _usuarioService.LoginAsync(dto);

        if (!sucesso)
            return Unauthorized(new { sucesso = false, mensagem });

        return Ok(new { sucesso = true, token });
    }
}

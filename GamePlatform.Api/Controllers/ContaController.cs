using GamePlatform.Application.DTOs;
using GamePlatform.Application.Interfaces.Serivces;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public ContaController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioDto dto)
    {
        var resultado = await _usuarioService.RegistrarAsync(dto);

        if (!resultado.sucesso)
            return BadRequest(new { sucesso = false, resultado.mensagem });

        return Ok(new { sucesso = true, resultado.mensagem });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var resultado = await _usuarioService.LoginAsync(dto);

        if (!resultado.sucesso)
            return Unauthorized(new { sucesso = false, resultado.mensagem });

        return Ok(new { sucesso = true, resultado.token });
    }
}
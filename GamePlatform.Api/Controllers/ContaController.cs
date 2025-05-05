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

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    /// <param name="dto">Objeto contendo as informações do novo usuário.</param>
    /// <returns>Retorna OK com a mensagem de sucesso ou BadRequest caso haja algum erro no processo de registro.</returns>
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
    /// <param name="dto">Objeto contendo as credenciais de login (email e senha) do usuário.</param>
    /// <returns>Retorna OK com o token JWT gerado se o login for bem-sucedido, ou Unauthorized caso as credenciais estejam incorretas.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (sucesso, token, mensagem) = await _usuarioService.LoginAsync(dto);

        if (!sucesso)
            return Unauthorized(new { sucesso = false, mensagem });

        return Ok(new { sucesso = true, token });
    }
}

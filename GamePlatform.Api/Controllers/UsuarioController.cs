using GamePlatform.Api.Extensions;
using GamePlatform.Application.DTOs.Usuario;
using GamePlatform.Application.Interfaces.Serivces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    protected readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetMeuUsuario()
    {
        var id = User.GetUserId();
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPut()]
    [Authorize]
    public async Task<IActionResult> AtualizarMeuUsuario([FromBody] AtualizarUsuarioDto dto)
    {
        var id = User.GetUserId();
        var sucesso = await _usuarioService.AtualizarAsync(id, dto);
        return sucesso ? NoContent() : BadRequest("Erro ao atualizar usuário.");
    }

    [HttpDelete()]
    [Authorize]
    public async Task<IActionResult> ExcluirMinhaConta()
    {
        var id = User.GetUserId();
        var sucesso = await _usuarioService.ExcluirAsync(id);
        return sucesso ? NoContent() : BadRequest("Erro ao excluir conta.");
    }

    [HttpGet("listar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListarTodos()
    {
        var usuarios = await _usuarioService.ListarTodosAsync();
        return Ok(usuarios);
    }

    [HttpPut("{id}/promover")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoverParaAdmin(Guid id)
    {
        var sucesso = await _usuarioService.PromoverParaAdminAsync(id);
        return sucesso ? NoContent() : BadRequest("Erro ao promover usuário.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }
}

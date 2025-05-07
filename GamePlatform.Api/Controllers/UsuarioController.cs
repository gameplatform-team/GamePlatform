using GamePlatform.Application.DTOs.Usuario;
using GamePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IUsuarioContextService _usuarioContext;

    public UsuarioController(IUsuarioService usuarioService, IUsuarioContextService usuarioContext)
    {
        _usuarioService = usuarioService;
        _usuarioContext = usuarioContext;
    }

    /// <summary>
    /// Obtém os dados do usuário autenticado.
    /// </summary>
    /// <response code="200">Usuário encontrado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpGet("perfil")]
    [Authorize]
    public async Task<IActionResult> GetPerfil()
    {
        var id = _usuarioContext.GetUsuarioId();
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>
    /// Obtém os dados de um usuário específico pelo seu ID (somente administradores).
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <response code="200">Usuário encontrado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>
    /// Atualiza os dados do usuário autenticado.
    /// </summary>
    /// <param name="dto">Dados para atualização do usuário</param>
    /// <response code="204">Usuário atualizado com sucesso</response>
    /// <response code="400">Erro na atualização (ex: e-mail em uso ou dados inválidos)</response>
    [HttpPut("perfil")]
    [Authorize]
    public async Task<IActionResult> AtualizarPerfil([FromBody] AtualizarUsuarioDto dto)
    {
        var id = _usuarioContext.GetUsuarioId();
        var resultado = await _usuarioService.AtualizarAsync(id, dto);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }

    /// <summary>
    /// Atualiza os dados de um usuário específico pelo seu ID (somente administradores).
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="dto">Dados para atualização do usuário</param>
    /// <response code="204">Usuário atualizado com sucesso</response>
    /// <response code="400">Erro na atualização (ex: e-mail em uso ou usuário não encontrado)</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AtualizarById(Guid id, [FromBody] AtualizarUsuarioDto dto)
    {
        var resultado = await _usuarioService.AtualizarAsync(id, dto);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }

    /// <summary>
    /// Exclui a conta do usuário autenticado.
    /// </summary>
    /// <response code="204">Usuário excluído com sucesso</response>
    /// <response code="400">Erro ao excluir o usuário (ex: usuário não encontrado)</response>
    [HttpDelete("perfil")]
    [Authorize]
    public async Task<IActionResult> ExcluirPerfil()
    {
        var id = _usuarioContext.GetUsuarioId();
        var resultado = await _usuarioService.ExcluirAsync(id);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }

    /// <summary>
    /// Exclui um usuário específico pelo ID (somente administradores).
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <response code="204">Usuário excluído com sucesso</response>
    /// <response code="400">Erro ao excluir o usuário (ex: usuário não encontrado)</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExcluirById(Guid id)
    {
        var resultado = await _usuarioService.ExcluirAsync(id);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }

    /// <summary>
    /// Lista todos os usuários do sistema (somente administradores).
    /// </summary>
    /// <response code="200">Lista de usuários retornada com sucesso</response>
    [HttpGet("listar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListarTodos()
    {
        var usuarios = await _usuarioService.ListarTodosAsync();
        return Ok(usuarios);
    }
}

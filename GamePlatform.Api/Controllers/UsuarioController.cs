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

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtém os dados do usuário autenticado.
    /// </summary>
    /// <returns>Retorna os dados do usuário ou NotFound caso o usuário não exista.</returns>
    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetMeuUsuario()
    {
        var id = User.GetUserId();
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>
    /// Obtém os dados de um usuário específico pelo seu ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser obtido.</param>
    /// <returns>Retorna os dados do usuário ou NotFound caso o usuário não exista.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>
    /// Atualiza os dados do usuário autenticado.
    /// </summary>
    /// <param name="dto">Objeto contendo os novos dados do usuário.</param>
    /// <returns>Retorna NoContent se a atualização for bem-sucedida, ou BadRequest caso ocorra algum erro.</returns>
    [HttpPut()]
    [Authorize]
    public async Task<IActionResult> AtualizarMeuUsuario([FromBody] AtualizarUsuarioDto dto)
    {
        var id = User.GetUserId();
        var resultado = await _usuarioService.AtualizarAsync(id, dto);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }

    /// <summary>
    /// Exclui a conta do usuário autenticado.
    /// </summary>
    /// <returns>Retorna NoContent se a exclusão for bem-sucedida, ou BadRequest caso ocorra algum erro.</returns>
    [HttpDelete()]
    [Authorize]
    public async Task<IActionResult> ExcluirMinhaConta()
    {
        var id = User.GetUserId();
        var resultado = await _usuarioService.ExcluirAsync(id);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }

    /// <summary>
    /// Lista todos os usuários no sistema (somente para administradores).
    /// </summary>
    /// <returns>Retorna a lista de usuários.</returns>
    [HttpGet("listar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListarTodos()
    {
        var usuarios = await _usuarioService.ListarTodosAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Promove um usuário para o papel de administrador.
    /// </summary>
    /// <param name="id">ID do usuário a ser promovido.</param>
    /// <returns>Retorna NoContent se a promoção for bem-sucedida, ou BadRequest caso ocorra algum erro.</returns>
    [HttpPut("{id}/promover")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoverParaAdmin(Guid id)
    {
        var resultado = await _usuarioService.PromoverParaAdminAsync(id);
        return resultado.Sucesso ? NoContent() : BadRequest(resultado.Mensagem);
    }
}

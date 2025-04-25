using System.ComponentModel.DataAnnotations;

namespace GamePlatform.Application.DTOs.Jogo;

public class CadastrarJogoDto
{
    [Required]
    public string Titulo { get; init; }
    
    [Required]
    public decimal Preco { get; init; }
}
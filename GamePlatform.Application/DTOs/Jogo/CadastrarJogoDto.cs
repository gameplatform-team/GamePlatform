using System.ComponentModel.DataAnnotations;

namespace GamePlatform.Application.DTOs.Jogo;

public class CadastrarJogoDto
{
    [Required]
    [MinLength(1)]
    public string Titulo { get; init; }
    
    [Required]
    [Range(0.01, 100000.00)]
    public decimal Preco { get; init; }
}
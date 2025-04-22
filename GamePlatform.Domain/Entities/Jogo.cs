namespace GamePlatform.Domain.Entities;

public class Jogo : BaseEntity
{
    public string Titulo { get; private set; }
    public decimal Preco { get; private set; }

    public Jogo(string titulo, decimal preco)
    {
        Titulo = titulo;
        Preco = preco;
    }
}

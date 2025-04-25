using GamePlatform.Domain.Entities;
using GamePlatform.Domain.Interfaces;
using GamePlatform.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Infrastructure.Repositories;

public class JogoRepository : IJogoRepository
{
    private readonly DataContext _context;
    
    public JogoRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ExisteTituloAsync(string titulo)
    {
        return await _context.Jogos.AnyAsync(j => j.Titulo == titulo);
    }

    public async Task AdicionarAsync(Jogo jogo)
    {
        await _context.Jogos.AddAsync(jogo);
        await _context.SaveChangesAsync();
    }
}
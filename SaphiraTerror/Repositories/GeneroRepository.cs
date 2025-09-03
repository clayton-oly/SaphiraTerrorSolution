using Microsoft.EntityFrameworkCore;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;

namespace SaphiraTerror.Repositories
{
    public class GeneroRepository : IGeneroRepository
    {
        //campo de apoio
        private readonly SaphiraTerrorDbContext _context;

        //injeção de dependência no construtor
        public GeneroRepository(SaphiraTerrorDbContext context)
        {
            _context = context;
        }

        //implementar somente este metodo
        public async Task<List<Genero>> GetAllAsync()
        {
            return await _context.Generos.ToListAsync();
        }

        //stand by
        public async Task AddAsync(Genero genero)
        {
            await _context.Generos.AddAsync(genero);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero != null)
            {
                _context.Generos.Remove(genero);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Genero> GetByIdAsync(int id)
        {
            return await _context.Generos.FirstOrDefaultAsync(g => g.IdGenero == id);
        }

        public async Task UpdateAsync(Genero genero)
        {
            _context.Generos.Update(genero);
            await _context.SaveChangesAsync();
        }
    }
}
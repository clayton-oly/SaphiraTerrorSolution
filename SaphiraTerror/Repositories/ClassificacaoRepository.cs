using Microsoft.EntityFrameworkCore;
using SaphiraTerror.Data;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;

namespace SaphiraTerror.Repositories
{
    public class ClassificacaoRepository : IClassificacaoRepository
    {
        //campo de apoio
        private readonly SaphiraTerrorDbContext _context;
        //injeção de dependência no construtor
        public ClassificacaoRepository(SaphiraTerrorDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Classificacao classificacao)
        {
            _context.Classificacoes.Add(classificacao);
            await _context.SaveChangesAsync();
        }

        //listar todas as classificações
        public async Task<List<Classificacao>> GetAllAsync()
        {
            return await _context.Classificacoes.Include(c => c.Filmes).ToListAsync();
        }

        public async Task<Classificacao> GetByIdAsync(int id)
        {
            return await _context.Classificacoes.Include(c => c.Filmes).FirstOrDefaultAsync(c => c.IdClassificacao == id);
        }

        public async Task UpdateAsync(Classificacao classificacao)
        {
            _context.Classificacoes.Update(classificacao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classificacao = await _context.Classificacoes.FindAsync(id);
            if (classificacao != null)
            {
                _context.Classificacoes.Remove(classificacao);
                _context.SaveChanges();
            }
        }
    }
}

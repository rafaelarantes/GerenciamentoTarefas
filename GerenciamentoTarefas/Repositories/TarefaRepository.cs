using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefas.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ApplicationDbContext _context;

        public TarefaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tarefa>> ObterTarefasPorUsuario(string usuarioId)
        {
            return await _context.Tarefas
                .AsNoTracking()
                .Include(t => t.Categoria)
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Categoria>> ObterCategorias()
        {
            return await _context.Categorias
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Incluir(Tarefa tarefa)
        {
            _context.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task<Tarefa?> ObterTarefaPorId(int? id)
        {
            return await _context.Tarefas
                   .AsNoTracking()
                   .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Atualizar(Tarefa tarefa)
        {
            _context.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Excluir(int id)
        {
            var tarefa = await ObterTarefaPorId(id);
            if (tarefa != null)
            {
                _context.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
        }
    }
}

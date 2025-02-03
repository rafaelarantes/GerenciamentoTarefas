using GerenciamentoTarefas.Models;

namespace GerenciamentoTarefas.Repositories
{
    public interface ITarefaRepository
    {
        Task<List<Tarefa>> ObterTarefasPorUsuario(string usuario);

        Task<List<Categoria>> ObterCategorias();

        Task Incluir(Tarefa tarefa);

        Task<Tarefa?> ObterTarefaPorId(int? id);

        Task Atualizar(Tarefa tarefa);

        Task Excluir(int id);
    }
}

namespace GerenciamentoTarefas.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<Tarefa> Tarefas { get; set; }
    }
}

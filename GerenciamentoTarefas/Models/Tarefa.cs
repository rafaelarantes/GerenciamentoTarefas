namespace GerenciamentoTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataConclusao { get; set; }
        public string Status { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public string Usuario { get; set; }
    }
}

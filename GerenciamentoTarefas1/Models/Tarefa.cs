

namespace GerenciamentoTarefas.Models
{
    public class Tarefa
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataConclusao { get; set; }

        public string Status { get; set; }

        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public Guid UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; } // Relacionamento com ApplicationUser
    }
}

using Microsoft.AspNetCore.Identity;

namespace GerenciamentoTarefas.Models
{
    public class Usuario : IdentityUser
    {
        public ICollection<Tarefa> Tarefas { get; set; }
    }
}

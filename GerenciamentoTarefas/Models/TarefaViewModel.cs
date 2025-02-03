using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoTarefas.Models
{
    public class TarefaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da tarefa não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição da tarefa é obrigatório.")]
        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de conclusão da tarefa é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de data inválido.")]
        [Display(Name = "Data de Conclusão")]
        public DateTime DataConclusao { get; set; }

        [Required(ErrorMessage = "O status da tarefa é obrigatório.")]
        [RegularExpression(@"^(Pendente|Concluida)$", ErrorMessage = "O status deve ser 'Pendente' ou 'Concluída'.")]
        public string Status { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "A categoria da tarefa é obrigatória.")]
        public int CategoriaId { get; set; }
    }
}

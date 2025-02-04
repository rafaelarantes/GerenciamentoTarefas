using GerenciamentoTarefas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Trabalho" },
                new Categoria { Id = 2, Nome = "Pessoal" },
                new Categoria { Id = 3, Nome = "Outros" }
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplicationDbContext.SeedData(modelBuilder);

            modelBuilder.Entity<Tarefa>()
               .HasOne(t => t.Usuario)
               .WithMany(u => u.Tarefas)
               .HasForeignKey(t => t.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

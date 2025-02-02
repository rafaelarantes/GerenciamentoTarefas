using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefas.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TarefaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tarefas
        public async Task<IActionResult> Index()
        {
            var tarefas = await _context.Tarefas
                .Include(t => t.Categoria)
                .Where(t => t.UsuarioId == User.Identity.Name)
                .ToListAsync();
            return View(tarefas);
        }

        // GET: Tarefas/Create
        public IActionResult Criar()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        // POST: Tarefas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(TarefaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tarefa = new Tarefa
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    DataConclusao = model.DataConclusao,
                    Status = model.Status,
                    CategoriaId = model.CategoriaId,
                    UsuarioId = User.Identity.Name
                };

                _context.Add(tarefa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        // GET: Tarefas/Edit/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }

            var tarefaViewModel = new TarefaViewModel
            {
                Id = tarefa.Id,
                Nome = tarefa.Nome,
                Descricao = tarefa.Descricao,
                DataConclusao = tarefa.DataConclusao,
                Status = tarefa.Status,
                CategoriaId = tarefa.CategoriaId
            };

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", tarefaViewModel.CategoriaId);
            return View(tarefaViewModel);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, TarefaViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tarefa = await _context.Tarefas.FindAsync(id);
                    if (tarefa == null)
                    {
                        return NotFound();
                    }

                    tarefa.Nome = model.Nome;
                    tarefa.Descricao = model.Descricao;
                    tarefa.DataConclusao = model.DataConclusao;
                    tarefa.Status = model.Status;
                    tarefa.CategoriaId = model.CategoriaId;

                    _context.Update(tarefa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tarefas.Any(t => t.Id == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        // POST: Tarefas/Excluir/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

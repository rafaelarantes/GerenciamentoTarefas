using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Models;
using GerenciamentoTarefas.Repositories;
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
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaController(ApplicationDbContext context, ITarefaRepository tarefaRepository)
        {
            _context = context;
            _tarefaRepository = tarefaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tarefas = await _tarefaRepository.ObterTarefasPorUsuario(User.Identity.Name);
            return View(tarefas);
        }

        public async Task<IActionResult> Criar()
        {
            var categorias = await _tarefaRepository.ObterCategorias();

            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome");
            return View();
        }

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

                await _tarefaRepository.Incluir(tarefa);

                return RedirectToAction(nameof(Index));
            }

            var categorias = await _tarefaRepository.ObterCategorias();

            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _tarefaRepository.ObterTarefaPorId(id);
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

            var categorias = await _tarefaRepository.ObterCategorias();

            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome", tarefaViewModel.CategoriaId);
            return View(tarefaViewModel);
        }

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
                var tarefa = await _tarefaRepository.ObterTarefaPorId(id);
                if (tarefa == null)
                {
                    return NotFound();
                }

                tarefa.Nome = model.Nome;
                tarefa.Descricao = model.Descricao;
                tarefa.DataConclusao = model.DataConclusao;
                tarefa.Status = model.Status;
                tarefa.CategoriaId = model.CategoriaId;

                await _tarefaRepository.Atualizar(tarefa);
                
                return RedirectToAction(nameof(Index));
            }

            var categorias = await _tarefaRepository.ObterCategorias();

            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome", model.CategoriaId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            await _tarefaRepository.Excluir(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

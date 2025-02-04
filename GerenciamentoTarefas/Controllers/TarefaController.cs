using GerenciamentoTarefas.Models;
using GerenciamentoTarefas.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciamentoTarefas.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaController(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tarefas = await _tarefaRepository.ObterTarefasPorUsuario(User.Identity.Name);
            return View(tarefas);
        }

        public async Task<IActionResult> Criar()
        {
            var tarefaViewModel = new TarefaViewModel
            {
                DataConclusao = DateTime.Now,
                Categorias = await PreencherCategoria()
            };

            return View(tarefaViewModel);
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
                    Usuario = User.Identity.Name
                };

                await _tarefaRepository.Incluir(tarefa);

                return RedirectToAction(nameof(Index));
            }

            var tarefaViewModel = new TarefaViewModel
            {
                Categorias = await PreencherCategoria()
            };

            return View(tarefaViewModel);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            var tarefa = await _tarefaRepository.ObterTarefaPorId(id);

            var tarefaViewModel = new TarefaViewModel
            {
                Id = tarefa.Id,
                Nome = tarefa.Nome,
                Descricao = tarefa.Descricao,
                DataConclusao = tarefa.DataConclusao,
                Status = tarefa.Status,
                CategoriaId = tarefa.CategoriaId,
                Categorias = await PreencherCategoria(tarefa.CategoriaId)
            };

            return View(tarefaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, TarefaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tarefa = await _tarefaRepository.ObterTarefaPorId(id);

                tarefa.Nome = model.Nome;
                tarefa.Descricao = model.Descricao;
                tarefa.DataConclusao = model.DataConclusao;
                tarefa.Status = model.Status;
                tarefa.CategoriaId = model.CategoriaId;

                await _tarefaRepository.Atualizar(tarefa);
                
                return RedirectToAction(nameof(Index));
            }

            model.Categorias = await PreencherCategoria(model.CategoriaId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            await _tarefaRepository.Excluir(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> PreencherCategoria(int categoriaId = 0)
        {
            var categorias = await _tarefaRepository.ObterCategorias();

            return new SelectList(categorias, "Id", "Nome", categoriaId);
        }
    }
}

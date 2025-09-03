using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.ViewModels;

namespace SaphiraTerror.Controllers
{
    public class TipoUsuarioController : Controller
    {
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public TipoUsuarioController(ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        //metodo de apoio criar filmeVM
        private async Task<TipoUsuarioViewModel> CriarTipoUsuarioViewModel(TipoUsuarioViewModel? model = null)
        {
            var tipoUsuarios = await _tipoUsuarioRepository.GetAllAsync();

            return new TipoUsuarioViewModel
            {
                IdTipoUsuario = model?.IdTipoUsuario ?? 0,
                DescricaoTipoUsuario = model?.DescricaoTipoUsuario,
            };
        }

        //index
        //[Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index()
        {
            var tipoUsuarios = await _tipoUsuarioRepository.GetAllAsync();

            //orderm decrescente
            tipoUsuarios = tipoUsuarios.OrderByDescending(f => f.IdTipoUsuario).ToList();

            return View(tipoUsuarios);
        }

        //create
        //[Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var model = await CriarTipoUsuarioViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoUsuarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tipoUsuario = new TipoUsuario
                {
                    DescricaoTipoUsuario = viewModel.DescricaoTipoUsuario,
                };

                await _tipoUsuarioRepository.AddAsync(tipoUsuario);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarTipoUsuarioViewModel(viewModel);
            return View(viewModel);
        }
        //edit
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var tipoUsuario = await _tipoUsuarioRepository.GetByIdAsync(id);
            if (tipoUsuario == null) return NotFound();

            var viewModel = new TipoUsuarioViewModel
            {
                IdTipoUsuario = tipoUsuario.IdTipoUsuario,
                DescricaoTipoUsuario = tipoUsuario.DescricaoTipoUsuario,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoUsuarioViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tipoUsuario = await _tipoUsuarioRepository.GetByIdAsync(id);
                if (tipoUsuario == null) return NotFound();

                tipoUsuario.DescricaoTipoUsuario = viewModel.DescricaoTipoUsuario;

                await _tipoUsuarioRepository.UpdateAsync(tipoUsuario);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarTipoUsuarioViewModel(viewModel);
            return View(viewModel);
        }

        //delete
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            var tipoUsuario = await _tipoUsuarioRepository.GetByIdAsync(id);
            if (tipoUsuario == null) return NotFound();

            return View(tipoUsuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tipoUsuarioRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.ViewModels;

namespace SaphiraTerror.Controllers
{
    public class GeneroController : Controller
    {
        private readonly IGeneroRepository _generoRepository;

        public GeneroController(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        //metodo de apoio criar filmeVM
        private async Task<GeneroViewModel> CriarGeneroViewModel(GeneroViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();

            return new GeneroViewModel
            {
                IdGenero = model?.IdGenero ?? 0,
                DescricaoGenero = model?.DescricaoGenero
            };
        }

        //index
        //[Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index()
        {
            var generos = await _generoRepository.GetAllAsync();

            //orderm decrescente
            generos = generos.OrderByDescending(f => f.IdGenero).ToList();

            return View(generos);
        }

        //create
        //[Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var model = await CriarGeneroViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GeneroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = new Genero
                {
                    DescricaoGenero = viewModel.DescricaoGenero,
                };

                await _generoRepository.AddAsync(genero);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarGeneroViewModel(viewModel);
            return View(viewModel);
        }
        //edit
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null) return NotFound();

            var viewModel = new GeneroViewModel
            {
                IdGenero = genero.IdGenero,
                DescricaoGenero = genero.DescricaoGenero,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GeneroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = await _generoRepository.GetByIdAsync(id);
                if (genero == null) return NotFound();

                genero.DescricaoGenero = viewModel.DescricaoGenero;

                await _generoRepository.UpdateAsync(genero);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarGeneroViewModel(viewModel);
            return View(viewModel);
        }

        //delete
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            var genero = await _generoRepository.GetByIdAsync(id);
            if (genero == null) return NotFound();

            return View(genero);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _generoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
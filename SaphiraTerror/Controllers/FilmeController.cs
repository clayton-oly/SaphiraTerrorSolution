using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.ViewModels;

namespace SaphiraTerror.Controllers
{
    public class FilmeController : Controller
    {
        //campo de apoio para injeção de dependência
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IClassificacaoRepository _classificacaoRepository;

        //construtuor
        public FilmeController(IFilmeRepository filmeRepository, IGeneroRepository generoRepository, IClassificacaoRepository classificacaoRepository)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _classificacaoRepository = classificacaoRepository;
        }

        //metodo de apoio criar filmeVM
        private async Task<FilmeViewModel> CriarFilmeViewModel(FilmeViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();
            var classificacoes = await _classificacaoRepository.GetAllAsync();

            return new FilmeViewModel
            {
                IdFilme = model?.IdFilme ?? 0,
                TituloFilme = model?.TituloFilme,
                ProdutoraFilme = model?.ProdutoraFilme,
                GeneroId = model?.GeneroId ?? 0,
                ClassificacaoId = model?.ClassificacaoId ?? 0,
                UrlImagem = model?.UrlImagem,
                ImagemUpload = model?.ImagemUpload,
                Generos = generos.Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero
                }),
                Classificacoes = classificacoes.Select(c => new SelectListItem
                {
                    Value = c.IdClassificacao.ToString(),
                    Text = c.DescricaoClassificacao
                })
            };
        }

        //index
        //[Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index(int? generoId, string? search)
        {
            var filmes = await _filmeRepository.GetAllAsync();

            //filtro
            if (generoId.HasValue && generoId.Value > 0)
                filmes = filmes.Where(f => f.GeneroId == generoId).ToList();

            //search
            if (!string.IsNullOrEmpty(search))
                filmes = filmes.Where(f => f.Titulo.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            //orderm decrescente
            filmes = filmes.OrderByDescending(f => f.IdFilme).ToList();

            //componentes
            ViewBag.Generos = new SelectList(await _generoRepository.GetAllAsync(), "IdGenero", "DescricaoGenero");
            ViewBag.FiltroGeneroId = generoId;
            ViewBag.Search = search;

            return View(filmes);
        }

        //create
        //[Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var model = await CriarFilmeViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string? caminhoImagem = null;
                if (viewModel.ImagemUpload != null)
                {
                    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagemUpload.FileName);
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", nomeArquivo);

                    //criar a pasta se não existir
                    using var stream = new FileStream(caminho, FileMode.Create);
                    await viewModel.ImagemUpload.CopyToAsync(stream);
                    caminhoImagem = "/img/" + nomeArquivo;
                }

                var filme = new Filme
                {
                    Titulo = viewModel.TituloFilme,
                    Produtora = viewModel.ProdutoraFilme,
                    GeneroId = viewModel.GeneroId,
                    ClassificacaoId = viewModel.ClassificacaoId,
                    UrlImagem = caminhoImagem,
                };
                await _filmeRepository.AddAsync(filme);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);
        }

        //edit
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null) return NotFound();

            var viewModel = new FilmeViewModel
            {
                IdFilme = filme.IdFilme,
                TituloFilme = filme.Titulo,
                ProdutoraFilme = filme.Produtora,
                UrlImagem = filme.UrlImagem,
                GeneroId = filme.GeneroId,
                ClassificacaoId = filme.ClassificacaoId,
                Generos = (await _generoRepository.GetAllAsync()).Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero

                }),
                Classificacoes = (await _classificacaoRepository.GetAllAsync()).Select(g => new SelectListItem
                {
                    Value = g.IdClassificacao.ToString(),
                    Text = g.DescricaoClassificacao
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FilmeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var filme = await _filmeRepository.GetByIdAsync(id);
                if (filme == null) return NotFound();

                filme.Titulo = viewModel.TituloFilme;
                filme.Produtora = viewModel.ProdutoraFilme;
                filme.ClassificacaoId = viewModel.ClassificacaoId;
                filme.GeneroId = viewModel.GeneroId;

                if (viewModel.ImagemUpload != null)
                {
                    var nomeArquivo = Guid.NewGuid() + Path.GetExtension(viewModel.ImagemUpload.FileName);
                    var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);
                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await viewModel.ImagemUpload.CopyToAsync(stream);
                    }
                    filme.UrlImagem = "/img/" + nomeArquivo;
                }
                await _filmeRepository.UpdateAsync(filme);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);

        }

        //delete
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null) return NotFound();

            return View(filme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _filmeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
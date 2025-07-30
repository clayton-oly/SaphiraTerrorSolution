using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaphiraTerror.Interfaces;
using SaphiraTerror.Models;
using SaphiraTerror.ViewModels;

namespace SaphiraTerror.Controllers
{
    public class FilmeController : Controller
    {
        //campo de apoio
        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IClassificacaoRepository _classificacaoRepository;

        //injeção de dependencia
        public FilmeController(IFilmeRepository filmeRepository, IGeneroRepository generoRepository, IClassificacaoRepository classificacaoRepository)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _classificacaoRepository = classificacaoRepository;
        }

        //create filme viewmodel
        private async Task<FilmeViewModel> CriarFilmeViewModel(FilmeViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();
            var classificacoes = await _classificacaoRepository.GetAllAsync();

            return new FilmeViewModel
            {
                IdFilmeViewModel = model?.IdFilmeViewModel ?? 0,
                TituloFilmeViewModel = model?.TituloFilmeViewModel,
                ProdutoraFilmeViewModel = model?.ProdutoraFilmeViewModel,
                GeneroIdFilmeViewModel = model?.GeneroIdFilmeViewModel ?? 0,
                ClassificacaoIdFilmeViewModel = model?.ClassificacaoIdFilmeViewModel ?? 0,

                UrlImagemFilmeViewModel = model?.UrlImagemFilmeViewModel,
                ImagemUpload = model?.ImagemUpload,

                Generos = generos.Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero.ToString()
                }),

                Classificacoes = classificacoes.Select(c => new SelectListItem
                {
                    Value = c.IdClassificacao.ToString(),
                    Text = c.DescricaoClassificacao.ToString()
                })
            };
        }

        //refator em seguida
        public IActionResult Index()
        {
            return View();
        }

        //create
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarFilmeViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmeViewModel viewModel)
        {
            if (ModelState.IsValid) 
            {
                string caminhoImagem = null;
                if (viewModel.ImagemUpload != null) 
                {
                    var nomeArquivo = Guid.NewGuid().ToString()+Path.GetExtension(viewModel.ImagemUpload.FileName);
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", nomeArquivo);
                    var stream = new FileStream(caminho, FileMode.Create);
                    await viewModel.ImagemUpload.CopyToAsync(stream);
                    caminhoImagem = "img/" + nomeArquivo;
                }

                var filme = new Filme
                {
                    Titulo = viewModel.TituloFilmeViewModel,
                    Produtora = viewModel.ProdutoraFilmeViewModel,
                    GeneroId = viewModel.GeneroIdFilmeViewModel,
                    ClassificacaoId = viewModel.ClassificacaoIdFilmeViewModel,
                    UrlImagem = viewModel.UrlImagemFilmeViewModel
                };

                await _filmeRepository.AddAsync(filme);
                return RedirectToAction(nameof(Index));
            }

            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);
        }
    }
}

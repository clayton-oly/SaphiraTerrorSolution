using Microsoft.AspNetCore.Mvc.Rendering;

namespace SaphiraTerror.ViewModels
{
    public class FilmeViewModel
    {
        public int IdFilme { get; set; }
        public string TituloFilme { get; set; }
        public string ProdutoraFilme { get; set; }
        public string? UrlImagem { get; set; }
        public IFormFile? ImagemUpload { get; set; }
        public int ClassificacaoId { get; set; }
        public int GeneroId { get; set; }

        //Coleção para popular o dropdown
        public IEnumerable<SelectListItem>? Classificacoes { get; set; }
        public IEnumerable<SelectListItem>? Generos { get; set; }
    }
}

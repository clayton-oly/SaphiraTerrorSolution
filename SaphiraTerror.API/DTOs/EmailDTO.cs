using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.API.DTOs
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "Nome do Rementente é obrigatorio")]
        public string NomeRemetente { get; set; }
        [Required(ErrorMessage = "E-mail do Rementente é obrigatorio")]
        public string EmailRementente { get; set; }
        public string? Telefone { get; set; }
        [Required(ErrorMessage = "Assunto é obrigatorio")]
        public string Assunto { get; set; }
        [Required(ErrorMessage = "Mensagem é obrigatorio")]
        public string Mensagem { get; set; }
    }
}

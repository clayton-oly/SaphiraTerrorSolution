using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SaphiraTerror.API.DTOs;
using SaphiraTerror.API.Interfaces;

namespace SaphiraTerror.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        //Configurar a injeção de dependencia
        private readonly IEmailService _emailService;

        //método construtor (atalho ctor)
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public ActionResult Post([FromBody] EmailDTO email)
        {
            //EmailService Injetado(Injeção de dependencia)
            _emailService.Enviar(email);

            return Ok("E-mail enviado com sucesso!");
        }
    }
}

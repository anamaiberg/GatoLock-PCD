using Microsoft.AspNetCore.Mvc;
using GatoLockAPI.Models;
using GatoLockAPI.Services;

namespace GatoLockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MensagensController : ControllerBase
{
    private readonly MensagemService _service;

    public MensagensController(
        MensagemService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(
            _service.ObterTodas()
        );
    }

    [HttpPost]
    public IActionResult Post(
        [FromBody] Mensagem mensagem)
    {
        _service.Adicionar(mensagem);

        return Ok(new
        {
            sucesso = true,
            mensagem = "Mensagem cadastrada!"
        });
    }
}
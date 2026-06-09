using Microsoft.AspNetCore.Mvc;
using GatoLockAPI.Models;
using GatoLockAPI.Services;

namespace GatoLockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MensagensController : ControllerBase
{
    private readonly MensagemService _service;
    private readonly SolicitacoesQueueService _fila;

    public MensagensController(
        MensagemService service,
        SolicitacoesQueueService fila)
    {
        _service = service;
        _fila = fila;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(
            _service.ObterTodas()
        );
    }

    [HttpGet("fila")]
    public IActionResult Fila()
    {
        return Ok(_fila.ObterFilaAtual());
    }

    [HttpPost]
    public IActionResult Post(
        [FromBody] Mensagem mensagem)
    {
        var solicitacao = _service.Adicionar(mensagem);

        return Accepted(new
        {
            sucesso = true,
            mensagem = "Mensagem enfileirada!",
            id = solicitacao.Id,
            status = solicitacao.Status.ToString()
        });
    }
}
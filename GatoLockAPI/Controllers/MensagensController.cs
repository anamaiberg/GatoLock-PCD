using Microsoft.AspNetCore.Mvc;
using GatoLockAPI.Models;
using GatoLockAPI.Services;
using Grpc.Core;

namespace GatoLockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MensagensController : ControllerBase
{
    private readonly MensagemService _service;
    private readonly SolicitacoesQueueService _fila;
    private readonly GrpcMensagensGateway _grpcGateway;

    public MensagensController(
        MensagemService service,
        SolicitacoesQueueService fila,
        GrpcMensagensGateway grpcGateway)
    {
        _service = service;
        _fila = fila;
        _grpcGateway = grpcGateway;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var mensagens = await _grpcGateway.ObterTodasViaGrpcAsync();

            return Ok(mensagens);
        }
        catch (RpcException)
        {
            return Ok(_service.ObterTodas());
        }
    }

    [HttpGet("grpc")]
    public async Task<IActionResult> GetViaGrpc()
    {
        var mensagens = await _grpcGateway.ObterTodasViaGrpcAsync();

        return Ok(mensagens);
    }

    [HttpGet("fila")]
    public IActionResult Fila()
    {
        return Ok(_fila.ObterFilaAtual());
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Mensagem mensagem)
    {
        try
        {
            var resposta = await _grpcGateway.EnfileirarViaGrpcAsync(mensagem);

            return Accepted(new
            {
                sucesso = true,
                mensagem = resposta.Mensagem,
                status = "NaFila"
            });
        }
        catch (RpcException)
        {
            var solicitacao = _service.Adicionar(mensagem);

            return Accepted(new
            {
                sucesso = true,
                mensagem = "Mensagem enfileirada (fallback local por incompatibilidade HTTP/2).",
                id = solicitacao.Id,
                status = solicitacao.Status.ToString()
            });
        }
    }
}
using GatoLockAPI.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GatoLockAPI.Services;

public class MensagensGrpcService : MensagensGrpc.MensagensGrpcBase
{
    private readonly MensagemService _service;

    public MensagensGrpcService(MensagemService service)
    {
        _service = service;
    }

    public override Task<ListaMensagensResponse> ObterTodas(
        Empty request,
        ServerCallContext context)
    {
        var response = new ListaMensagensResponse();

        response.Mensagens.AddRange(
            _service.ObterTodas().ConvertAll(ParaDto)
        );

        return Task.FromResult(response);
    }

    public override Task<OperacaoResponse> Adicionar(
        MensagemRequest request,
        ServerCallContext context)
    {
        var mensagem = new Mensagem
        {
            NomeAdotante = request.NomeAdotante,
            NomeGato = request.NomeGato,
            Texto = request.Texto
        };

        var solicitacao = _service.Adicionar(mensagem);

        return Task.FromResult(new OperacaoResponse
        {
            Sucesso = true,
            Mensagem = $"Mensagem enfileirada com id {solicitacao.Id}!"
        });
    }

    private static MensagemDto ParaDto(Mensagem mensagem)
    {
        return new MensagemDto
        {
            Id = mensagem.Id,
            NomeAdotante = mensagem.NomeAdotante,
            NomeGato = mensagem.NomeGato,
            Texto = mensagem.Texto
        };
    }
}
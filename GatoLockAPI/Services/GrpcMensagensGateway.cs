using GatoLockAPI.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace GatoLockAPI.Services;

public class GrpcMensagensGateway
{
    private readonly MensagensGrpc.MensagensGrpcClient _client;

    public GrpcMensagensGateway(IConfiguration configuration)
    {
        var endereco = configuration["Grpc:Endereco"] ?? "http://localhost:5248";

        var channel = GrpcChannel.ForAddress(endereco);
        _client = new MensagensGrpc.MensagensGrpcClient(channel);
    }

    public async Task<OperacaoResponse> EnfileirarViaGrpcAsync(Mensagem mensagem)
    {
        var resposta = await _client.AdicionarAsync(new MensagemRequest
        {
            NomeAdotante = mensagem.NomeAdotante,
            NomeGato = mensagem.NomeGato,
            Texto = mensagem.Texto
        });

        return resposta;
    }

    public async Task<IReadOnlyList<MensagemDto>> ObterTodasViaGrpcAsync()
    {
        var resposta = await _client.ObterTodasAsync(new Empty());

        return resposta.Mensagens.ToList();
    }
}
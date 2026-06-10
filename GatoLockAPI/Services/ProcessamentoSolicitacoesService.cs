using GatoLockAPI.Hubs;
using GatoLockAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace GatoLockAPI.Services;

public class ProcessamentoSolicitacoesService : BackgroundService
{
    private readonly SolicitacoesQueueService _fila;
    private readonly MensagemService _mensagens;
    private readonly IHubContext<SolicitacoesHub> _hub;

    public ProcessamentoSolicitacoesService(
        SolicitacoesQueueService fila,
        MensagemService mensagens,
        IHubContext<SolicitacoesHub> hub)
    {
        _fila = fila;
        _mensagens = mensagens;
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var solicitacao in _fila.Reader.ReadAllAsync(stoppingToken))
        {
            _fila.MarcarProcessando(solicitacao);

            await _hub.Clients.All.SendAsync(
                "solicitacaoProcessando",
                solicitacao,
                stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            _fila.Concluir(solicitacao);
            _mensagens.RegistrarConcluida(solicitacao);

            await _hub.Clients.All.SendAsync(
                "solicitacaoConcluida",
                solicitacao,
                stoppingToken);

            await AtualizarFila(stoppingToken);
        }
    }

    private Task AtualizarFila(CancellationToken cancellationToken)
    {
        var filaAtual = _fila.ObterFilaAtual();

        return _hub.Clients.All.SendAsync(
            "filaAtualizada",
            filaAtual,
            cancellationToken);
    }
}
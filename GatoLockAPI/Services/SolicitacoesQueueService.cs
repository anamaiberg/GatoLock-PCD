using System.Threading.Channels;
using GatoLockAPI.Models;

namespace GatoLockAPI.Services;

public class SolicitacoesQueueService
{
    private readonly object _lock = new();
    private readonly Channel<SolicitacaoAdocao> _fila = Channel.CreateUnbounded<SolicitacaoAdocao>();
    private readonly List<SolicitacaoAdocao> _emEspera = new();
    private readonly List<SolicitacaoAdocao> _processadas = new();
    private int _proximoId = 1;

    public ChannelReader<SolicitacaoAdocao> Reader => _fila.Reader;

    public SolicitacaoAdocao Enfileirar(Mensagem mensagem)
    {
        var solicitacao = new SolicitacaoAdocao
        {
            Id = ProximoId(),
            NomeAdotante = mensagem.NomeAdotante,
            NomeGato = mensagem.NomeGato,
            Texto = mensagem.Texto,
            Status = SolicitacaoStatus.NaFila,
            CriadaEm = DateTime.UtcNow
        };

        lock (_lock)
        {
            _emEspera.Add(solicitacao);
        }

        _fila.Writer.TryWrite(solicitacao);

        return solicitacao;
    }

    public void MarcarProcessando(SolicitacaoAdocao solicitacao)
    {
        lock (_lock)
        {
            solicitacao.Status = SolicitacaoStatus.Processando;

            _emEspera.RemoveAll(item => item.Id == solicitacao.Id);
        }
    }

    public void Concluir(SolicitacaoAdocao solicitacao)
    {
        lock (_lock)
        {
            solicitacao.Status = SolicitacaoStatus.Concluida;
            solicitacao.ProcessadaEm = DateTime.UtcNow;
            _processadas.Insert(0, solicitacao);
        }
    }

    public List<SolicitacaoAdocao> ObterProcessadas()
    {
        lock (_lock)
        {
            return _processadas
                .Select(Clone)
                .ToList();
        }
    }

    public List<SolicitacaoAdocao> ObterFilaAtual()
    {
        lock (_lock)
        {
            return _emEspera
                .Select(Clone)
                .ToList();
        }
    }

    private int ProximoId()
    {
        lock (_lock)
        {
            return _proximoId++;
        }
    }

    private static SolicitacaoAdocao Clone(SolicitacaoAdocao item)
    {
        return new SolicitacaoAdocao
        {
            Id = item.Id,
            NomeAdotante = item.NomeAdotante,
            NomeGato = item.NomeGato,
            Texto = item.Texto,
            Status = item.Status,
            CriadaEm = item.CriadaEm,
            ProcessadaEm = item.ProcessadaEm
        };
    }
}
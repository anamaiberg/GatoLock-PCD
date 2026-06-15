using System.Threading.Channels;
using GatoLockAPI.Models;

namespace GatoLockAPI.Services;

public class SolicitacoesQueueService
{
    private readonly Mutex _mutex = new();
    private readonly Channel<SolicitacaoAdocao> _fila = Channel.CreateUnbounded<SolicitacaoAdocao>();
    private readonly List<SolicitacaoAdocao> _emEspera = new();
    private readonly List<SolicitacaoAdocao> _processadas = new();
    private int _proximoId = 1;

    public ChannelReader<SolicitacaoAdocao> Reader => _fila.Reader;

    public SolicitacaoAdocao Enfileirar(Mensagem mensagem)
    {
        _mutex.WaitOne();

        try
        {
            var solicitacao = new SolicitacaoAdocao
            {
                Id = _proximoId++,
                NomeAdotante = mensagem.NomeAdotante,
                NomeGato = mensagem.NomeGato,
                Texto = mensagem.Texto,
                Status = SolicitacaoStatus.NaFila,
                CriadaEm = DateTime.UtcNow
            };

            _emEspera.Add(solicitacao);

            _fila.Writer.TryWrite(solicitacao);

            return solicitacao;
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    public void MarcarProcessando(SolicitacaoAdocao solicitacao)
    {
        _mutex.WaitOne();

        try
        {
            solicitacao.Status = SolicitacaoStatus.Processando;

            _emEspera.RemoveAll(item => item.Id == solicitacao.Id);
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    public void Concluir(SolicitacaoAdocao solicitacao)
    {
        _mutex.WaitOne();

        try
        {
            solicitacao.Status = SolicitacaoStatus.Concluida;
            solicitacao.ProcessadaEm = DateTime.UtcNow;
            _processadas.Insert(0, solicitacao);
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    public List<SolicitacaoAdocao> ObterProcessadas()
    {
        _mutex.WaitOne();

        try
        {
            return _processadas
                .Select(Clone)
                .ToList();
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    public List<SolicitacaoAdocao> ObterFilaAtual()
    {
        _mutex.WaitOne();

        try
        {
            return _emEspera
                .Select(Clone)
                .ToList();
        }
        finally
        {
            _mutex.ReleaseMutex();
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
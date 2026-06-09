using GatoLockAPI.Models;

namespace GatoLockAPI.Services;

public class MensagemService
{
    private readonly List<Mensagem> _mensagensConcluidas = new();
    private readonly object _lock = new();
    private readonly SolicitacoesQueueService _fila;

    public MensagemService(SolicitacoesQueueService fila)
    {
        _fila = fila;
    }

    public List<Mensagem> ObterTodas()
    {
        lock (_lock)
        {
            return _mensagensConcluidas
                .Select(mensagem => new Mensagem
                {
                    Id = mensagem.Id,
                    NomeAdotante = mensagem.NomeAdotante,
                    NomeGato = mensagem.NomeGato,
                    Texto = mensagem.Texto
                })
                .ToList();
        }
    }

    public SolicitacaoAdocao Adicionar(Mensagem mensagem)
    {
        return _fila.Enfileirar(mensagem);
    }

    public void RegistrarConcluida(SolicitacaoAdocao solicitacao)
    {
        lock (_lock)
        {
            _mensagensConcluidas.Add(new Mensagem
            {
                Id = solicitacao.Id,
                NomeAdotante = solicitacao.NomeAdotante,
                NomeGato = solicitacao.NomeGato,
                Texto = solicitacao.Texto
            });
        }
    }
}
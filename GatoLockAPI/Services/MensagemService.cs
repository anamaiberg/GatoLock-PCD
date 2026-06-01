using GatoLockAPI.Models;

namespace GatoLockAPI.Services;

public class MensagemService
{
    private readonly List<Mensagem> _mensagens = new();

    public List<Mensagem> ObterTodas()
    {
        return _mensagens;
    }

    public void Adicionar(Mensagem mensagem)
    {
        mensagem.Id = _mensagens.Count + 1;

        _mensagens.Add(mensagem);
    }
}
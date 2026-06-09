namespace GatoLockAPI.Models;

public enum SolicitacaoStatus
{
    NaFila,
    Processando,
    Concluida
}

public class SolicitacaoAdocao
{
    public int Id { get; set; }

    public string NomeAdotante { get; set; } = string.Empty;

    public string NomeGato { get; set; } = string.Empty;

    public string Texto { get; set; } = string.Empty;

    public SolicitacaoStatus Status { get; set; }

    public DateTime CriadaEm { get; set; }

    public DateTime? ProcessadaEm { get; set; }
}
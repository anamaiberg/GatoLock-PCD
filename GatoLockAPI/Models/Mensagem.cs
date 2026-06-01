namespace GatoLockAPI.Models;

public class Mensagem
{
    public int Id { get; set; }

    public string NomeAdotante { get; set; } = "";

    public string NomeGato { get; set; } = "";

    public string Texto { get; set; } = "";
}
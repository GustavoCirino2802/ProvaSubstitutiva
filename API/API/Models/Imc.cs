
namespace API.Models;

public class Imc
{
    public int Id { get; set;}
    public double Altura { get; set; }
    public double Peso { get; set; }
    public double ResultadoImc { get; set; }
    public string? Classificacao { get; set; }
    public DateTime CriadoEm { get; set; }
    public int AlunoId { get; set; }
    public Aluno? aluno { get; set; }
}

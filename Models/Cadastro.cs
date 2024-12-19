namespace CadastroWebApp.Models
{
    public class Cadastro
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required DateTime DataAbertura { get; set; }
        public decimal SaldoInicial { get; set; }
        public required string TipoConta { get; set; }
    }
}

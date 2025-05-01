using Questao5.Domain.Enumerators;

namespace Questao5.Application.Queries.Responses
{
    public class ConsultarSaldoResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string TipoErro { get; set; }
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Saldo { get; set; }

        public static ConsultarSaldoResponse Ok() => new()
        {
            Sucesso = true,
        };

        public static ConsultarSaldoResponse Ok(int numeroConta, string nomeTitular, DateTime dataConsulta, decimal saldo) => new()
        {
            Sucesso = true,
            NumeroConta = numeroConta,
            Mensagem = "Consulta relizada com sucesso",
            NomeTitular = nomeTitular,
            DataConsulta = dataConsulta,
            Saldo = "R$ " + saldo
        };

        public static ConsultarSaldoResponse Erro(string mensagem, ETipoErro tipoErro) => new()
        {
            Sucesso = false,
            Mensagem = mensagem,
            TipoErro = tipoErro.GetDescription()
        };
    }
}

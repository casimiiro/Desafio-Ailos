using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Responses
{
    public class ResultadoMovimentacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string TipoErro { get; set; }
        public string IdMovimento { get; set; }

        public static ResultadoMovimentacao Ok(string idMovimento = null) => new() { Sucesso = true, IdMovimento = idMovimento };

        public static ResultadoMovimentacao Erro(string mensagem, ETipoErro tipoErro) => new() { Sucesso = false, Mensagem = mensagem, TipoErro = tipoErro.GetDescription() };
    }

}

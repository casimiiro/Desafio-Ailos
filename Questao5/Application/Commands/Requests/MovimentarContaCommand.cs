using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCommand : IRequest<ResultadoMovimentacao>
    {
        public string IdRequisicao { get; set; }
        public int NumeroConta { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}

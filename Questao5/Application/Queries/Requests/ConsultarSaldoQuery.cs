using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultarSaldoQuery : IRequest<ConsultarSaldoResponse>
    {
        public int NumeroConta { get; }

        public ConsultarSaldoQuery(int numeroConta)
        {
            NumeroConta = numeroConta;
        }
    }

}

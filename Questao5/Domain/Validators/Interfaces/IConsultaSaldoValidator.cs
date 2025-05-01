using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Validators.Interfaces
{
    public interface IConsultaSaldoValidator
    {
        ConsultarSaldoResponse Validar(ConsultarSaldoQuery query, ContaCorrente conta);
    }
}

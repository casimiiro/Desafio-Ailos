using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Validators.Interfaces
{
    public interface IMovimentacaoValidator
    {
        ResultadoMovimentacao Validar(MovimentarContaCommand comando, ContaCorrente conta);
    }
}

using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validators.Interfaces;

namespace Questao5.Domain.Validators.Saldo
{
    public class ConsultaSaldoValidator: IConsultaSaldoValidator
    {
        public ConsultarSaldoResponse Validar(ConsultarSaldoQuery query, ContaCorrente conta)
        {
            if (conta == null)
                return ConsultarSaldoResponse.Erro("Apenas contas correntes cadastradas podem consultar o saldo", ETipoErro.INVALID_ACCOUNT);

            if (conta.Ativo == 0)
                return ConsultarSaldoResponse.Erro("Apenas contas correntes ativas podem consultar o saldo", ETipoErro.INACTIVE_ACCOUNT);

            return ConsultarSaldoResponse.Ok();
        }
    }
}

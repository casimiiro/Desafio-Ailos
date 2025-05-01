using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validators.Interfaces;

namespace Questao5.Domain.Validators.Movimento
{
    public class MovimentacaoValidator : IMovimentacaoValidator
    {
        public ResultadoMovimentacao Validar(MovimentarContaCommand comando, ContaCorrente conta)
        {
            if (conta == null)
                return ResultadoMovimentacao.Erro("Apenas contas correntes cadastradas podem receber movimentação", ETipoErro.INVALID_ACCOUNT);

            if (conta.Ativo == 0)
                return ResultadoMovimentacao.Erro("Apenas contas correntes ativas podem receber movimentação", ETipoErro.INACTIVE_ACCOUNT);

            if (comando.Valor <= 0)
                return ResultadoMovimentacao.Erro("Apenas valores positivos podem ser recebidos", ETipoErro.INVALID_VALUE);

            if (comando.TipoMovimento != ETipoMovimento.Credito.GetDescription() && comando.TipoMovimento != ETipoMovimento.Debito.GetDescription())
                return ResultadoMovimentacao.Erro("Apenas os tipos “débito” ou “crédito” podem ser aceitos", ETipoErro.INVALID_TYPE);

            return ResultadoMovimentacao.Ok();
        }
    }

}

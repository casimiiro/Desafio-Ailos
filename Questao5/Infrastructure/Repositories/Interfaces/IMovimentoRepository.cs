using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface IMovimentoRepository
    {
        Task RegistrarMovimentoAsync(Movimento movimento);
        Task<decimal> ObterSaldoAsync(int numeroConta);
    }

}

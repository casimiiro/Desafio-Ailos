using System.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IDbConnection _db;

        public MovimentoRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<decimal> ObterSaldoAsync(int numeroConta)
        {
            var sql = $@"
                SELECT 
                    (
                    COALESCE(
                        SUM(
                            CASE WHEN TipoMovimento = '{ETipoMovimento.Credito.GetDescription()}' THEN Valor ELSE 0 END), 0) - 
                    COALESCE(
                        SUM(
                            CASE WHEN TipoMovimento = '{ETipoMovimento.Debito.GetDescription()}' THEN Valor ELSE 0 END), 0)) AS Saldo
                FROM Movimento m
                JOIN ContaCorrente cc ON cc.idContaCorrente = m.idContaCorrente
                WHERE cc.numero = @numeroConta";

            return await _db.QueryFirstOrDefaultAsync<decimal>(sql, new { numeroConta = numeroConta });
        }

        public async Task RegistrarMovimentoAsync(Movimento movimento)
        {
            var sql = @"
            INSERT INTO movimento 
            (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES 
            (@Id, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);";

            await _db.ExecuteAsync(sql, new
            {
                Id = movimento.Id.ToString(),
                movimento.IdContaCorrente,
                DataMovimento = movimento.DataMovimento.ToString("dd/MM/yyyy"),
                movimento.TipoMovimento,
                movimento.Valor
            });
        }
    }

}

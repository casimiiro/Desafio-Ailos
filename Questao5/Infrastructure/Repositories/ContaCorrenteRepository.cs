using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Repositories.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDbConnection _db;

        public ContaCorrenteRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<ContaCorrente> ObterPorIdAsync(int numeroConta)
        {
            var query = @"SELECT * FROM contacorrente WHERE numero = @numeroConta";
            return await _db.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { numeroConta = numeroConta });
        }
    }
}

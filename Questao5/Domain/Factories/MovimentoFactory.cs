using Questao5.Domain.Entities;
using Questao5.Domain.Factories.Interfaces;

namespace Questao5.Domain.Factories
{
    public class MovimentoFactory : IMovimentoFactory
    {
        public Movimento Criar(string idcontacorrente, string tipoMovimento, decimal valor)
        {
            return new Movimento(
                id: Guid.NewGuid().ToString(),
                idContaCorrente: idcontacorrente,
                dataMovimento: DateTime.Now,
                tipo: tipoMovimento,
                valor: valor
            );
        }
    }

}

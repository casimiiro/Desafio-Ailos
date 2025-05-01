using Questao5.Domain.Entities;

namespace Questao5.Domain.Factories.Interfaces
{
    public interface IMovimentoFactory
    {
        Movimento Criar(string idcontacorrente, string tipoMovimento, decimal valor);
    }

}

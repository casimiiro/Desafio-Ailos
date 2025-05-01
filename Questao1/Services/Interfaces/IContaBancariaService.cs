using Questao1.Models;

namespace Questao1.Services.Interfaces
{
    public interface IContaBancariaService
    {
        ContaBancaria CriarConta(int numero, string titular, double? depositoInicial = null);
        void RealizarDeposito(ContaBancaria conta, double valor);
        public void RealizarSaque(ContaBancaria conta, double valor);
    }
}

using Questao1.Models;
using Questao1.Services.Interfaces;

namespace Questao1.Services
{
    public class ContaBancariaService : IContaBancariaService
    {
        public ContaBancaria CriarConta(int numero, string titular, double? depositoInicial = null)
        {
            return new ContaBancaria(numero, titular, depositoInicial ?? 0);
        }

        public void RealizarDeposito(ContaBancaria conta, double valor)
        {
            conta.Depositar(valor);
        }

        public void RealizarSaque(ContaBancaria conta, double valor)
        {
            conta.Sacar(valor);
        }
    }
}

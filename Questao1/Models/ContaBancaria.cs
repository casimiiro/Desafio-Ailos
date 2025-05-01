using System.Globalization;

namespace Questao1.Models
{
    public class ContaBancaria
    {
        public int Numero { get; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        private const double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string titular, double saldoInicial = 0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldoInicial;
        }

        public void AlterarTitular(string novoTitular) => Titular = novoTitular;

        public void Depositar(double valor) => Saldo += valor;

        public void Sacar(double valor) => Saldo -= valor + TaxaSaque;

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}

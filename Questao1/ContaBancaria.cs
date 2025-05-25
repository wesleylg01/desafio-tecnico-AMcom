using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        private const double TaxaDeSaque = 3.50;
        private int NumeroConta { get; init; }
        private string NomeTitularConta { get; set; }
        private double? ValorDepositoInicial { get; set; }
        private double Saldo { get; set; }

        public ContaBancaria(int numero, string titular, double? depositoInicial = null)
        {
            this.NumeroConta = numero;
            this.NomeTitularConta = titular;
            this.ValorDepositoInicial = depositoInicial;

            this.Saldo = depositoInicial ?? 0;
        }

        public void Deposito(double quantia)
        {
            this.Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            this.Saldo -= (quantia + TaxaDeSaque);
        }
        public override string ToString()
        {
            return $"Conta {NumeroConta}, Titular: {NomeTitularConta}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}

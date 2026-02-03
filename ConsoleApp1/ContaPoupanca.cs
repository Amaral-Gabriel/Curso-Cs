namespace ConsoleApp1
{
    internal class ContaPoupanca : Conta
    {

        public override string TipoDeConta => "Conta Poupança";
        public override void Saque(decimal valor)

        {
            if (valor <= 0)
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            if (Saldo < valor)
            {
                Console.WriteLine("Saldo insuficiente.");
                return;
            }

            Saldo -= valor;
        }

        public void RenderJuros(decimal percentual)
        {
            if (percentual <= 0) return;

            Saldo += Saldo * percentual;
        }
    }
}

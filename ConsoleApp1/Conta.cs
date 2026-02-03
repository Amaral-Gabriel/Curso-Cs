namespace ConsoleApp1
{
    internal abstract class Conta
    {
        public int NumeroDaConta;
        public decimal Saldo;
        public Cliente Titular;

        public abstract object TipoDeConta { get; }

        public virtual void Deposito(decimal valor)
        {
            if (valor <= 0)
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            Saldo += valor;
        }

        public abstract void Saque(decimal valor);
    }
}

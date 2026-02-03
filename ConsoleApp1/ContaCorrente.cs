namespace ConsoleApp1
{
    internal class ContaCorrente : Conta
    {
      
        public override string TipoDeConta => "Conta Corrente";

        private decimal taxaDeSaque = 5;

        public override void Saque(decimal valor)
        {
            if (valor <= 0)
            {
                Console.WriteLine("Valor inválido.");
                return;
            } if (Saldo < valor)
            {
                Console.WriteLine("Saldo insuficiente.");
            }
            else
            {
                Saldo -= taxaDeSaque;
                Saldo -= valor;
            }
  

                
        }
    }
}

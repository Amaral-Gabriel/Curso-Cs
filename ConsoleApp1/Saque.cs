using System;

namespace ConsoleApp1
{
    public static class Saque
    {
        public static void Sacar(Conta conta)
        {
            Console.Clear();
            Console.WriteLine($"--- SAQUE ({conta.TipoDeConta}) ---");
            Console.WriteLine($"Saldo disponível: {conta.Saldo}"); // Ler (get) pode!

            Console.Write("Digite o valor do saque: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                // MUDANÇA: Apenas chama o método. Não mexe no saldo.
                conta.Saque(valor);
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            Util.Pausar(); // (Ou Console.ReadLine se não tiver a classe Util)
        }
    }
}
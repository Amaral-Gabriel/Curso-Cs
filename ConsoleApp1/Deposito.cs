using System;

namespace ConsoleApp1
{
    // Adicionamos 'static' na classe
    public static class Deposito
    {
        // Adicionamos 'static' no método
        public static void Depositar(Conta conta)
        {
            Console.Write("Valor do depósito: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                // Chama o método da conta (que já valida se é > 0)
                conta.Deposito(valor);
                Console.WriteLine("Depósito realizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }

            Util.Pausar();
        }
    }
}
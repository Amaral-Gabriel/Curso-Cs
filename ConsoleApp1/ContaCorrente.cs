using System;

namespace ConsoleApp1
{
    public class ContaCorrente : Conta
    {
        public ContaCorrente()
        {
            TipoDeConta = "Conta Corrente";
        }

        
        public override void Saque(decimal valor)
        {
            decimal taxa = 5.00m; 
            decimal totalDoSaque = valor + taxa; 

            if (totalDoSaque > Saldo)
            {
                Console.WriteLine($"Saldo insuficiente! Você precisa de {totalDoSaque:C} (Saque + Taxa de {taxa:C})");
                Console.WriteLine($"Seu saldo atual é apenas: {Saldo:C}");
            }
            else
            {
                Saldo -= totalDoSaque; 
                Console.WriteLine($"Saque de {valor:C} realizado com sucesso!");
                Console.WriteLine($"Taxa de operação: {taxa:C}");
            }
        }
    }
}
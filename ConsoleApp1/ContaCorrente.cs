using System;

namespace ConsoleApp1
{
    public class ContaCorrente : Conta
    {
        public ContaCorrente()
        {
            TipoDeConta = "Conta Corrente";
        }

        // AQUI ESTÁ A LÓGICA DA TAXA DE 5 REAIS
        public override void Saque(decimal valor)
        {
            decimal taxa = 5.00m; // Define a taxa fixa
            decimal totalDoSaque = valor + taxa; // Valor + 5 reais

            if (totalDoSaque > Saldo)
            {
                Console.WriteLine($"Saldo insuficiente! Você precisa de {totalDoSaque:C} (Saque + Taxa de {taxa:C})");
                Console.WriteLine($"Seu saldo atual é apenas: {Saldo:C}");
            }
            else
            {
                Saldo -= totalDoSaque; // Tira o dinheiro E a taxa da conta
                Console.WriteLine($"Saque de {valor:C} realizado com sucesso!");
                Console.WriteLine($"Taxa de operação: {taxa:C}");
            }
        }
    }
}
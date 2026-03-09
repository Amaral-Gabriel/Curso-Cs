using System;

namespace ConsoleApp1
{
    public class ContaPoupanca : Conta
    {
        public ContaPoupanca()
        {
            TipoDeConta = "Conta Poupança";
        }

        public override void Saque(decimal valor)
        {
            
            if (valor > Saldo)
            {
                Console.WriteLine("Saldo insuficiente!");
            }
            else
            {
                Saldo -= valor;
                Console.WriteLine($"Saque de {valor:C} realizado.");
            }

        }

        
        public void RenderJuros()
        {
            decimal taxa = 0.01m;
            decimal lucro = Saldo * taxa;

            Saldo += lucro;

            Console.WriteLine($"\n[SUCESSO] Rendimento aplicado!");
            Console.WriteLine($"Você recebeu: {lucro:C}");
            Console.WriteLine($"Novo Saldo: {Saldo:C}");
        }
    }
}
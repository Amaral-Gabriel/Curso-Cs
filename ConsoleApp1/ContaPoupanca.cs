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
            // Taxa de 0.10 centavos
            if (valor + 0.10m > Saldo)
            {
                Console.WriteLine("Saldo insuficiente para saque + taxa!");
            }
            else
            {
                Saldo -= (valor + 0.10m);
                Console.WriteLine($"Saque de {valor:C} realizado (Taxa: R$ 0,10)");
            }
        }

        // --- NOVO: FAZ O DINHEIRO CRESCER ---
        public void RenderJuros()
        {
            decimal taxa = 0.05m; // 5% de rendimento
            decimal lucro = Saldo * taxa;

            Saldo += lucro;

            Console.WriteLine($"\n[SUCESSO] Rendimento aplicado!");
            Console.WriteLine($"Você ganhou: {lucro:C}");
            Console.WriteLine($"Novo Saldo: {Saldo:C}");
        }
    }
}
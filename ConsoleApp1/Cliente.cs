using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Cliente
    {
        public int Id;
        public string Nome;

        public List<Conta> Contas = new List<Conta>();

        public void DadosCliente()
        {
            Console.WriteLine($"Nome: {Nome}");
            Console.WriteLine($"Id: {Id}");
        }

        public Conta VerContas()
        {
            if (Contas.Count == 0)
            {
                Console.WriteLine("Não há contas cadastradas.");
                return null;
            }

            Console.WriteLine("=== Contas abertas ===");

            for (int i = 0; i < Contas.Count; i++)
            {
                var c = Contas[i];
                Console.WriteLine($"{i + 1} - Conta: {c.NumeroDaConta} | Tipo: {c.GetType().Name}");
            }

            Console.Write("Escolha a conta pelo número da lista: ");
            if (!int.TryParse(Console.ReadLine(), out int opcao) || opcao < 1 || opcao > Contas.Count)
            {
                Console.WriteLine("Conta não encontrada.");
                return null;
            }

            return Contas[opcao - 1]; // retorna a conta selecionada
        }

        public Conta AcessarConta(int numeroDaConta)
        {
            foreach (var conta in Contas)
            {
                if (conta.NumeroDaConta == numeroDaConta)
                {
                    return conta;
                }
            }

            Console.WriteLine("Conta não encontrada.");
            return null;
        }
    }
}

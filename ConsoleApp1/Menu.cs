using System;
using System.Linq;

namespace ConsoleApp1
{
    public static class Menu
    {
        public static void MenuPrincipal(Cliente cliente)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Olá, {cliente.Nome.ToUpper()} | Acesso Concedido");
                Console.WriteLine("---------------------------------");

                cliente.MostrarContas();

                Console.WriteLine("\n1. Acessar Conta");
                Console.WriteLine("2. Sair");
                Console.Write("\nOpção: ");

                string opcao = Console.ReadLine();

                if (opcao == "1") AcessarConta(cliente);
                else if (opcao == "2") break;
            }
        }

        private static void AcessarConta(Cliente cliente)
        {
            Console.Write("\nInforme o NÚMERO da conta: ");
            if (int.TryParse(Console.ReadLine(), out int numero))
            {
                Conta conta = cliente.Contas.FirstOrDefault(c => c.NumeroDaConta == numero);

                if (conta != null)
                {
                    OperacoesConta(conta);
                }
                else
                {
                    Console.WriteLine("Conta não encontrada!");
                    Console.ReadLine();
                }
            }
        }

        private static void OperacoesConta(Conta conta)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== CONTA {conta.NumeroDaConta} ({conta.TipoDeConta}) ===");
                Console.WriteLine($"Saldo Disponível: {conta.Saldo:C}");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1. Sacar");
                Console.WriteLine("2. Depositar");

                // SE FOR POUPANÇA, MOSTRA A OPÇÃO DE RENDER
                if (conta is ContaPoupanca)
                {
                    Console.WriteLine("3. Render Juros (Simular Mês)");
                }

                Console.WriteLine("4. Voltar");
                Console.Write("Opção: ");

                string op = Console.ReadLine();

                if (op == "1")
                {
                    Console.Write("Valor Saque: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal v))
                    {
                        conta.Saque(v);
                        Console.ReadLine();
                    }
                }
                else if (op == "2")
                {
                    Console.Write("Valor Depósito: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal v))
                    {
                        conta.Deposito(v);
                        Console.WriteLine("Sucesso!");
                        Console.ReadLine();
                    }
                }
                else if (op == "3" && conta is ContaPoupanca poupanca)
                {
                    // AQUI A MÁGICA ACONTECE
                    poupanca.RenderJuros();
                    Console.ReadLine();
                }
                else if (op == "4")
                {
                    break;
                }
            }
        }
    }
}
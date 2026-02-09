using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            // 1. Carrega os dados silenciosamente
            List<Cliente> clientes = BancoDeDados.Carregar();

            // 2. Se for a primeira vez, cria o Admin
            if (clientes.Count == 0)
            {
                clientes = GerarDadosTeste();
                BancoDeDados.Salvar(clientes);
            }

            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("         BANCO MASTER       ");
                Console.WriteLine("=================================");

                Console.Write("Usuário: ");
                string nome = Console.ReadLine();

                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                
                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(senha))
                {
                    continue;
                }

                
                Cliente clienteLogado = clientes.FirstOrDefault(c => c.Nome == nome && c.Senha == senha);

                if (clienteLogado != null)
                {
                    Menu.MenuPrincipal(clienteLogado);

                    BancoDeDados.Salvar(clientes);
                }
                else
                {
                    Console.WriteLine("\n[X] Login inválido! Tente novamente.");
                    Console.WriteLine("Pressione ENTER...");
                    Console.ReadLine();
                }
            }
        }

        static List<Cliente> GerarDadosTeste()
        {
            var lista = new List<Cliente>();

            // Cria o Admin padrão
            var admin = new Cliente { Id = 1, Nome = "Admin", Senha = "123" };
            var contaAdmin = new ContaCorrente { NumeroDaConta = 1001 };
            contaAdmin.Deposito(10000); // Começa com 10 mil
            admin.AdicionarConta(contaAdmin);

            lista.Add(admin);
            return lista;
        }
    }
}
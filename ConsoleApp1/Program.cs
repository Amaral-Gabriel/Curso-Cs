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
            

            while (true)
            {
                Console.Clear();
                
                Console.WriteLine(@" 
    ┌───────────────────────────────────────────────────────────────────────────┐
    │                                                                           │
    │                                                                           │
    │   ____    _    _   _  ____ ___    __  __    _    ____ _____ _____ ____    │
    │  | __ )  / \  | \ | |/ ___/ _ \  |  \/  |  / \  / ___|_   _| ____|  _ \   │
    │  |  _ \ / _ \ |  \| | |  | | | | | |\/| | / _ \ \___ \ | | |  _| | |_) |  │
    │  | |_) / ___ \| |\  | |__| |_| | | |  | |/ ___ \ ___) || | | |___|  _ <   │
    │  |____/_/   \_\_| \_|\____\___/  |_|  |_/_/   \_\____/ |_| |_____|_| \_\  │
    │                                                                           │
    │                                                                           │
    └───────────────────────────────────────────────────────────────────────────┘");


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
                    Menu.MenuPrincipal(clienteLogado, clientes);

                   
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
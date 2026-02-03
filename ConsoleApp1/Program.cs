using ConsoleApp1;



class Program
{
    static void Main()
    {


        Cliente cliente = new Cliente
        {
            Id = 1,
            Nome = "Maria"
        };


        Cliente cliente2 = new Cliente
        {
            Id = 2,
            Nome = "Gabriel"
        };


        Conta conta1 = new ContaCorrente
        {
            NumeroDaConta = 1001,
            Saldo = 500,
            Titular = cliente2
        };

        Conta conta2 = new ContaPoupanca
        {
            NumeroDaConta = 2001,
            Saldo = 1000,
            Titular = cliente
        };

        cliente.Contas.Add(conta1);
        cliente.Contas.Add(conta2);








        

        static void Pausar()
        {
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }

        static void MenuPrincipal(Cliente cliente)
        {
            bool executando = true;

            while (executando)
            {
                Console.Clear();
                Console.WriteLine("========= MENU PRINCIPAL =========");
                Console.WriteLine($"Olá {cliente.Nome}");
                Console.WriteLine("1 - Ver contas");

                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Conta contaSelecionada = cliente.VerContas();
                        if (contaSelecionada != null)
                            MenuConta(contaSelecionada);
                        break;

                    case "0":
                        executando = false;
                        break;

                    default:
                        Console.WriteLine("Opção inválida.");
                        Pausar();
                        break;
                }
            }
        }

        static void MenuConta(Conta conta)
        {
            bool dentroDaConta = true;

            while (dentroDaConta)
            {
                Console.Clear();
                Console.WriteLine($"=== CONTA {conta.NumeroDaConta} ({conta.GetType().Name}) ===");
                Console.WriteLine("1 - Ver saldo");
                Console.WriteLine("2 - Depositar");
                Console.WriteLine("3 - Sacar");

                if (conta is ContaPoupanca)
                    Console.WriteLine("4 - Render juros");

                Console.WriteLine("0 - Voltar");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.WriteLine($"Saldo atual: {conta.Saldo}");
                        Pausar();
                        break;

                    case "2":
                        Depositar(conta);
                        break;

                    case "3":
                        Sacar(conta);
                        break;

                    case "4":
                        if (conta is ContaPoupanca cp)
                        {
                            Console.Write("Digite percentual de juros (ex: 0,05 = 5%): ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal juros))
                            {
                                cp.RenderJuros(juros);
                                Console.WriteLine("Juros aplicados!");
                            }
                            else
                            {
                                Console.WriteLine("Valor inválido!");
                            }
                            Pausar();
                        }
                        break;

                    case "0":
                        dentroDaConta = false;
                        break;

                    default:
                        Console.WriteLine("Valor inválido!");
                        Pausar();
                        break;
                }
            }
        }

        static void Depositar(Conta conta)
        {
            Console.Write("Valor do depósito: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                conta.Deposito(valor);
                Console.WriteLine("Depósito realizado!");
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            Pausar();
        }

        static void Sacar(Conta conta)
        {

            Console.Write("Valor do saque: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                if (conta.Saldo < valor)
                {
                    Console.WriteLine("Saldo insuficiente.");
                }
                else
                {
                    conta.Saldo -= 5;
                    conta.Saldo -= valor;
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
            Pausar();
        }

        MenuPrincipal(cliente);

    }
}


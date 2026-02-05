using System;
using System.Collections.Generic; // Necessário para usar List<>

namespace ConsoleApp1
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; } // <--- NOVO: Necessário para o Login

        // A lista já nasce instanciada para evitar erros de "NullReference"
        public List<Conta> Contas { get; set; } = new List<Conta>();

        public void AdicionarConta(Conta conta)
        {
            // Adiciona na lista deste cliente
            Contas.Add(conta);

            // Avisa para a conta que este cliente é o dono dela
            conta.Titular = this;
        }

        // Método opcional para listar na tela (ajuda nos testes)
        public void MostrarContas()
        {
            Console.WriteLine($"\nContas de {Nome}:");
            foreach (var conta in Contas)
            {
                Console.WriteLine($" - {conta.TipoDeConta}: Nº {conta.NumeroDaConta} | Saldo: {conta.Saldo:C}");
            }
        }
    }
}
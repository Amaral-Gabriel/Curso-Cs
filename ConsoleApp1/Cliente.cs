using System;
using System.Collections.Generic; 

namespace ConsoleApp1
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; } 

        
        public List<Conta> Contas { get; set; } = new List<Conta>();

        public void AdicionarConta(Conta conta)
        {
            Contas.Add(conta);

            
            conta.Titular = this;
        }

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
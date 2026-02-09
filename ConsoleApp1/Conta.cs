using System;
using System.Text.Json.Serialization; 

namespace ConsoleApp1
{
    [JsonDerivedType(typeof(ContaCorrente), typeDiscriminator: "corrente")]
    [JsonDerivedType(typeof(ContaPoupanca), typeDiscriminator: "poupanca")]
    public abstract class Conta
    {
        public int NumeroDaConta { get; set; }
        public string TipoDeConta { get; set; }

        
        public decimal Saldo { get; set; }

        [JsonIgnore]
        public Cliente Titular { get; set; }

        public Conta()
        {
            
        }

        public virtual void Saque(decimal valor)
        {
            if (valor > Saldo)
            {
                Console.WriteLine("Saldo insuficiente!");
            }
            else
            {
                Saldo -= valor;
                Console.WriteLine($"Saque de {valor:C} realizado!");
            }
        }

        public void Deposito(decimal valor)
        {
            Saldo += valor;
        }
    }
}
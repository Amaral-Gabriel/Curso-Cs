using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp1
{
    public static class BancoDeDados
    {
        // onde o executável está
        private static string CaminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "banco_dados.json");

        public static void Salvar(List<Cliente> clientes)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(clientes, options);
                File.WriteAllText(CaminhoArquivo, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro crítico ao salvar: {ex.Message}");
                Console.ReadLine();
            }
        }

        public static List<Cliente> Carregar()
        {

            if (!File.Exists(CaminhoArquivo)) return new List<Cliente>();

            try
            {
                string json = File.ReadAllText(CaminhoArquivo);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                    
                };

                List<Cliente> clientes = JsonSerializer.Deserialize<List<Cliente>>(json, options);

                foreach (var cliente in clientes)
                {
                    foreach (var conta in cliente.Contas)
                    {
                        conta.Titular = cliente;
                    }
                }

                return clientes;
            }
            catch
            {
                return new List<Cliente>();
            }
        } 
    
   
    }
}
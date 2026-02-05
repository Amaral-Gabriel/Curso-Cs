using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp1
{
    public static class BancoDeDados
    {
        // Caminho seguro (onde o executável está)
        private static string CaminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "banco_dados.json");

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
                List<Cliente> clientes = JsonSerializer.Deserialize<List<Cliente>>(json);

                // Reconecta os titulares (necessário após carregar do JSON)
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
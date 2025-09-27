using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using LeitorNfse.Models;
using LeitorNfse.Services;

namespace LeitorNfse
{
    class Program
    {
        static void Main(string[] args)
        {
            // Carrega configurações do appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string inputFolder = config["InputFolder"] ?? "input";
            string connectionString = config.GetConnectionString("DefaultConnection");

            // Cria pasta de entrada se não existir
            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine($"Criando pasta de entrada '{inputFolder}'...");
                Directory.CreateDirectory(inputFolder);
                Console.WriteLine("Coloque seus arquivos XML dentro dessa pasta e execute novamente.");
                return;
            }

            // Busca arquivos XML
            var files = Directory.GetFiles(inputFolder, "*.xml");
            if (files.Length == 0)
            {
                Console.WriteLine($"Nenhum arquivo XML encontrado em '{inputFolder}'.");
                return;
            }

            // Processa cada arquivo
            foreach (var file in files)
            {
                Console.WriteLine($"Processando {Path.GetFileName(file)}...");
                try
                {
                    List<NotaFiscal> notas = XmlParser.Parse(file);
                    if (notas == null || notas.Count == 0)
                    {
                        Console.WriteLine("Nenhuma nota extraída deste arquivo.");
                        continue;
                    }

                    DatabaseService.SaveNotas(notas, connectionString);
                    Console.WriteLine($"Inseridas {notas.Count} nota(s) no banco.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar '{file}': {ex.Message}");
                }
            }

            Console.WriteLine("Processamento concluído.");
        }
    }
}

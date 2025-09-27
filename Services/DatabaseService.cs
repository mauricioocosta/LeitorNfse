using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using LeitorNfse.Models;

namespace LeitorNfse.Services
{
    public static class DatabaseService
    {
        public static void SaveNotas(IEnumerable<NotaFiscal> notas, string connectionString)
        {
            const string sql = @"
INSERT INTO NotasFiscais
(Numero, PrestadorCNPJ, TomadorCNPJ, DataEmissao, Descricao, Valor)
VALUES
(@Numero, @PrestadorCNPJ, @TomadorCNPJ, @DataEmissao, @Descricao, @Valor)";

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using var tx = conn.BeginTransaction();

                foreach (var n in notas)
                {
                    conn.Execute(sql, new
                    {
                        Numero = n.Numero,
                        PrestadorCNPJ = n.PrestadorCNPJ,
                        TomadorCNPJ = n.TomadorCNPJ,
                        DataEmissao = n.DataEmissao,
                        Descricao = n.Descricao,
                        Valor = n.Valor
                    }, transaction: tx);
                }

                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar no banco: {ex.Message}");
                throw;
            }
        }
    }
}

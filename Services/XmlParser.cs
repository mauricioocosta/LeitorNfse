using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using LeitorNfse.Models;

namespace LeitorNfse.Services
{
    public static class XmlParser
    {
        public static List<NotaFiscal> Parse(string filePath)
        {
            var doc = XDocument.Load(filePath);

            var nota = new NotaFiscal
            {
                Numero = doc.Descendants("Numero")
                            .FirstOrDefault()?.Value.Trim() ?? string.Empty,

                Prestador = new Prestador
                {
                    CNPJ = doc.Descendants("Prestador")
                              .Descendants("CNPJ")
                              .FirstOrDefault()?.Value.Trim() ?? string.Empty
                },

                Tomador = new Tomador
                {
                    CNPJ = doc.Descendants("Tomador")
                              .Descendants("CNPJ")
                              .FirstOrDefault()?.Value.Trim() ?? string.Empty
                },

                Servico = new Servico
                {
                    Descricao = doc.Descendants("Servico")
                                   .Descendants("Descricao")
                                   .FirstOrDefault()?.Value.Trim() ?? string.Empty
                }
            };

            // Data de emissão
            var dataStr = doc.Descendants("DataEmissao")
                             .FirstOrDefault()?.Value.Trim();
            if (!string.IsNullOrWhiteSpace(dataStr))
            {
                if (DateTime.TryParse(dataStr, out var dt) ||
                    DateTime.TryParseExact(dataStr,
                        new[] { "yyyy-MM-dd", "dd/MM/yyyy" },
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out dt))
                {
                    nota.DataEmissao = dt;
                }
            }

            // Valor total
            var valorStr = doc.Descendants("Servico")
                              .Descendants("Valor")
                              .FirstOrDefault()?.Value.Trim();
            if (!string.IsNullOrWhiteSpace(valorStr) &&
                decimal.TryParse(valorStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
            {
                nota.Servico.Valor = valor;
            }

            return new List<NotaFiscal> { nota };
        }
    }
}
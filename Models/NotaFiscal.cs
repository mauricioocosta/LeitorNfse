using System;

namespace LeitorNfse.Models
{
    public class NotaFiscal
    {
        public string Numero { get; set; }
        public Prestador Prestador { get; set; }
        public Tomador Tomador { get; set; }
        public DateTime DataEmissao { get; set; }
        public Servico Servico { get; set; }

        // Propriedades auxiliares para Dapper
        public string PrestadorCNPJ => Prestador?.CNPJ;
        public string TomadorCNPJ => Tomador?.CNPJ;
        public string Descricao => Servico?.Descricao;
        public decimal Valor => Servico?.Valor ?? 0;
    }

    public class Prestador
    {
        public string CNPJ { get; set; }
    }

    public class Tomador
    {
        public string CNPJ { get; set; }
    }

    public class Servico
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }
}
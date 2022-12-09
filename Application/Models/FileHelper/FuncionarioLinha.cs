using Application.FileHelperConverters;
using FileHelpers;
using System;

namespace Application.Models.FileHelper
{
    [IgnoreFirst(1)]
    [DelimitedRecord(";")]
    public class FuncionarioLinha
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }

        [FieldConverter(typeof(CurrencyConverter))]
        public decimal ValorHora { get; set; }
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy")]
        public DateTime Data { get; set; }
        [FieldConverter(ConverterKind.Date, "HH:mm:ss")]
        public DateTime HoraEntrada { get; set; }
        [FieldConverter(ConverterKind.Date, "HH:mm:ss")]
        public DateTime HoraSaida { get; set; }
        public string Almoco { get; set; }
    }
}

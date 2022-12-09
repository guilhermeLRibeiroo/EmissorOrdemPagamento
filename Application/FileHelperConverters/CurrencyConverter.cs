using FileHelpers;
using System.Globalization;

namespace Application.FileHelperConverters
{
    public class CurrencyConverter
        : ConverterBase
    {
        private NumberFormatInfo nfi = new NumberFormatInfo();

        public CurrencyConverter()
        {
            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencySymbol = "R$";
            nfi.NegativeSign = "-";
        }

        public override object StringToField(string from)
        {
            from = from.Replace(" ", "");
            return decimal.Parse(from, NumberStyles.Currency, nfi);
        }
    }
}

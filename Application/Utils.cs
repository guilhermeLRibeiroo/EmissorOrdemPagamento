using System;
using System.Collections.Generic;

namespace Application
{
    public static class Utils
    {
        public static int ObterNumeroMesPelaString(string month)
        {
            var months = new Dictionary<string, int>
            {
                 { "janeiro",   1 },
                 { "fevereiro", 2 },
                 { "março",     3 },
                 { "abril",     4 },
                 { "maio",      5 },
                 { "junho",     6 },
                 { "julho",     7 },
                 { "agosto",    8 },
                 { "setembro",  9 },
                 { "outubro",   10 },
                 { "novembro",  11 },
                 { "dezembro",  12 }
            };

            if (months.TryGetValue(month, out int result))
                return result;

            throw new Exception("Mês não encontrado.");
        }
    }
}

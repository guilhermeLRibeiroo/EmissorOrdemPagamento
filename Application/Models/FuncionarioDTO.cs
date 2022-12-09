using Newtonsoft.Json;

namespace Application.Models
{
    public class FuncionarioDTO
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public decimal TotalReceber { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasDebito { get; set; }
        public int DiasFalta { get; set; }
        public int DiasExtra { get; set; }
        public int DiasTrabalhados { get; set; }
        [JsonIgnore]
        public decimal ValorHora { get; set; }
    }
}

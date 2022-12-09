using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Models
{
    public class DepartamentoDTO
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public int AnoVigencia { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal TotalDescontos { get; set; }
        public decimal TotalExtras { get; set; }
        public IList<FuncionarioDTO> Funcionarios { get; set; }

        [JsonIgnore]
        public int DiasDoMes { get; set; }

        public DepartamentoDTO()
        {
            Funcionarios = new List<FuncionarioDTO>();
        }
    }
}

using System.Collections.Generic;

namespace Application.Models
{
    public class EmitirDTO
    {
        public List<DepartamentoDTO> Departamentos { get; set; }
        public List<ErroDTO> Erros { get; set; }

        public EmitirDTO()
        {
            Erros = new List<ErroDTO>();
            Departamentos = new List<DepartamentoDTO>();
        }
    }
}

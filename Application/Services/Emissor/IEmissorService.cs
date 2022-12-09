using Application.Models;
using System.Linq;
using System.Web;

namespace Application.Services.Emissor
{
    public interface IEmissorService
    {
        EmitirDTO DepartamentosParaEmissao(ParallelQuery<HttpPostedFileBase> files);
    }
}

using Application.Models;
using Application.Services.Emissor;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class EmissorController
        : Controller
    {
        private IEmissorService _emissorService;

        public EmissorController(IEmissorService emissorService)
        {
            _emissorService = emissorService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Resultado()
        {
            var resultado = Session["Resultado"];

            if (resultado == null)
                return View();

            ViewBag.DepartamentosJSON = JsonConvert.SerializeObject(((EmitirDTO)resultado).Departamentos);
            ViewBag.Erros = ((EmitirDTO)resultado).Erros.Where(err => err != null).ToList();
            return View();
        }

        [HttpPost]
        public void Emitir()
        {
            var files = Enumerable.Range(0, Request.Files.Count)
                           .Select(i => Request.Files[i])
                           .AsParallel();

            var resultado = _emissorService.DepartamentosParaEmissao(files);

            Session["Resultado"] = resultado;
        }

        [HttpGet]
        public FileResult DownloadResultado(string json)
        {
            if(string.IsNullOrEmpty(json))
                return null;

            var nomeArquivo = "emissao.json";

            byte[] bytes = Encoding.UTF8.GetBytes(json);
            var content = new MemoryStream(bytes);
            return File(content, "application/json", nomeArquivo);
        }
    }
}
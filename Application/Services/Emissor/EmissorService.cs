using Application.Models;
using Application.Models.FileHelper;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Application.Services.Emissor
{
    public class EmissorService
        : IEmissorService
    {
        public static object _lock = new object();

        public EmitirDTO DepartamentosParaEmissao(ParallelQuery<HttpPostedFileBase> files)
        {
            var result = new EmitirDTO();

            var listaTasks = new Task[files.Count()];

            var index = 0;
            foreach (var file in files)
            {
                listaTasks[index] = (Task.Run(() => PreProcessamentoDepartamento(file)).ContinueWith(_departamento =>
                {
                    lock (_lock)
                    {
                        if (_departamento.Exception != null)
                        {
                            result.Erros.Add(new ErroDTO
                            {
                                NomeArquivo = file.FileName,
                                Mensagem = "Ocorreu um erro ao processar dados."
                            });
                            
                            return;
                        }

                        result.Erros.AddRange(_departamento.Result.Erros.Where(err => err != null));

                        var departamento = _departamento.Result.Departamentos.FirstOrDefault();

                        if (departamento != null)
                            result.Departamentos.Add(departamento);
                    }
                }));

                index++;
            }

            Task.WaitAll(listaTasks);

            return result;
        }

        private EmitirDTO PreProcessamentoDepartamento(HttpPostedFileBase file)
        {
            EmitirDTO resultado = new EmitirDTO();

            var engine = new FileHelperEngine<FuncionarioLinha>();
            List<FuncionarioLinha> result = null;

            try
            {
                result = engine.ReadStreamAsList(new StreamReader(file.InputStream), maxRecords: 100_000);
            }
            catch (ConvertException e)
            {
                resultado.Erros.Add(new ErroDTO
                {
                    NomeArquivo = file.FileName,
                    Mensagem = $"Formato da linha {e.LineNumber} está fora do padrão."
                });

                return resultado;
            }
            catch (Exception)
            {
                resultado.Erros.Add(new ErroDTO
                {
                    NomeArquivo = file.FileName,
                    Mensagem = $"Formato de uma linha do arquivo está fora do padrão."
                });

                return resultado;
            }

            var departamento = new DepartamentoDTO();

            var erro = DefinirNomeEMesAnoVigenciaDepartamento(ref departamento, file.FileName);

            if (erro != null)
            {
                resultado.Erros.Add(erro);
                return resultado;
            }

            foreach (var linha in result)
            {
                FuncionarioDTO funcionario = departamento.Funcionarios.FirstOrDefault(f => f.Codigo == linha.Codigo);
                int indexFuncionario = departamento.Funcionarios.IndexOf(funcionario);

                if (funcionario == null)
                {
                    funcionario = new FuncionarioDTO
                    {
                        Codigo = linha.Codigo,
                        Nome = linha.Nome,
                        ValorHora = linha.ValorHora
                    };
                }

                var inicioFimAlmoco = linha.Almoco.Replace(" ", "").Split('-');

                DateTime inicioAlmoco = DateTime.Parse(inicioFimAlmoco[0]);
                DateTime fimAlmoco = DateTime.Parse(inicioFimAlmoco[1]);

                var horasTrabalho = (linha.HoraSaida - (fimAlmoco - inicioAlmoco) - linha.HoraEntrada).Hours;

                if (horasTrabalho < 8)
                {
                    funcionario.HorasDebito += 8 - horasTrabalho;
                    departamento.TotalDescontos += (8 - horasTrabalho) * funcionario.ValorHora;
                }

                else if (horasTrabalho > 8)
                {
                    funcionario.HorasExtras += horasTrabalho - 8;
                    departamento.TotalExtras += (horasTrabalho - 8) * funcionario.ValorHora;
                }

                switch (linha.Data.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        funcionario.DiasExtra += 1;
                        departamento.TotalExtras += funcionario.ValorHora * 8;
                        break;
                }

                funcionario.DiasTrabalhados += 1;
                funcionario.DiasFalta = departamento.DiasDoMes - (funcionario.DiasTrabalhados - funcionario.DiasExtra);
                funcionario.TotalReceber = (funcionario.ValorHora * ((funcionario.DiasTrabalhados * 8) + funcionario.HorasExtras - funcionario.HorasDebito));

                if (indexFuncionario != -1)
                    departamento.Funcionarios[indexFuncionario] = funcionario;
                else
                    departamento.Funcionarios.Add(funcionario);
            }

            for (int i = 0; i < departamento.Funcionarios.Count(); i++)
            {
                var _funcionario = departamento.Funcionarios[i];

                departamento.TotalDescontos += _funcionario.ValorHora * (_funcionario.DiasFalta * 8);
                departamento.TotalPagar += _funcionario.TotalReceber - (_funcionario.ValorHora * (_funcionario.DiasFalta * 8));
            }

            resultado.Departamentos.Add(departamento);

            return resultado;
        }

        private ErroDTO DefinirNomeEMesAnoVigenciaDepartamento(ref DepartamentoDTO departamento, string fileName)
        {
            try
            {
                var fileNameInfo = fileName.Split('-');

                var mesVigencia = fileNameInfo[fileNameInfo.Length - 2];
                var numeroMesVigencia = Utils.ObterNumeroMesPelaString(mesVigencia.ToLower());
                var anoVigencia = Convert.ToInt32(fileNameInfo[fileNameInfo.Length - 1].Replace(".csv", ""));

                var departamentoNome = fileNameInfo[fileNameInfo.Length - 3];
                departamentoNome = departamentoNome.Substring(departamentoNome.LastIndexOf('/') + 1);

                departamento = new DepartamentoDTO
                {
                    AnoVigencia = anoVigencia,
                    MesVigencia = mesVigencia,
                    Departamento = departamentoNome
                };

                departamento.DiasDoMes = Enumerable.Range(1, DateTime.DaysInMonth(departamento.AnoVigencia, numeroMesVigencia))
                                             .Select(dia => new DateTime(anoVigencia, numeroMesVigencia, dia))
                                             .Count(dia => dia.DayOfWeek != DayOfWeek.Sunday
                                                        && dia.DayOfWeek != DayOfWeek.Saturday);
            }
            catch (Exception e)
            {
                return new ErroDTO
                {
                    NomeArquivo = fileName,
                    Mensagem = "Nome do arquivo não está padronizado."
                };
            }

            return null;
        }
    }
}

using ApiFinancas.Enums;
using ApiFinancas.Models;
using ApiFinancas.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiFinancas.Controllers
{
    /// <summary>
    /// Controller de ArquivosCliente
    /// </summary>
    [Route("Investimento")]
    public class InvestimentoController : ControllerBase
    {

        string conexaoBanco = System.Environment.GetEnvironmentVariable("BdFutturo");

        ILogger<InvestimentoController> log;

        public InvestimentoController(ILogger<InvestimentoController> logger)
        {
            this.log = logger;
        }

        /// <summary>
        /// Obt�m a lista de Fabricantes
        /// </summary>
        /// <param name="sistema"></param>
        /// <param name="token"></param>
        /// <param name="filtros"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1")]
        public ActionResult CriarInvestimento([FromHeader] string token, [FromBody] InvestimentoModel investimento)
        {

            RetornoEntidadeModel<InvestimentoModel> retornoModel = new RetornoEntidadeModel<InvestimentoModel>();
            try
            {
                string mensagemErro = "";
                int idUsuario = 0;
                TipoContaEnum tipoUsuario = TipoContaEnum.UsuarioComum;
                bool tokenValido = UsuarioService.ValidarToken(token, ref idUsuario, ref tipoUsuario, ref mensagemErro);
                if (!tokenValido)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = mensagemErro;
                    return StatusCode(403, retornoModel);
                }

                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    investimento.IdUsuario = idUsuario;
                }
                

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string insertQuery = " INSERT INTO [Main].[Investimentos]([IdUsuario],[Nome],[Vencimento],[DataInclusao],[DataAlteracao],[Versao],[VoExcluido],[Categoria],[Risco])" +
                                         " VALUES (@IdUsuario,@Nome,@Vencimento,GETDATE(),GETDATE(),1,0,@Categoria,@Risco);" +
                                         " SELECT CAST(SCOPE_IDENTITY() as int);";


                    var retornoBd = contexto.QuerySingle<int>(insertQuery, investimento);
                    investimento.Id = retornoBd;
                    retornoModel.Mensagem = $"Investimento inclu�do com sucesso.";
                    retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                    retornoModel.Entidade = investimento;
                };

                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na inclus�o do investimento: {ex.Message}.";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarInvestimento([FromHeader] string token, [FromBody] InvestimentoModel investimento, [FromRoute] int id)
        {

            RetornoBaseModel retornoModel = new RetornoBaseModel();
            try
            {
                string mensagemErro = "";
                int idUsuario = 0;
                TipoContaEnum tipoUsuario = TipoContaEnum.UsuarioComum;
                bool tokenValido = UsuarioService.ValidarToken(token, ref idUsuario, ref tipoUsuario, ref mensagemErro);
                if (!tokenValido)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = mensagemErro;
                    return StatusCode(403, retornoModel);
                }

                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    investimento.IdUsuario = idUsuario;
                }


                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    investimento.Id = id;
                    string insertQuery =" UPDATE [Main].[Investimentos] " +
                                        " SET [Nome] = @Nome, " +
                                        " [Vencimento] = @Vencimento, " +
                                        " [DataAlteracao] = GETDATE()," +
                                        " [Versao] = Versao+1," +
                                        " [Categoria] = @Categoria," +
                                        " [Risco] = @Risco " +
                                        " WHERE Id = @Id AND VoExcluido=0 AND [IdUsuario] = @IdUsuario";

                    var retorno = contexto.Execute(insertQuery, investimento);
                };

                retornoModel.Mensagem = "Investimento alterado com sucesso.";
                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na alterac�o do investimento: {ex.Message}.";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterInvestimento([FromHeader] string token, [FromRoute] int id)
        {

            RetornoEntidadeModel<InvestimentoModel> retornoModel = new RetornoEntidadeModel<InvestimentoModel>();
            try
            {
                string mensagemErro = "";
                int idUsuario = 0;
                TipoContaEnum tipoUsuario = TipoContaEnum.UsuarioComum;
                bool tokenValido = UsuarioService.ValidarToken(token, ref idUsuario, ref tipoUsuario, ref mensagemErro);
                if (!tokenValido)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = mensagemErro;
                    return StatusCode(403, retornoModel);
                }

                string clausulaExtra = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaExtra = " AND IdUsuario = @IdUsuario ";
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var investimento = new { Id = id };
                    string insertQuery = " Select * FROM [Main].[Investimentos] " +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaExtra}";

                    var retornoBd = contexto.Query<InvestimentoModel>(insertQuery, investimento).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Usu�rio n�o localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Usu�rio localizado com sucesso";
                        retornoModel.Entidade = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na obten��o do investimento: {ex.Message}.";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
            
            
        }

        [HttpGet]
        [Route("v1")]
        public ActionResult ObterInvestimentos([FromHeader] string token)
        {

            RetornoListaEntidadesModel<InvestimentoModel> retornoModel = new RetornoListaEntidadesModel<InvestimentoModel>();

            try
            {
                string mensagemErro = "";
                int idUsuario = 0;
                TipoContaEnum tipoUsuario = TipoContaEnum.UsuarioComum;
                bool tokenValido = UsuarioService.ValidarToken(token, ref idUsuario, ref tipoUsuario, ref mensagemErro);
                if (!tokenValido)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = mensagemErro;
                    return StatusCode(403, retornoModel);
                }

                string clausulaExtra = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaExtra = " AND IdUsuario = @IdUsuario ";
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = " SELECT " +
                                         "     Investimentos.Id, Investimentos.Nome, ROUND(SUM(ISNULL(MovimentacoesInvestimentos.Valor,0)),2) AS Valor" +
                                         " FROM " +
                                         "    [Main].[Investimentos] WITH (NOLOCK)" +
                                         "    LEFT JOIN [Main].[MovimentacoesInvestimentos] WITH (NOLOCK) ON (Investimentos.Id = MovimentacoesInvestimentos.IdInvestimento AND MovimentacoesInvestimentos.VoExcluido = 0)" +
                                         " WHERE " +
                                         "    Investimentos.VoExcluido=0 AND Investimentos.IdUsuario = @IdUsuario " +
                                         " GROUP BY" +
                                         "    Investimentos.Id, Investimentos.Nome" +
                                         " ORDER BY " +
                                         "    Nome";


                    var model = new { IdUsuario = idUsuario };
                    var retornoBd = contexto.Query<InvestimentoModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Mensagem = "Nenhum investimento localizado.";
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        return StatusCode(400,retornoModel);
                    }
                    else
                    {
                        retornoModel.Mensagem = "Investimentos localizados com sucesso.";
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na obten��o dos investimentos: {ex.Message}.";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
           

        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirInvestimento([FromHeader] string token, [FromRoute] int id)
        {

            RetornoBaseModel retornoModel = new RetornoBaseModel();
            try
            {
                string mensagemErro = "";
                int idUsuario = 0;
                TipoContaEnum tipoUsuario = TipoContaEnum.UsuarioComum;
                bool tokenValido = UsuarioService.ValidarToken(token, ref idUsuario, ref tipoUsuario, ref mensagemErro);
                if (!tokenValido)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = mensagemErro;
                    return StatusCode(403, retornoModel);
                }

                string clausulaExtra = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaExtra = " AND IdUsuario = @IdUsuario ";
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var investimento = new { Id = id };
                    string insertQuery = " UPDATE [Main].[Investimentos] " +
                                         " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaExtra}";

                    var retorno = contexto.Execute(insertQuery, investimento);

                    if (retorno == null || retorno == 0)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Investimento n�o localizado";
                        return StatusCode(400,retornoModel);
                    }
                    else
                    {
                        retornoModel.Mensagem = "Investimento exlclu�do com sucesso.";
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na exclus�o do investimento: {ex.Message}.";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }
    }

}

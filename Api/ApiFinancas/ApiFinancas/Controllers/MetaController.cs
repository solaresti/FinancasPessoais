using ApiFinancas.Models;
using ApiFinancas.Services;
using ApiFinancas.Enums;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiFinancas.Controllers
{
    /// <summary>
    /// Controller de Metas
    /// </summary>
    [Route("Meta")]
    public class MetaController : ControllerBase
    {

        string conexaoBanco = System.Environment.GetEnvironmentVariable("BdFutturo");

        ILogger<MetaController> log;

        public MetaController(ILogger<MetaController> logger)
        {
            this.log = logger;
        }

        /// <summary>
        /// Obtém a lista de Fabricantes
        /// </summary>
        /// <param name="sistema"></param>
        /// <param name="token"></param>
        /// <param name="filtros"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1")]
        public ActionResult CriarMeta([FromHeader] string token, [FromBody] MetaModel metaModel)
        {
            RetornoEntidadeModel<MetaModel> retornoModel = new RetornoEntidadeModel<MetaModel>();

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

                if (metaModel.IdUsuario != idUsuario && tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    metaModel.DataInclusao = DateTime.Now;
                    metaModel.DataAlteracao = metaModel.DataInclusao;
                    metaModel.Versao = 1;
                    metaModel.VoExcluido = false;

                    string insertQuery = " INSERT INTO Main.Metas (IdUsuario, Nome, Descritivo, Valor, Data, DataInclusao, DataAlteracao, Versao, VoExcluido) " +
                                         " VALUES  (@IdUsuario, @Nome, @Descritivo, @Valor, @Data, GETDATE(), GETDATE(), 1,0); " +
                                         " SELECT CAST(SCOPE_IDENTITY() as int);";


                    var retornoBd = contexto.QuerySingle<int>(insertQuery, metaModel);
                    metaModel.Id = retornoBd;
                };

                retornoModel.Entidade = metaModel;
                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Meta incluída com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na inclusão da meta: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarMeta([FromHeader] string token, [FromBody] MetaModel metaModel, [FromRoute] int id)
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

                string clausulaAdicional = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaAdicional = " AND IdUsuario = @IdUsuario";
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    metaModel.Id = id;
                    string insertQuery = "UPDATE [Main].[Metas] " +
                                        " SET [Nome] = @Nome, " +
                                        " [Descritivo] = @Descritivo, " +
                                        " [Valor] = @Valor, " +
                                        " [Data] = @Data, " +
                                        " [DataAlteracao] = GETDATE()," +
                                        " [Versao] = Versao+1 " +
                                        $" WHERE Id = @Id AND VoExcluido=0  {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, metaModel);
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Meta atualizada com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na alteração da meta: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterMeta([FromHeader] string token, [FromRoute] int id)
        {
            RetornoEntidadeModel<MetaModel> retornoModel = new RetornoEntidadeModel<MetaModel>();
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

                string clausulaAdicional = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaAdicional = " AND IdUsuario = @IdUsuario";
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var usuarioModel = new { Id = id, IdUsuario = idUsuario };
                    string insertQuery = " Select * FROM [Main].[Metas] " +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retornoBd = contexto.Query<MetaModel>(insertQuery, usuarioModel).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Meta não localizada";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Meta obtida com sucesso";
                        retornoModel.Entidade = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção da meta: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }


        [HttpGet]
        [Route("v1")]
        public ActionResult ObterMetas([FromHeader] string token)
        {

            RetornoListaEntidadesModel<MetaModel> retornoModel = new RetornoListaEntidadesModel<MetaModel>();
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

                string clausulaAdicional = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaAdicional = " AND IdUsuario = @IdUsuario";
                }


                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = $" Select * FROM [Main].[Metas] WHERE VoExcluido=0 {clausulaAdicional} ORDER BY Nome";

                    var model = new { idUsuario = idUsuario };
                    var retornoBd = contexto.Query<MetaModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Metas obtidas com sucesso";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção da meta: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirMeta([FromHeader] string token, [FromRoute] int id)
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

                string clausulaAdicional = "";
                if (tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    clausulaAdicional = " AND IdUsuario = @IdUsuario";
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var model = new { Id = id, IdUsuario = idUsuario };
                    string insertQuery = " UPDATE [Main].[Metas] " +
                                         " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, model);

                    if (retorno == null || retorno == 0)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Meta não localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Meta excluída com sucesso";
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na exclusão da meta: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }
    }

}

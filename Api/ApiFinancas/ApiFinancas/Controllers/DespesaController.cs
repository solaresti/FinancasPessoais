using ApiFinancas.Models;
using ApiFinancas.Services;
using ApiFinancas.Enums;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiFinancas.Controllers
{
    /// <summary>
    /// Controller de Despesa
    /// </summary>
    [Route("Despesa")]
    public class DespesaController : ControllerBase
    {

        string conexaoBanco = System.Environment.GetEnvironmentVariable("BdFutturo");

        ILogger<DespesaController> log;

        public DespesaController(ILogger<DespesaController> logger)
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
        public ActionResult CriarDespesa([FromHeader] string token, [FromBody] DespesaModel despesa)
        {
            RetornoEntidadeModel<DespesaModel> retornoModel = new RetornoEntidadeModel<DespesaModel>();

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


                if (despesa.IdUsuario != idUsuario && tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    despesa.DataInclusao = DateTime.Now;
                    despesa.DataAlteracao = despesa.DataInclusao;
                    despesa.Versao = 1;
                    despesa.VoExcluido = false;

                    string insertQuery = " INSERT INTO Main.Despesas (IdUsuario, IdCategoria, Descritivo, Valor, Data, DataInclusao, DataAlteracao, Versao, VoExcluido) " +
                                         " VALUES  (@IdUsuario, @IdCategoria, @Descritivo, @Valor, @Data, GETDATE(), GETDATE(), 1,0); " +
                                         " SELECT CAST(SCOPE_IDENTITY() as int);";


                    var retornoBd = contexto.QuerySingle<int>(insertQuery, despesa);
                    despesa.Id = retornoBd;
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Despesa incluída com sucesso";
                retornoModel.Entidade = despesa;
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na inclusão de despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarDespesa([FromHeader] string token, [FromBody] DespesaModel despesa, [FromRoute] int id)
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
                    despesa.Id = id;
                    string insertQuery = "UPDATE [Main].[Despesas] " +
                                        " SET [IdCategoria] = @IdCategoria, " +
                                        " [Descritivo] = @Descritivo, " +
                                        " [Valor] = @Valor, " +
                                        " [Data] = @Data, " +
                                        " [DataAlteracao] = GETDATE()," +
                                        " [Versao] = Versao+1 " +
                                        $" WHERE Id = @Id AND VoExcluido=0  {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, despesa);
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Despesa atualizada com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na alteração da despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterDespesa([FromHeader] string token, [FromRoute] int id)
        {
            RetornoEntidadeModel<DespesaModel> retornoModel = new RetornoEntidadeModel<DespesaModel>();
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
                    string insertQuery = " Select * FROM [Main].[Despesas] " +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retornoBd = contexto.Query<DespesaModel>(insertQuery, usuarioModel).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Despesa não localizada";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Despesa obtida com sucesso";
                        retornoModel.Entidade = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção da despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }


        [HttpGet]
        [Route("v1")]
        public ActionResult ObterDespesas([FromHeader] string token)
        {

            RetornoListaEntidadesModel<DespesaModel> retornoModel = new RetornoListaEntidadesModel<DespesaModel>();
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
                    string selectQuery = $" Select * FROM [Main].[Despesas] WHERE VoExcluido=0 {clausulaAdicional} ORDER BY idCategoria";

                    var model = new { idUsuario = idUsuario };
                    var retornoBd = contexto.Query<DespesaModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Despesas obtidas com sucesso";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção de despesas: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirDespesa([FromHeader] string token, [FromRoute] int id)
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
                    string insertQuery = " UPDATE [Main].[Despesas] " +
                                         " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, model);

                    if (retorno == null || retorno == 0)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Despesa não localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Despesa excluída com sucesso";
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na exclusão da despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }
    }

}

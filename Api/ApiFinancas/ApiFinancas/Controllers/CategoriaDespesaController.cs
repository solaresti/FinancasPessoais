using ApiFinancas.Models;
using ApiFinancas.Services;
using ApiFinancas.Enums;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiFinancas.Controllers
{
    /// <summary>
    /// Controller de CategoriaDespesa
    /// </summary>
    [Route("CategoriaDespesa")]
    public class CategoriaDespesaController : ControllerBase
    {

        string conexaoBanco = System.Environment.GetEnvironmentVariable("BdFutturo");

        ILogger<CategoriaDespesaController> log;

        public CategoriaDespesaController(ILogger<CategoriaDespesaController> logger)
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
        public ActionResult CriarCategoriaDespesa([FromHeader] string token, [FromBody] CategoriaDespesaModel categoriaDespesa)
        {
            RetornoEntidadeModel<CategoriaDespesaModel> retornoModel = new RetornoEntidadeModel<CategoriaDespesaModel>();

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

                // pensando em permitir para o administrador também
                if (categoriaDespesa.IdUsuario != idUsuario)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    categoriaDespesa.DataInclusao = DateTime.Now;
                    categoriaDespesa.DataAlteracao = categoriaDespesa.DataInclusao;
                    categoriaDespesa.Versao = 1;
                    categoriaDespesa.VoExcluido = false;

                    string insertQuery = " INSERT INTO Main.CategoriasDespesas (IdUsuario, Nome, Descritivo, MetaValorMensal, DataInclusao, DataAlteracao, Versao, VoExcluido) " +
                                         " VALUES  (@IdUsuario, @Nome, @Descritivo, @MetaValorMensal, GETDATE(), GETDATE(), 1,0); " +
                                         " SELECT CAST(SCOPE_IDENTITY() as int);";


                    var retornoBd = contexto.QuerySingle<int>(insertQuery, categoriaDespesa);
                    categoriaDespesa.Id = retornoBd;
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Categoria de despesa incluída com sucesso";
                retornoModel.Entidade = categoriaDespesa;
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na inclusão da categoria de despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarCategoriaDespesa([FromHeader] string token, [FromBody] CategoriaDespesaModel categoriaDespesa, [FromRoute] int id)
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
                    categoriaDespesa.Id = id;
                    string insertQuery = "UPDATE [Main].[CategoriasDespesas] " +
                                        " SET [Nome] = @Nome, " +
                                        " [Descritivo] = @Descritivo, " +
                                        " [MetaValorMensal] = @MetaValorMensal, " +
                                        " [DataAlteracao] = GETDATE()," +
                                        " [Versao] = Versao+1 " +
                                        $" WHERE Id = @Id AND VoExcluido=0  {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, categoriaDespesa);
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Categoria de despesa atualizada com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na alteração de categoria de despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterCategoriaDespesa([FromHeader] string token, [FromRoute] int id)
        {
            RetornoEntidadeModel<CategoriaDespesaModel> retornoModel = new RetornoEntidadeModel<CategoriaDespesaModel>();
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
                    string insertQuery = " Select * FROM [Main].[CategoriasDespesas] " +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retornoBd = contexto.Query<CategoriaDespesaModel>(insertQuery, usuarioModel).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Categoria de despesa não localizada";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Categoria de despesa obtida com sucesso";
                        retornoModel.Entidade = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção de categoria de despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }


        [HttpGet]
        [Route("v1")]
        public ActionResult ObterCategoriasDespesas([FromHeader] string token)
        {

            RetornoListaEntidadesModel<CategoriaDespesaModel> retornoModel = new RetornoListaEntidadesModel<CategoriaDespesaModel>();
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
                    string selectQuery = $" Select * FROM [Main].[CategoriasDespesas] WHERE VoExcluido=0 {clausulaAdicional} ORDER BY Nome";

                    var model = new { idUsuario = idUsuario };
                    var retornoBd = contexto.Query<CategoriaDespesaModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Categorias de despesa obtidas com sucesso";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção de categorias de despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirCategoriaDespesa([FromHeader] string token, [FromRoute] int id)
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
                    string insertQuery = " UPDATE [Main].[CategoriasDespesas] " +
                                         " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, model);

                    if (retorno == null || retorno == 0)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Categoria de despesa não localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Categoria de despesa excluída com sucesso";
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na exclusão da categoria de despesa: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }
    }

}

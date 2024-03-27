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
    [Route("MovimentacaoInvestimento")]
    public class MovimentacaoInvestimentoController : ControllerBase
    {

        string conexaoBanco = System.Environment.GetEnvironmentVariable("BdFutturo");

        ILogger<MovimentacaoInvestimentoController> log;

        public MovimentacaoInvestimentoController(ILogger<MovimentacaoInvestimentoController> logger)
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
        public ActionResult CriarMovimentacao([FromHeader] string token, [FromBody] MovimentacaoInvestimentoModel movimentacaoModel)
        {
            RetornoEntidadeModel<MovimentacaoInvestimentoModel> retornoModel = new RetornoEntidadeModel<MovimentacaoInvestimentoModel>();

            if (!ModelState.IsValid)
            {
                // Obtém todas as mensagens de erro
                //List<string> mensagensDeErro = ModelState.Values
                //    .SelectMany(v => v.Errors)
                //    .Select(e => e.ErrorMessage)
                //    .ToList();

                retornoModel.Codigo = CodigosRetornoEnum.DadosInvalidos;
                retornoModel.Mensagem = $"Dados inválidos.";
                return StatusCode(400, retornoModel);

            }

            if (movimentacaoModel==null)
            {
                retornoModel.Codigo = CodigosRetornoEnum.PayloadInvalido;
                retornoModel.Mensagem = "Dados inválidos (1F1A4CBD).";
                return StatusCode(400, retornoModel);
            }
            else if (movimentacaoModel.Data > DateTime.Today)
            {
                retornoModel.Codigo = CodigosRetornoEnum.DadosInvalidos;
                retornoModel.Mensagem = $"Você não pode lançar uma movimentação futura. (0B9D4CC2)";
                return StatusCode(400, retornoModel);
            }
            else if (movimentacaoModel.Data < DateTime.Today.AddDays(-730))
            {
                retornoModel.Codigo = CodigosRetornoEnum.DadosInvalidos;
                retornoModel.Mensagem = $"Você não pode lançar uma movimentação anterior a {DateTime.Today.AddDays(-730).ToString("dd/MM/yyyy")} .";
                return StatusCode(400, retornoModel);
            }
 


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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    movimentacaoModel.IdUsuario = idUsuario;
                    movimentacaoModel.DataInclusao = DateTime.Now;
                    movimentacaoModel.DataAlteracao = movimentacaoModel.DataInclusao;
                    movimentacaoModel.Versao = 1;
                    movimentacaoModel.VoExcluido = false;

                    string insertQuery = " INSERT INTO Main.MovimentacoesInvestimentos (IdUsuario, IdMeta, IdInvestimento, Descritivo, Valor, Data, DataInclusao, DataAlteracao, Versao, VoExcluido) " +
                                         " VALUES  (@IdUsuario, @IdMeta, @IdInvestimento, @Descritivo, @Valor, @Data, GETDATE(), GETDATE(), 1,0); " +
                                         " SELECT CAST(SCOPE_IDENTITY() as int);";

                    
                    var retornoBd = contexto.QuerySingle<int>(insertQuery, movimentacaoModel);
                    movimentacaoModel.Id = retornoBd;
                };


                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Movimentação incluída com sucesso";
                retornoModel.Entidade = movimentacaoModel;
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na inclusão da movimentação (E8275746): {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarMovimentacao([FromHeader] string token, [FromBody] MovimentacaoInvestimentoModel movimentacaoModel, [FromRoute] int id)
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
                    movimentacaoModel.Id = id;
                    string insertQuery = "UPDATE [Main].[MovimentacoesInvestimentos] " +
                                        " SET [Valor] = @Valor, " +
                                        " [Data] = @Data, " +
                                        " [Descritivo] = @Descritivo, " +
                                        " [IdMeta] = @IdMeta, " +
                                        " [IdInvestimento] = @IdInvestimento, " +
                                        " [DataAlteracao] = GETDATE()," +
                                        " [Versao] = Versao+1 " +
                                        $" WHERE Id = @Id AND VoExcluido=0  {clausulaAdicional}";

                    var retorno = contexto.Execute(insertQuery, movimentacaoModel);
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Movimentação atualizada com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na alteração da movimentação: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterMovimentacao([FromHeader] string token, [FromRoute] int id)
        {
            RetornoEntidadeModel<MovimentacaoInvestimentoModel> retornoModel = new RetornoEntidadeModel<MovimentacaoInvestimentoModel>();
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
                    var movimentacaoModel = new { Id = id, IdUsuario = idUsuario };
                    string insertQuery = " Select * FROM [Main].[MovimentacoesInvestimentos] " +
                                         $" WHERE Id = @Id AND VoExcluido=0 {clausulaAdicional}";

                    var retornoBd = contexto.Query<MovimentacaoInvestimentoModel>(insertQuery, movimentacaoModel).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Movimentação não localizada";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Movimentação obtida com sucesso";
                        retornoModel.Entidade = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção da movimentação: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }


        [HttpGet]
        [Route("v1")]
        public ActionResult ObterMovimentacoes([FromHeader] string token)
        {

            RetornoListaEntidadesModel<MovimentacaoInvestimentoModel> retornoModel = new RetornoListaEntidadesModel<MovimentacaoInvestimentoModel>();

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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = $" Select * FROM [Main].[MovimentacoesInvestimentos] WHERE VoExcluido=0 AND IdUsuario = @IdUsuario ORDER BY Data";

                    var model = new { idUsuario = idUsuario };
                    var retornoBd = contexto.Query<MovimentacaoInvestimentoModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Movimentações obtidas com sucesso";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção da movimentação: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirMovimentacao([FromHeader] string token, [FromRoute] int id)
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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var model = new { Id = id, IdUsuario = idUsuario };
                    string insertQuery = " UPDATE [Main].[MovimentacoesInvestimentos] " +
                                         " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                         $" WHERE Id = @Id AND VoExcluido=0 AND IdUsuario = @IdUsuario";

                    var retorno = contexto.Execute(insertQuery, model);

                    if (retorno == null || retorno == 0)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Movimentação não localizada";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Movimentação excluída com sucesso";
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na exclusão da movimentação: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }


        [HttpGet]
        [Route("v1/ExtratoMeta/{idMeta}")]
        public ActionResult ObterExtratoPorMeta([FromHeader] string token, [FromRoute] int idMeta)
        {

            RetornoListaEntidadesModel<ExtratoModel> retornoModel = new RetornoListaEntidadesModel<ExtratoModel>();

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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = " SELECT " +
                                         " [MovimentacoesInvestimentos].[Id],"+
                                         " [MovimentacoesInvestimentos].[Data]," +
                                         " [MovimentacoesInvestimentos].[Descritivo]," +
                                         " [MovimentacoesInvestimentos].[Valor]," +
                                         " [MovimentacoesInvestimentos].[IdInvestimento]," +
                                         " [Investimentos].[Nome] " +
                                         " FROM [Main].[MovimentacoesInvestimentos] WITH (NOLOCK) " +
                                         " JOIN [Main].[Investimentos] WITH (NOLOCK) ON (MovimentacoesInvestimentos.IdInvestimento = Investimentos.Id)" +
                                         " WHERE [MovimentacoesInvestimentos].[IdUsuario] = @IdUsuario " +
                                         " And [MovimentacoesInvestimentos].[IdMeta] = @IdMeta " +
                                         " And [MovimentacoesInvestimentos].[VoExcluido] = 0 "+
                                         " ORDER BY" +
                                         " Data";

                    var model = new { IdUsuario = idUsuario, IdMeta = idMeta };
                    var retornoBd = contexto.Query<ExtratoModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Extrato obtido com sucesso.";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção do extrato (C64C0C8E): {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpGet]
        [Route("v1/PosicaoMeta/{idMeta}")]
        public ActionResult ObterPosicaoMetaPorInvestimento([FromHeader] string token, [FromRoute] int idMeta)
        {

            RetornoListaEntidadesModel<PosicaoModel> retornoModel = new RetornoListaEntidadesModel<PosicaoModel>();

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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = " SELECT " +
                                         " SUM([MovimentacoesInvestimentos].[Valor]) As Valor," +
                                         " [MovimentacoesInvestimentos].[IdInvestimento]," +
                                         " [Investimentos].[Nome]" +
                                         " FROM " +
                                         " [Main].[MovimentacoesInvestimentos] WITH (NOLOCK)" +
                                         " JOIN [Main].[Investimentos] WITH (NOLOCK) ON (MovimentacoesInvestimentos.IdInvestimento = Investimentos.Id)" +
                                         " WHERE [MovimentacoesInvestimentos].[IdUsuario] = @IdUsuario " +
                                         " And [MovimentacoesInvestimentos].[IdMeta] = @IdMeta " +
                                         " And [MovimentacoesInvestimentos].[VoExcluido] = 0 " +
                                         " GROUP BY " +
                                         " [MovimentacoesInvestimentos].[IdInvestimento]," +
                                         " [Investimentos].[Nome]" +
                                         " ORDER BY" +
                                         " [Investimentos].[Nome]";

                    var model = new { IdUsuario = idUsuario, IdMeta = idMeta };
                    var retornoBd = contexto.Query<PosicaoModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Extrato obtido com sucesso.";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção do extrato (C64C0C8E): {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpGet]
        [Route("v1/ExtratoInvestimento/{idInvestimento}")]
        public ActionResult ObterExtratoPorInvestimento([FromHeader] string token, [FromRoute] int idInvestimento)
        {

            RetornoListaEntidadesModel<ExtratoModel> retornoModel = new RetornoListaEntidadesModel<ExtratoModel>();

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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = " SELECT " +
                                         " [MovimentacoesInvestimentos].[Data]," +
                                         " [MovimentacoesInvestimentos].[Descritivo]," +
                                         " [MovimentacoesInvestimentos].[Valor]," +
                                         " [MovimentacoesInvestimentos].[IdMeta]," +
                                         " [Metas].[Nome] " +
                                         " FROM [Main].[MovimentacoesInvestimentos] WITH (NOLOCK) " +
                                         " JOIN [Main].[Metas] WITH (NOLOCK) ON (MovimentacoesInvestimentos.IdMeta = Metas.Id)" +
                                         " WHERE [MovimentacoesInvestimentos].[IdUsuario] = @IdUsuario " +
                                         " And [MovimentacoesInvestimentos].[IdInvestimento] = @IdInvestimento" +
                                         " And [MovimentacoesInvestimentos].[VoExcluido] = 0 " +
                                         " ORDER BY" +
                                         " Data";

                    var model = new { IdUsuario = idUsuario, IdInvestimento = idInvestimento };
                    var retornoBd = contexto.Query<ExtratoModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Extrato obtido com sucesso.";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção do extrato (FACEA1B5): {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }


        [HttpGet]
        [Route("v1/PosicaoInvestimento/{idInvestimento}")]
        public ActionResult ObterPosicaoInvestimentoPorMeta([FromHeader] string token, [FromRoute] int idInvestimento)
        {

            RetornoListaEntidadesModel<PosicaoModel> retornoModel = new RetornoListaEntidadesModel<PosicaoModel>();

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

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = " SELECT " +
                                         " SUM([MovimentacoesInvestimentos].[Valor]) As Valor," +
                                         " [MovimentacoesInvestimentos].[IdMeta]," +
                                         " [Metas].[Nome]" +
                                         " FROM " +
                                         " [Main].[MovimentacoesInvestimentos] WITH (NOLOCK)" +
                                         " JOIN [Main].[Metas] WITH (NOLOCK) ON (MovimentacoesInvestimentos.IdMeta = Metas.Id)" +
                                         " WHERE [MovimentacoesInvestimentos].[IdUsuario] = @IdUsuario " +
                                         " And [MovimentacoesInvestimentos].[IdInvestimento] = @IdInvestimento " +
                                         " And [MovimentacoesInvestimentos].[VoExcluido] = 0 " +
                                         " GROUP BY " +
                                         " [MovimentacoesInvestimentos].[IdMeta]," +
                                         " [Metas].[Nome]" +
                                         " ORDER BY" +
                                         " [Metas].[Nome]";

                    var model = new { IdUsuario = idUsuario, IdInvestimento = idInvestimento };
                    var retornoBd = contexto.Query<PosicaoModel>(selectQuery, model).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Extrato obtido com sucesso.";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção do extrato (54914750): {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

    }

}

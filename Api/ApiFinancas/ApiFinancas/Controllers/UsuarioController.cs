using ApiFinancas.Models;
using ApiFinancas.Services;
using ApiFinancas.Enums;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiFinancas.Controllers
{
    /// <summary>
    /// Controller de Usuário
    /// </summary>
    [Route("Usuario")]
    public class UsuarioController : ControllerBase
    {

        string conexaoBanco = System.Environment.GetEnvironmentVariable("BdFutturo");

        ILogger<UsuarioController> log;

        public UsuarioController(ILogger<UsuarioController> logger)
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
        public ActionResult CriarUsuario([FromHeader] string token, [FromBody] UsuarioModel usuarioModel)
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
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    usuarioModel.DataInclusao = DateTime.Now;
                    usuarioModel.DataAlteracao = usuarioModel.DataInclusao;
                    usuarioModel.Versao = 1;
                    usuarioModel.VoExcluido = false;

                    string insertQuery = " INSERT INTO Controle.Usuarios (DataInclusao, DataAlteracao, Versao, VoExcluido, Login, Celular, Senha, PrimeiroNome, IdUsuarioPai, Status, ConvitesAtivos, CodigoAlteracaoSenha, TipoConta) " +
                                         " VALUES  (@DataInclusao, @DataAlteracao, @Versao, @VoExcluido, @Login, @Celular, @Senha, @PrimeiroNome, @IdUsuarioPai, @Status, @ConvitesAtivos, @CodigoAlteracaoSenha, @TipoConta); " +
                                         " SELECT CAST(SCOPE_IDENTITY() as int);";


                    var retornoBd = contexto.QuerySingle<int>(insertQuery, usuarioModel);
                    usuarioModel.Id = retornoBd;
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Usuário incluído com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na inclusão do usuário: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarUsuario([FromHeader] string token, [FromBody] UsuarioModel usuarioModel, [FromRoute] int id)
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

                if (id != idUsuario && tipoUsuario != TipoContaEnum.UsuarioAdministrador)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    usuarioModel.Id = id;
                    string insertQuery = "UPDATE [Controle].[Usuarios] " +
                                        " SET [PrimeiroNome] = @PrimeiroNome, " +
                                        " [Celular] = @Celular, " +
                                        " [Senha] = @Senha, " +
                                        " [DataAlteracao] = GETDATE()," +
                                        " [Versao] = Versao+1 " +
                                        " WHERE Id = @Id AND VoExcluido=0";

                    var retorno = contexto.Execute(insertQuery, usuarioModel);
                };

                retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                retornoModel.Mensagem = "Usuário atualizado com sucesso";
                return Ok(retornoModel);
            }
            catch (Exception ex)
            {
                retornoModel.Mensagem = $"Erro na alteração do usuário: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }


        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterUsuario([FromHeader] string token, [FromRoute] int id)
        {
            RetornoEntidadeModel<UsuarioModel> retornoModel = new RetornoEntidadeModel<UsuarioModel>();
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

                if (id != idUsuario && tipoUsuario!= TipoContaEnum.UsuarioAdministrador)
                {
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var usuarioModel = new { Id = id, IdToken = idUsuario };
                    string insertQuery = " Select * FROM [Controle].[Usuarios] " +
                                         " WHERE Id = @Id AND VoExcluido=0";

                    var retornoBd = contexto.Query<UsuarioModel>(insertQuery, usuarioModel).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Usuário não localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Usuário obtido com sucesso";
                        retornoModel.Entidade = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção do usuário: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }

        [HttpGet]
        [Route("v1/ObterToken")]
        public ActionResult ObterToken([FromHeader] string login, [FromHeader] string senha)
        {
            RetornoBaseModel retornoModel = new RetornoBaseModel();
            try
            {
                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string insertQuery = " Select Id, TipoConta FROM [Controle].[Usuarios] " +
                                         " WHERE VoExcluido=0 AND Login = @Morango AND Senha = @Banana";

                    var modeloLogin = new { Morango = login, Banana = senha };
                    var retornoBd = contexto.Query<UsuarioModel>(insertQuery, modeloLogin).FirstOrDefault();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Usuário não localizado ou senha inválida.";
                        return StatusCode(400, retornoModel);
                    }

                    string tokenAberto = $"{retornoBd.Id}|{(int)retornoBd.TipoConta}|{DateTime.Now.AddHours(2).Ticks}";
                    string tokenFechado = CriptografiaService.EncryptStringToken(tokenAberto);

                    retornoModel.Mensagem = tokenFechado;
                    retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                    return StatusCode(200, retornoModel);
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro no login do usuário do usuário: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }

        }

        [HttpGet]
        [Route("v1")]
        public ActionResult ObterUsuarios([FromHeader] string token)
        {

            RetornoListaEntidadesModel<UsuarioModel> retornoModel = new RetornoListaEntidadesModel<UsuarioModel>();
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
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário.";
                    return StatusCode(403, retornoModel);
                }


                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    string selectQuery = " Select * FROM [Controle].[Usuarios] WHERE VoExcluido=0 ORDER BY PrimeiroNome";

                    var retornoBd = contexto.Query<UsuarioModel>(selectQuery).ToList();

                    if (retornoBd == null)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Nenhum registro localizado";
                        return StatusCode(400, retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Usuários obtidos com sucesso";
                        retornoModel.ListaEntidades = retornoBd;
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na obtenção do usuário: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirUsuario([FromHeader] string token, [FromRoute] int id)
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
                    retornoModel.Codigo = CodigosRetornoEnum.NaoAutorizado;
                    retornoModel.Mensagem = "Método não permitido para este usuário.";
                    return StatusCode(403, retornoModel);
                }

                using (var contexto = new SqlConnection(conexaoBanco))
                {
                    var investimento = new { Id = id };
                    string insertQuery = " UPDATE [Controle].[Usuarios] " +
                                         " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                         " WHERE Id = @Id AND VoExcluido=0";

                    var retorno = contexto.Execute(insertQuery, investimento);

                    if (retorno == null || retorno == 0)
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.NaoLocalizado;
                        retornoModel.Mensagem = "Usuário não localizado";
                        return StatusCode(400,retornoModel);
                    }
                    else
                    {
                        retornoModel.Codigo = CodigosRetornoEnum.Sucesso;
                        retornoModel.Mensagem = "Usuário excluído com sucesso";
                        return Ok(retornoModel);
                    }
                };
            }
            catch (Exception ex)
            {

                retornoModel.Mensagem = $"Erro na exclusão do usuário: {ex.Message}";
                retornoModel.Codigo = CodigosRetornoEnum.Excecao;
                return StatusCode(500, retornoModel);
            }
            

        }
    }

}

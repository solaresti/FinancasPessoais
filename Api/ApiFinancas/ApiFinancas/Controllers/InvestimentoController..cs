using ApiFinancas.Models;
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
        /// Obtém a lista de Fabricantes
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

            using (var contexto = new SqlConnection(conexaoBanco))
            {
                string insertQuery = " INSERT INTO [Main].[Investimentos]([IdUsuario],[Nome],[Vencimento],[DataInclusao],[DataAlteracao],[Versao],[VoExcluido],[Categoria],[Risco])" +
                                     " VALUES (@IdUsuario,@Nome,@Vencimento,GETDATE(),GETDATE(),1,0,@Categoria,@Risco);" +
                                     " SELECT CAST(SCOPE_IDENTITY() as int);";


                var retorno = contexto.QuerySingle<int>(insertQuery,investimento);
                investimento.Id = retorno;
            };

            return Ok(investimento);
        }

        [HttpPut]
        [Route("v1/{id}")]
        public ActionResult AlterarInvestimento([FromHeader] string token, [FromBody] InvestimentoModel investimento, [FromRoute] int id)
        {

            using (var contexto = new SqlConnection(conexaoBanco))
            {
                investimento.Id = id;
                string insertQuery = "UPDATE [Main].[Investimentos] " +
                                    " SET [Nome] = @Nome, " +
                                    " [Vencimento] = @Vencimento, " +
                                    " [DataAlteracao] = GETDATE()," +
                                    " [Versao] = Versao+1," +
                                    " [Categoria] = @Categoria," +
                                    " [Risco] = @Risco " +
                                    " WHERE Id = @Id AND VoExcluido=0";

                var retorno = contexto.Execute(insertQuery, investimento);
            };

            return Ok(investimento);
        }

        [HttpGet]
        [Route("v1/{id}")]
        public ActionResult ObterInvestimento([FromHeader] string token, [FromRoute] Guid id)
        {

            using (var contexto = new SqlConnection(conexaoBanco))
            {
                var investimento = new { Id = id };
                string insertQuery = " Select * FROM [Main].[Investimentos] " +
                                     " WHERE Id = @Id AND VoExcluido=0";

                var retorno = contexto.Query<InvestimentoModel>(insertQuery, investimento).FirstOrDefault();

                if (retorno==null)
                {
                    return StatusCode(404);
                }
                else
                {
                    return Ok(retorno);
                }
            };
            
        }

        [HttpGet]
        [Route("v1")]
        public ActionResult ObterInvestimentos([FromHeader] string token)
        {

            using (var contexto = new SqlConnection(conexaoBanco))
            {
                string insertQuery = " Select * FROM [Main].[Investimentos] WHERE VoExcluido=0 ORDER Nome";

                var retorno = contexto.Query<InvestimentoModel>(insertQuery).ToList();

                if (retorno == null)
                {
                    return StatusCode(404);
                }
                else
                {
                    return Ok(retorno);
                }
            };

        }

        [HttpDelete]
        [Route("v1/{id}")]
        public ActionResult ExcluirInvestimento([FromHeader] string token, [FromRoute] int id)
        {

            using (var contexto = new SqlConnection(conexaoBanco))
            {
                var investimento = new { Id = id };
                string insertQuery = " UPDATE [Main].[Investimentos] " +
                                     " SET VoExcluido=1, DataAlteracao = Getdate(), Versao=Versao+1" +
                                     " WHERE Id = @Id AND VoExcluido=0";

                var retorno = contexto.Execute(insertQuery, investimento);

                if (retorno == null || retorno==0)
                {
                    return StatusCode(404);
                }
                else
                {
                    return Ok(retorno);
                }
            };

        }
    }

}

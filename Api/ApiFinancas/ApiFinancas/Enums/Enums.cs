namespace ApiFinancas.Enums
{
    /// <summary>
    /// Código de retorno da API
    /// </summary>
    public enum CodigosRetornoEnum
    {
        Sucesso = 1,
        Excecao = -1,
        NaoLocalizado = -2,
        NaoAutorizado = 3
    }

    /// <summary>
    /// Tipod e conta
    /// </summary>
    public enum TipoContaEnum
    {
        UsuarioComum = 1,
        UsuarioAdministrador = 99
    }
}

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
        NaoAutorizado = -3,
        PayloadInvalido=-4,
        DadosInvalidos=-5
    }

    /// <summary>
    /// Tipod e conta
    /// </summary>
    public enum TipoContaEnum
    {
        UsuarioComum = 1,
        UsuarioAdministrador = 99
    }

    /// <summary>
    /// Categoria de investimentos
    /// </summary>
    public enum CategoriaInvestimentoEnum
    {
        RendaFixa = 1,
        RendaVariavel = 2,
        Cambial = 3,
        Criptomoedas = 4,
        MultiMercados = 5
    }

    /// <summary>
    /// Categoria de investimentos
    /// </summary>
    public enum RiscoInvestimentoEnum
    {
        Baixo = 1,
        Médio = 2,
        Alto = 3,
    }
}

namespace PizzariaApp.Models
{
    /// <summary>
    /// Representa as etapas do fluxo de um pedido dentro da pizzaria.
    /// </summary>
    public enum StatusPedido
    {
        Pendente = 0,
        EmPreparo = 1,
        ProntoParaEntrega = 2,
        Entregue = 3
    }

    public enum TamanhoPizza
    {
        Pequena = 0,
        Media = 1,
        Grande = 2
    }
}

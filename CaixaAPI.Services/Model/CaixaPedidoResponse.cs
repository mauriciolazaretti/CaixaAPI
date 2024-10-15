namespace CaixaAPI.Services.Model
{
    public record CaixaPedidoResponse
    (
        string? caixa_id,
        List<string> produtos,
        string? observacao
    );

}

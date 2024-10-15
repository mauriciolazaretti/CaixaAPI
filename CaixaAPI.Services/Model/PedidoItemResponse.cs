namespace CaixaAPI.Services.Model
{
    public record PedidoItemResponse
    (
        int pedido_id,
        List<CaixaPedidoResponse> caixas
    );

}

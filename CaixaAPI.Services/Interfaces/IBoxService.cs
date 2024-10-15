using CaixaAPI.Services.Model;

namespace CaixaAPI.Services.Interfaces
{
    public interface IBoxService
    {
        PedidoResponse Calcular(PedidoInput input);
    }
}

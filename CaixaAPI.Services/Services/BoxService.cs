using CaixaAPI.Services.Interfaces;
using CaixaAPI.Services.Model;

namespace CaixaAPI.Services.Services
{
    public class BoxService : IBoxService
    {
        //caixas em cm3
        private readonly decimal _caixa1Volume = 96000m;
        private readonly decimal _caixa2Volume = 160000;
        private readonly decimal _caixa3Volume = 240000m;
        public PedidoResponse Calcular(PedidoInput input)
        {
            var pedidoItems = new List<PedidoItemResponse>();
            foreach (var pedido in input.Pedidos)
            {
                var items = pedido.produtos
                    .Select(x => new { x.produto_id, dimensao = x.dimensoes.Largura * x.dimensoes.Comprimento * x.dimensoes.Altura })
                    .ToList()
                    .OrderByDescending(x => x.dimensao)
                    .ToList();

                Dictionary<string, decimal> bins = new Dictionary<string, decimal>()
                {
                    { "Caixa 1", _caixa1Volume },
                    { "Caixa 2", _caixa2Volume },
                    { "Caixa 3", _caixa3Volume }
                };

                Dictionary<string, List<string>> produtos = new Dictionary<string, List<string>>();
                var sum = items.Sum(x => x.dimensao);
                var binAllItens = bins
                    .OrderBy(x => x.Value)
                    .FirstOrDefault(x => x.Value >= sum && x.Value - sum < x.Value);
                //tentando encaixar todos itens na menor caixa possível
                if (!binAllItens.Equals(default(KeyValuePair<string, decimal>)))
                {
                    produtos.Add(binAllItens.Key, items.Select(x => x.produto_id).ToList());
                    items.Clear();

                }
                foreach (var item in items)
                {
                    var bestOption = string.Empty;
                    var bin = bins
                        .Where(x => x.Value >= item.dimensao && x.Value - item.dimensao < x.Value)
                        .OrderByDescending(x => x.Value)
                        .FirstOrDefault();

                    if (!bin.Equals(default(KeyValuePair<string, decimal>)))
                    {
                        bestOption = bin.Key;
                        bins[bin.Key] = bin.Value - item.dimensao;

                    }
                    else
                    {
                        bestOption = "";

                    }
                    if (!produtos.ContainsKey(bestOption))
                    {
                        produtos.Add(bestOption, new List<string>() { item.produto_id });
                    }
                    else
                    {
                        produtos[bestOption].Add(item.produto_id);

                    }

                }

                var response = produtos
                        .Select(x => new CaixaPedidoResponse(x.Key == "" ? null : x.Key, x.Value, x.Key == "" ? "Produto não cabe em nenhuma caixa disponível." : null))
                        .ToList();
                pedidoItems.Add(new PedidoItemResponse(pedido.pedido_id, response));
            }
            return new PedidoResponse(pedidoItems);
        }
    }

}
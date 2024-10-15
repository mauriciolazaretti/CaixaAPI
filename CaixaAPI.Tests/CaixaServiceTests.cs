using CaixaAPI.Services.Interfaces;
using CaixaAPI.Services.Model;
using CaixaAPI.Services.Services;


namespace CaixaAPI.Tests
{
    public class CaixaServiceTests
    {
        private readonly IBoxService _boxService;

        public CaixaServiceTests()
        {
            _boxService = new BoxService();
        }

        [Fact]
        public void Calcular_ShouldPackItemsIntoBox1()
        {
            // Arrange
            var pedidoInput = new PedidoInput
            (
                new List<Pedido>
                {
                    new Pedido
                    (
                        1,
                        new List<Produto>
                        {
                            new Produto ("A", new Dimensao (10, 20, 30)),
                            new Produto ("B", new Dimensao (5, 10, 15)),
                        }
                    )
                }
            );

            // Act
            var response = _boxService.Calcular(pedidoInput);

            // Assert
            Assert.Single(response.pedidos);
            var pedidoResponse = response.pedidos.First();
            Assert.NotNull(pedidoResponse);
            Assert.Equal(1,pedidoResponse.pedido_id);

            Assert.True(pedidoResponse.caixas.Count == 1); 

            // Assert contents of Caixa 1
            var caixa1Response = pedidoResponse.caixas.FirstOrDefault(c => c.caixa_id == "Caixa 1");
            Assert.NotNull(caixa1Response);
            Assert.Equal(2,caixa1Response.produtos.Count); 
            Assert.Equal("A", caixa1Response.produtos.First());

        }
        [Fact]
        public void Calcular_ShouldPackItemsIntoBox2()
        {
            // Arrange
            var pedidoInput = new PedidoInput
            (
                new List<Pedido>
                {
                    new Pedido
                    (
                        1,
                        new List<Produto>
                        {
                            new Produto ("A", new Dimensao (10, 20, 30)),
                            new Produto ("B", new Dimensao (50, 50, 55)),
                        }
                    )
                }
            );

            // Act
            var response = _boxService.Calcular(pedidoInput);

            // Assert
            Assert.Single(response.pedidos);
            var pedidoResponse = response.pedidos.First();
            Assert.NotNull(pedidoResponse);
            Assert.Equal(1, pedidoResponse.pedido_id);

            Assert.True(pedidoResponse.caixas.Count == 1);

            // Assert contents of Caixa 1
            var caixa2Response = pedidoResponse.caixas.FirstOrDefault(c => c.caixa_id == "Caixa 2");
            Assert.NotNull(caixa2Response);
            Assert.Equal(2, caixa2Response.produtos.Count);
            Assert.Equal("B", caixa2Response.produtos.First());

        }
        [Fact]
        public void Calcular_ShouldPackItemsIntoBox3()
        {
            // Arrange
            var pedidoInput = new PedidoInput
            (
                new List<Pedido>
                {
                    new Pedido
                    (
                        1,
                        new List<Produto>
                        {
                            new Produto ("A", new Dimensao (30, 50, 50)),
                            new Produto ("B", new Dimensao (50, 50, 55)),
                        }
                    )
                }
            );

            // Act
            var response = _boxService.Calcular(pedidoInput);

            // Assert
            Assert.Single(response.pedidos);
            var pedidoResponse = response.pedidos.First();
            Assert.NotNull(pedidoResponse);
            Assert.Equal(1, pedidoResponse.pedido_id);

            Assert.True(pedidoResponse.caixas.Count == 1);

            // Assert contents of Caixa 1
            var caixa3Response = pedidoResponse.caixas.FirstOrDefault(c => c.caixa_id == "Caixa 3");
            Assert.NotNull(caixa3Response);
            Assert.Equal(2, caixa3Response.produtos.Count);
            Assert.Equal("B", caixa3Response.produtos.First());

        }
        [Fact]
        public void Calcular_ShouldPackItemsIntoManyBoxes()
        {
            // Arrange
            var pedidoInput = new PedidoInput
            (
                new List<Pedido>
                {
                    new Pedido
                    (
                        1,
                        new List<Produto>
                        {
                            new Produto ("A", new Dimensao (15,15, 30)),
                            new Produto ("PS6", new Dimensao (30, 50, 40)),
                            new Produto ("APPLE VISION", new Dimensao (30, 40, 30)),
                            new Produto ("B", new Dimensao (50, 50, 55)),
                        }
                    )
                }
            );

            // Act
            var response = _boxService.Calcular(pedidoInput);

            // Assert
            Assert.Single(response.pedidos);
            var pedidoResponse = response.pedidos.First();
            Assert.NotNull(pedidoResponse);
            Assert.Equal(1, pedidoResponse.pedido_id);

            Assert.True(pedidoResponse.caixas.Count > 1);

            // Assert contents of Caixa 1
            var caixa3Response = pedidoResponse.caixas.FirstOrDefault(c => c.caixa_id == "Caixa 3");
            Assert.NotNull(caixa3Response);
            Assert.Equal(2, caixa3Response.produtos.Count);
            Assert.Equal("B", caixa3Response.produtos.First());

        }

        [Fact]
        public void Calcular_ShouldIndicateItemDoesntFitIfNoBoxAvailable()
        {
            // Arrange
            var pedidoInput = new PedidoInput
            (
                new List<Pedido>
                {
                    new Pedido
                    (
                        1,
                        new List<Produto>
                        {
                            new Produto ("A", new Dimensao (100, 200, 300)),
                        }
                    )
                }
            );
            // Act
            var response = _boxService.Calcular(pedidoInput);

            // Assert
            Assert.Single(response.pedidos); // One response for the single pedido
            var pedidoResponse = response.pedidos.First();
            Assert.NotNull(pedidoResponse);
            Assert.Equal(1,pedidoResponse.pedido_id);

            Assert.Single(pedidoResponse.caixas); // One box used (empty)

            var caixa1Response = pedidoResponse.caixas.First();
            Assert.Null( caixa1Response.caixa_id);
            Assert.NotNull(caixa1Response);
            Assert.NotEmpty(caixa1Response.produtos); // No products in Caixa 1
            
            Assert.Equal("Produto não cabe em nenhuma caixa disponível.",caixa1Response.observacao);
        }

        [Fact]
        public void Calcular_ShouldReturnEmpty()
        {
            // Arrange
            var pedidoInput = new PedidoInput
            (
                new List<Pedido>
                ()
            );
            // Act
            var response = _boxService.Calcular(pedidoInput);

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.pedidos); // One response for the single pedido
            
        }
    }
}
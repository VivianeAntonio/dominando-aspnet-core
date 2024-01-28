using AppSemTemplate.Controllers;
using AppSemTemplate.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System.Security.Claims;

namespace Testes
{
    public class ControllerTests
    {
        [Fact]
        public void TesteController_Index_Sucesso()
        {
            // Arrange
            var controller = new TesteController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_Index_Sucesso()
        {
            //Arrange

            //DbContext Options
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            //Contexto
            var ctx = new AppDbContext(options);

            ctx.Produtos.Add(new AppSemTemplate.Models.Produto { Id = 1, Nome = "Produto 1", Valor = 10m });
            ctx.Produtos.Add(new AppSemTemplate.Models.Produto { Id = 2, Nome = "Produto 2", Valor = 10m });
            ctx.Produtos.Add(new AppSemTemplate.Models.Produto { Id = 3, Nome = "Produto 3", Valor = 10m });
            ctx.SaveChanges();

            //Identity
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();    
            mockClaimsIdentity.Setup(m => m.Name).Returns("teste@teste.com");

            var principal = new ClaimsPrincipal(mockClaimsIdentity.Object);

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(principal);

            //Controller
            var controller = new ProdutosController(ctx)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockContext.Object
                }
            };

            //Act
            var result = controller.Index().Result;

            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
using AppSemTemplate.Controllers;
using AppSemTemplate.Data;
using AppSemTemplate.Models;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

            var imgService = new Mock<IImageUploadService>();

            //Controller
            var controller = new ProdutosController(ctx, imgService.Object)
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

        [Fact]
        public void ProdutoController_CriarNovoProduto_Sucesso()
        {
            //Arrange

            //DbContext Options
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            //Contexto
            var ctx = new AppDbContext(options);

            //IFormFile
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.jpg";
            fileMock.Setup(_ => _.FileName).Returns(fileName);
                                   
            var imgService = new Mock<IImageUploadService>();
            imgService.Setup(s => s.UploadArquivo(
                new ModelStateDictionary(),
                fileMock.Object,
                It.IsAny<string>()
                )).ReturnsAsync(true);

            //Controller
            var controller = new ProdutosController(ctx, imgService.Object);

            //Produto
            var produto = new Produto
            {
                Id = 1,
                ImagemUpload = fileMock.Object,
                Nome = "Teste",
                Valor = 50
            };

            //Act
            var result = controller.CriarNovoProduto(produto).Result;

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ProdutoController_CriarNovoProduto_ErroValidacaoProduto()
        {
            //Arrange

            //DbContext Options
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            //Contexto
            var ctx = new AppDbContext(options);
                                              
            var imgService = new Mock<IImageUploadService>();
            
            //Controller
            var controller = new ProdutosController(ctx, imgService.Object);

            controller.ModelState.AddModelError("Nome", "O campo nome é requerido");

            //Produto
            var produto = new Produto
            {               
            };

            //Act
            var result = controller.CriarNovoProduto(produto).Result;

            //Assert
            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }
    }
}
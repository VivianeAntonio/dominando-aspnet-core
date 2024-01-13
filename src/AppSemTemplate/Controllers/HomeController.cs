using AppSemTemplate.Configuration;
using AppSemTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfiguration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration,
                              IOptions<ApiConfiguration> apiConfiguration,
                              ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _apiConfiguration = apiConfiguration.Value;
            _logger = logger;   
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Information");

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var apiConfig = new ApiConfiguration();
            _configuration.GetSection(ApiConfiguration.ConfigName).Bind(apiConfig);

            var secret = apiConfig.UserSecret;
            var user = _configuration[$"{ApiConfiguration.ConfigName}:UserKey"];
            var domain = apiConfig.Domain;

            return View();
        }

        [Route("teste")]
        public IActionResult Teste()
        {
            throw new Exception("Erro generico");
            return View("Index");  
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde";
                modelErro.Titulo = "Ocorreu um erro";
                modelErro.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página não existe";
                modelErro.Titulo = "Ops! Página não encontrada";
                modelErro.ErrorCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isso";
                modelErro.Titulo = "Acesso negado";
                modelErro.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde";
                modelErro.Titulo = "Ocorreu um erro";
                modelErro.ErrorCode = id;
            }
            else
            {
                return StatusCode(500);
            }
            return View("Error", modelErro);
        }
    }
}

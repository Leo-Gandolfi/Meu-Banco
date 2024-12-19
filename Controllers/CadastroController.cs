using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using CadastroWebApp.Models;

namespace CadastrosWebApp.Controllers
{
    public class CadastroController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CadastroController> _logger;

        public CadastroController(IHttpClientFactory httpClientFactory, ILogger<CadastroController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CadastroAPI");

            var response = await client.GetFromJsonAsync<List<Cadastro>>("Cadastro");

            var cadastros = response ?? new List<Cadastro>();

            return View(cadastros);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cadastro cadastro)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados inválidos fornecidos para criação.");
                return View(cadastro); 
            }
            _logger.LogInformation("Recebendo dados do formulário de criação...");

            var client = _httpClientFactory.CreateClient("CadastroAPI");

            var response = await client.PostAsJsonAsync("cadastro", cadastro);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Cadastro realizado com sucesso!");
                return RedirectToAction("Index");
            }

            _logger.LogError("Falha ao realizar cadastro. Status: {StatusCode}", response.StatusCode);
            TempData["Error"] = "Erro ao realizar o cadastro.";
            return View(cadastro); 
        }
        public ActionResult DisplayImage(string imageName)
        {
            var imagePath = Path.Combine("wwwroot", "images", imageName);

            if (System.IO.File.Exists(imagePath))   
            {
                return File(imagePath, "image/png");
            }
            else
            {
                return File("~/images/default-image.png", "image/png");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("CadastroAPI");

            var response = await client.DeleteAsync($"cadastro/{id}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Cadastro excluído com sucesso!");
                return RedirectToAction("Index");
            }

            _logger.LogError("Falha ao excluir cadastro. Status: {StatusCode}", response.StatusCode);
            TempData["Error"] = "Erro ao excluir o cadastro.";
            return RedirectToAction("Index");
        }
    }
}

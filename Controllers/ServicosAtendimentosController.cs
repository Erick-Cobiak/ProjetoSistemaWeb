using Microsoft.AspNetCore.Mvc;

namespace Sistema_Veterinário.Controllers
{
    public class ServicosAtendimentosController : Controller
    {
        public IActionResult PageServAtend()
        {
            return View();
        }
    }
}

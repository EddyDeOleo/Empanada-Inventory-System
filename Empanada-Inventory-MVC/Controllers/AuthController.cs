using Microsoft.AspNetCore.Mvc;
using EmpanadaInventory.Models;

namespace EmpanadaInventory.Controllers
{
    public class AuthController : Controller
    {
        private const string UsuarioValido = "admin";
        private const string PasswordValida = "admin123";

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Usuario == UsuarioValido && model.Password == PasswordValida)
            {
                HttpContext.Session.SetString("usuario", model.Usuario);
                return RedirectToAction("Index", "Empanada");
            }

            ViewBag.Error = "Usuario no encontrado.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
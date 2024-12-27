using Microsoft.AspNetCore.Mvc;
using WebLogin.Models;
using WebLogin.Datos;
using WebLogin.Servicios;

namespace WebLogin.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string clave)
        {
            UsuarioDTO usuario = DBUsuario.Validar(correo, UtilidadServicio.ConvertirSHA256(clave));

            if (usuario == null)
            {
                ViewBag.Mensaje = "No se encontró el usuario.";
                return View();
            }

            if (!usuario.Confirmado)
            {
                ViewBag.Mensaje = $"Falta confirmar su correo electrónico a {correo}.";
            }
            else if (usuario.Restablecer)
            {
                ViewBag.Mensaje = $"Se ha solicitado restablecer correo: {correo}.";
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}

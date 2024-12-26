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

            if(usuario == null)
            {
                ViewBag.Mensaje = "No se encontro el usuario.";
                return View();
            }

            if(usuario.Confirmado)

            return View();
        }
    }
}

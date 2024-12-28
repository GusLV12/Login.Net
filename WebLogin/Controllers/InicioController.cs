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

        [HttpPost]
        public IActionResult Registrar(UsuarioDTO user)
        {
            if(user.Clave != user.ConfirmarClave)
            {
                ViewBag.Nombre = user.Nombre;
                ViewBag.Correo = user.Correo;
                ViewBag.Mensaje = "La contraseña no coinciden";
                return View();
            }

            if(DBUsuario.Obtener(user.Correo) == null)
            {
                user.Clave = UtilidadServicio.ConvertirSHA256(user.Clave);
                user.Token = UtilidadServicio.GenerarToken();
                user.Restablecer = false;
                user.Confirmado = false;
                bool res = DBUsuario.Registrar(user);
            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado"
            }

            return View();
        }
    }
}

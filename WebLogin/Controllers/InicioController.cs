using Microsoft.AspNetCore.Mvc;
using WebLogin.Models;
using WebLogin.Datos;
using WebLogin.Servicios;
using Microsoft.AspNetCore.Hosting;

namespace WebLogin.Controllers
{
    public class InicioController : Controller
    {
        private readonly PathService _pathService;

        public InicioController(PathService pathService)
        {
            _pathService = pathService;
        }

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
            if (user.Clave != user.ConfirmarClave)
            {
                ViewBag.Nombre = user.Nombre;
                ViewBag.Correo = user.Correo;
                ViewBag.Mensaje = "La contraseña no coinciden";
                return View();
            }

            if (DBUsuario.Obtener(user.Correo) == null)
            {
                user.Clave = UtilidadServicio.ConvertirSHA256(user.Clave);
                user.Token = UtilidadServicio.GenerarToken();
                user.Restablecer = false;
                user.Confirmado = false;
                bool res = DBUsuario.Registrar(user);

                if (res)
                {
                    // Usar PathService para obtener la ruta
                    string path = _pathService.ObtenerRutaArchivo("Plantilla/Confirmar.html");
                    string content = System.IO.File.ReadAllText(path);
                    string url = string.Format("{0}://{1}/Inicio/Confirmar?token={2}",
                            Request.Scheme,
                            Request.Host,
                            user.Token);

                    string htmlBody = string.Format(content, user.Nombre, url);

                    CorreoDTO correoDTO = new CorreoDTO()
                    {
                        Para = user.Correo,
                        Asunto = "Correo de Confirmacion",
                        Contenido = htmlBody,
                    };

                    bool send = CorreoServicio.Enviar(correoDTO);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada. Hemos enviado un mensaje al correo {user.Correo}";
                }
                else
                {
                    ViewBag.Creado = false;
                    ViewBag.Mensaje = "No se pudo crear su cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado";
            }

            return View();
        }
    }
}


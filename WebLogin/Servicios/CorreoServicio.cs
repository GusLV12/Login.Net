using WebLogin.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebLogin.Servicios
{
    public static class CorreoServicio
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Puerto = 587;
        private static string _NombreSend = "GusLV";
        private static string _Correo = "gusmedia117@gmail.com";
        private static string _Cllave = "";
    }
}

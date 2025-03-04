using System.Security.Cryptography;
using System.Text;

namespace CCTP_Manage.Helpers
{
    public class Encriptacion
    {
        public static string Encriptar(string cadena)
        {
            //salt
            cadena = cadena + "ITESRC2025";

            byte[] datos = Encoding.UTF8.GetBytes(cadena);
            var alg = SHA512.Create();

            byte[] encriptar = alg.ComputeHash(datos);

            string salida = BitConverter.ToString(encriptar).Replace("-", "");

            return salida;
        }
    }
}

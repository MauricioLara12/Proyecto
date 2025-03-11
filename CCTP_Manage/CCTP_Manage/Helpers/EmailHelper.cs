using System.Net.Mail;

public class EmailHelper
{
    public void EnviarNotificacionPorCorreo(string destinatario, string asunto, string cuerpo)
    {
        try
        {
            string body =
                "<body>" +
                "<h1>Recordatorio de Reservación</h1>" +
                "<h4>Estimado(a) cliente,</h4>" +
                "<span>Le recordamos que su reservación está próxima.</span>" +
                "<br/><br/><span>Saludos Cordiales.</span>" +
                "</body>";

            MailMessage correo = new MailMessage();
            SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com");

            correo.From = new MailAddress("tu-correo@gmail.com");
            correo.To.Add(destinatario);
            correo.Subject = asunto;
            correo.Body = body;
            correo.IsBodyHtml = true;

            clienteSmtp.Port = 587; // Puerto para Gmail
            clienteSmtp.Credentials = new System.Net.NetworkCredential("tu-correo@gmail.com", "tu-contraseña");
            clienteSmtp.EnableSsl = true; // SSL habilitado para mayor seguridad

            clienteSmtp.Send(correo);
        }
        catch
        {
            // Manejar la excepción (registrar, mostrar mensaje, etc.)
        }
    }
}

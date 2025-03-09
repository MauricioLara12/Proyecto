using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using CCTP_Manage.Helpers;

public class ReservacionesController : Controller
{
    private readonly string connectionString = "server=localhost;user=root;password=root;database=saloneventosdb";
    private readonly EmailHelper _emailHelper;

    public ReservacionesController(EmailHelper emailHelper)
    {
        _emailHelper = emailHelper;
    }

    public void VerificarReservacion()
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT usuario_id, fecha_reservacion, hora_inicio FROM reservaciones WHERE fecha_reservacion BETWEEN CURDATE() AND CURDATE() + INTERVAL 2 DAY";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string usuarioId = reader["usuario_id"].ToString();
                    DateTime fechaReservacion = DateTime.Parse(reader["fecha_reservacion"].ToString());
                    TimeSpan horaInicio = TimeSpan.Parse(reader["hora_inicio"].ToString());

                    // Obtener el correo del usuario
                    string destinatario = ObtenerCorreoUsuario(usuarioId);

                    string asunto = "Recordatorio de Reservación";
                    string cuerpo = $"Estimado cliente, le recordamos que tiene una reservación el {fechaReservacion.ToShortDateString()} a las {horaInicio}.";

                    if (!string.IsNullOrEmpty(destinatario))
                    {
                        _emailHelper.EnviarNotificacionPorCorreo(destinatario, asunto, cuerpo);
                    }
                }
            }
        }
    }

    private string ObtenerCorreoUsuario(string usuarioId)
    {
        string correoUsuario = "";

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT correo FROM usuarios WHERE usuario_id = @UsuarioId";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        correoUsuario = reader["correo"].ToString();
                    }
                }
            }
        }

        return correoUsuario;
    }
}

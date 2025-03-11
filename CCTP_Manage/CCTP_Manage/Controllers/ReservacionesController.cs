using CCTP_Manage.Models.Entities;
using CCTP_Manage.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CCTP_Manage.Helpers;

public class ReservacionesController : Controller
{
    private readonly Repository<Reservacione> _reservacionRepository;
    private readonly EmailHelper _emailHelper;

    public ReservacionesController(Repository<Reservacione> reservacionRepository, EmailHelper emailHelper)
    {
        _reservacionRepository = reservacionRepository;
        _emailHelper = emailHelper;
    }

    public void VerificarReservacion()
    {
        // Obtener reservaciones próximas (dentro de los próximos 2 días)
        var reservacionesProximas = _reservacionRepository.GetAll()
            .Where(r => r.FechaReservacion >= DateOnly.FromDateTime(DateTime.Now)
                     && r.FechaReservacion <= DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
            .ToList();

        foreach (var reservacion in reservacionesProximas)
        {
            // Obtener el usuario relacionado con la reservación
            var usuario = reservacion.Usuario;

            if (!string.IsNullOrEmpty(usuario.Correo))
            {
                // Construir el correo de notificación
                string asunto = "Recordatorio de Reservación";
                string cuerpo = $"Estimado {usuario.Nombre}, le recordamos que tiene una reservación el {reservacion.FechaReservacion.ToShortDateString()} a las {reservacion.HoraInicio}.";

                // Enviar el correo de notificación
                _emailHelper.EnviarNotificacionPorCorreo(usuario.Correo, asunto, cuerpo);
            }
        }
    }
}

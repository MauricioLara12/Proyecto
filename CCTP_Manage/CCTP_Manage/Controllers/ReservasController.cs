using Microsoft.AspNetCore.Mvc;
using CCTP_Manage.Models.Entities;
using CCTP_Manage.Repositories;

namespace CCTP_Manage.Controllers
{
    public class ReservasController : Controller
    {
        private readonly Repository<Reservacione> reservaRepository;

        public ReservasController(SaloneventosdbContext context)
        {
            reservaRepository = new Repository<Reservacione>(context);
        }

        [HttpGet]
        public IActionResult CancelarReserva(int id)
        {
            // Obtener la reserva a cancelar
            var reserva = reservaRepository.Get(id);

            if (reserva == null)
            {
                TempData["Error"] = "La reserva no existe o ya fue cancelada.";
                return RedirectToAction("PerfilView", "Account");
            }

            // Pasar la reserva a la vista
            return View(reserva);
        }

        [HttpPost]
        public IActionResult ConfirmarCancelacion(int id)
        {
            var reserva = reservaRepository.Get(id);

            if (reserva == null)
            {
                TempData["Error"] = "La reserva no existe o ya fue cancelada.";
                return RedirectToAction("PerfilView", "Account");
            }

            // Actualizar el estado de la reserva
            reserva.Estado = "Cancelada";

            // Validar si aplica reembolso
            if (DateOnly.FromDateTime(DateTime.Now).AddDays(5) <= reserva.FechaReservacion)
            {
                decimal reembolso = reserva.CostoTotal * 0.5m;
                TempData["Success"] = $"Reserva cancelada. Reembolso: {reembolso:C}.";
            }
            else
            {
                TempData["Success"] = "Reserva cancelada sin derecho a reembolso.";
            }

            reservaRepository.Update(reserva); // Guardar cambios en la base de datos
            return RedirectToAction("PerfilView", "Account");
        }
    }
}

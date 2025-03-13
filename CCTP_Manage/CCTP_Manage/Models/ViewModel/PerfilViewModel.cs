namespace CCTP_Manage.Models.ViewModel
{
    public class ReservacionViewModel
    {
        public int Id { get; set; }
        public DateOnly FechaReservacion { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public decimal CostoTotal { get; set; }
        public string Estado { get; set; } = null!;
        public string EstadoPago { get; set; } = null!;
        public List<string> Zonas { get; set; } = new();
    }

    public class PerfilViewModel
    {
        public string Nombre { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
        public DateOnly FechaRegistro { get; set; }
        public List<ReservacionViewModel> Reservaciones { get; set; } = new();
    }

}

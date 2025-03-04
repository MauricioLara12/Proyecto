using System;
using System.Collections.Generic;

namespace CCTP_Manage.Models.Entities;

public partial class Reservacione
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateOnly FechaReservacion { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public decimal CostoTotal { get; set; }

    public string EstadoPago { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<Reservacionzona> Reservacionzonas { get; set; } = new List<Reservacionzona>();

    public virtual Usuario Usuario { get; set; } = null!;
}

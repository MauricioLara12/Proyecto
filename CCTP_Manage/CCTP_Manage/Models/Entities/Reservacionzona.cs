using System;
using System.Collections.Generic;

namespace CCTP_Manage.Models.Entities;

public partial class Reservacionzona
{
    public int Id { get; set; }

    public int ReservacionId { get; set; }

    public int ZonaId { get; set; }

    public virtual Reservacione Reservacion { get; set; } = null!;

    public virtual Zona Zona { get; set; } = null!;
}

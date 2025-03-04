using System;
using System.Collections.Generic;

namespace CCTP_Manage.Models.Entities;

public partial class Zona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Costo { get; set; }

    public virtual ICollection<Reservacionzona> Reservacionzonas { get; set; } = new List<Reservacionzona>();
}

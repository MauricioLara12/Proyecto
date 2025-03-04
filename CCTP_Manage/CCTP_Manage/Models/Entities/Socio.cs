using System;
using System.Collections.Generic;

namespace CCTP_Manage.Models.Entities;

public partial class Socio
{
    public int Id { get; set; }

    public DateOnly FechaRenovacion { get; set; }

    public virtual Usuario IdNavigation { get; set; } = null!;
}

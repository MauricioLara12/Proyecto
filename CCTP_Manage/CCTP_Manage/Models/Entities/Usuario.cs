using System;
using System.Collections.Generic;

namespace CCTP_Manage.Models.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Contrasena { get; set; } = null!;

    public string TipoUsuario { get; set; } = null!;

    public DateOnly FechaRegistro { get; set; }

    public virtual ICollection<Reservacione> Reservaciones { get; set; } = new List<Reservacione>();

    public virtual Socio? Socio { get; set; }
}

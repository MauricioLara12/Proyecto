using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CCTP_Manage.Models.Entities;

public partial class SaloneventosdbContext : DbContext
{
    public SaloneventosdbContext()
    {
    }

    public SaloneventosdbContext(DbContextOptions<SaloneventosdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Reservacione> Reservaciones { get; set; }

    public virtual DbSet<Reservacionzona> Reservacionzonas { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Zona> Zonas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=saloneventosdb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Reservacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reservaciones");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CostoTotal)
                .HasPrecision(10, 2)
                .HasColumnName("costo_total");
            entity.Property(e => e.Estado)
                .HasMaxLength(45)
                .HasColumnName("estado");
            entity.Property(e => e.EstadoPago)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnType("enum('Pendiente','Pagado','Pago parcial')")
                .HasColumnName("estado_pago");
            entity.Property(e => e.FechaReservacion).HasColumnName("fecha_reservacion");
            entity.Property(e => e.HoraInicio)
                .HasColumnType("time")
                .HasColumnName("hora_inicio");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reservaciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("reservaciones_ibfk_1");
        });

        modelBuilder.Entity<Reservacionzona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reservacionzonas");

            entity.HasIndex(e => e.ReservacionId, "reservacion_id");

            entity.HasIndex(e => e.ZonaId, "zona_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReservacionId).HasColumnName("reservacion_id");
            entity.Property(e => e.ZonaId).HasColumnName("zona_id");

            entity.HasOne(d => d.Reservacion).WithMany(p => p.Reservacionzonas)
                .HasForeignKey(d => d.ReservacionId)
                .HasConstraintName("reservacionzonas_ibfk_1");

            entity.HasOne(d => d.Zona).WithMany(p => p.Reservacionzonas)
                .HasForeignKey(d => d.ZonaId)
                .HasConstraintName("reservacionzonas_ibfk_2");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("socios");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FechaRenovacion).HasColumnName("fecha_renovacion");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Socio)
                .HasForeignKey<Socio>(d => d.Id)
                .HasConstraintName("socios_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "correo").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("curdate()")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoUsuario)
                .HasColumnType("enum('Administrador','Socio','Cliente')")
                .HasColumnName("tipo_usuario");
        });

        modelBuilder.Entity<Zona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("zonas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Costo)
                .HasPrecision(10, 2)
                .HasColumnName("costo");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

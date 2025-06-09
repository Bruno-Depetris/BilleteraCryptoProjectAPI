using System;
using System.Collections.Generic;
using BilleteraCryptoProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace BilleteraCryptoProjectAPI.Data;

public partial class CryptoWalletApiDBContext : DbContext {
    public CryptoWalletApiDBContext() {
    }

    public CryptoWalletApiDBContext(DbContextOptions<CryptoWalletApiDBContext> options)
        : base(options) {
    }

    public static string GetConnectionString() {

        var host = Environment.GetEnvironmentVariable("MYSQL_ADDON_HOST");
        var port = Environment.GetEnvironmentVariable("MYSQL_ADDON_PORT");
        var db = Environment.GetEnvironmentVariable("MYSQL_ADDON_DB");
        var user = Environment.GetEnvironmentVariable("MYSQL_ADDON_USER");
        var pass = Environment.GetEnvironmentVariable("MYSQL_ADDON_PASSWORD");

        return $"Server={host};Port={port};Database={db};Uid={user};Pwd={pass};";
    }

    public virtual DbSet<Accione> Acciones { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cripto> Criptos { get; set; }

    public virtual DbSet<Cuenta> Cuentas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<HistorialPrecio> HistorialPrecios { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Operacione> Operaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=b1iasn4pyeo0luymq2zd-mysql.services.clever-cloud.com;port=3306;database=b1iasn4pyeo0luymq2zd;user=uu5of4u0dglzhdy5;password=wLTPJmRNrnCYn7GWtB9c", ServerVersion.Parse("8.4.2-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Accione>(entity => {
            entity.HasKey(e => e.AccionId).HasName("PRIMARY");

            entity.Property(e => e.AccionId).HasColumnName("AccionID");
            entity.Property(e => e.Accion).HasMaxLength(20);
        });

        modelBuilder.Entity<Cliente>(entity => {
            entity.HasKey(e => e.ClienteId).HasName("PRIMARY");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Cripto>(entity => {
            entity.HasKey(e => e.CriptoCode).HasName("PRIMARY");

            entity.Property(e => e.CriptoCode).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Cuenta>(entity => {
            entity.HasKey(e => e.CuentaId).HasName("PRIMARY");

            entity.HasIndex(e => e.ClienteId, "ClienteID");

            entity.HasIndex(e => e.EstadoId, "EstadoID");

            entity.Property(e => e.CuentaId).HasColumnName("CuentaID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Cuentas_ibfk_1");

            entity.HasOne(d => d.Estado).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Cuentas_ibfk_2");
        });

        modelBuilder.Entity<Estado>(entity => {
            entity.HasKey(e => e.EstadoId).HasName("PRIMARY");

            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .HasColumnName("Estado");
        });

        modelBuilder.Entity<HistorialPrecio>(entity => {
            entity.HasKey(e => e.HistorialId).HasName("PRIMARY");

            entity.HasIndex(e => e.CriptoCode, "CriptoCode");

            entity.Property(e => e.HistorialId).HasColumnName("HistorialID");
            entity.Property(e => e.CriptoCode).HasMaxLength(20);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Fuente).HasMaxLength(100);
            entity.Property(e => e.Precio).HasPrecision(18, 2);

            entity.HasOne(d => d.CriptoCodeNavigation).WithMany(p => p.HistorialPrecios)
                .HasForeignKey(d => d.CriptoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HistorialPrecios_ibfk_1");
        });

        modelBuilder.Entity<Movimiento>(entity => {
            entity.HasKey(e => e.MovimientoId).HasName("PRIMARY");

            entity.HasIndex(e => e.CriptoCode, "CriptoCode");

            entity.HasIndex(e => e.OperacionId, "OperacionID");

            entity.Property(e => e.MovimientoId).HasColumnName("MovimientoID");
            entity.Property(e => e.CantidadCripto).HasPrecision(18, 8);
            entity.Property(e => e.CriptoCode).HasMaxLength(20);
            entity.Property(e => e.EstadoBilletera).HasPrecision(18, 8);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.OperacionId).HasColumnName("OperacionID");

            entity.HasOne(d => d.CriptoCodeNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.CriptoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Movimientos_ibfk_2");

            entity.HasOne(d => d.Operacion).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.OperacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Movimientos_ibfk_1");
        });

        modelBuilder.Entity<Operacione>(entity => {
            entity.HasKey(e => e.OperacionId).HasName("PRIMARY");

            entity.HasIndex(e => e.AccionId, "AccionID");

            entity.HasIndex(e => e.CriptoCode, "CriptoCode");

            entity.HasIndex(e => e.CuentaId, "CuentaID");

            entity.Property(e => e.OperacionId).HasColumnName("OperacionID");
            entity.Property(e => e.AccionId).HasColumnName("AccionID");
            entity.Property(e => e.Cantidad).HasPrecision(18, 8);
            entity.Property(e => e.CriptoCode).HasMaxLength(20);
            entity.Property(e => e.CuentaId).HasColumnName("CuentaID");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.MontoArs)
                .HasPrecision(18, 2)
                .HasColumnName("MontoARS");

            entity.HasOne(d => d.Accion).WithMany(p => p.Operaciones)
                .HasForeignKey(d => d.AccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Operaciones_ibfk_3");

            entity.HasOne(d => d.CriptoCodeNavigation).WithMany(p => p.Operaciones)
                .HasForeignKey(d => d.CriptoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Operaciones_ibfk_2");

            entity.HasOne(d => d.Cuenta).WithMany(p => p.Operaciones)
                .HasForeignKey(d => d.CuentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Operaciones_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

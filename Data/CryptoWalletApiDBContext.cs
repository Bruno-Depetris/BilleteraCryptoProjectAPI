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
        var directUrl = GetEnv("MYSQL_URL", "DATABASE_URL");
        if (!string.IsNullOrWhiteSpace(directUrl)) {
            return BuildMySqlConnectionFromUrl(directUrl);
        }

        var host = GetEnv("MYSQL_ADDON_HOST", "MYSQLHOST");
        var port = GetEnv("MYSQL_ADDON_PORT", "MYSQLPORT");
        var db = GetEnv("MYSQL_ADDON_DB", "MYSQLDATABASE");
        var user = GetEnv("MYSQL_ADDON_USER", "MYSQLUSER");
        var pass = GetEnv("MYSQL_ADDON_PASSWORD", "MYSQLPASSWORD");

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(port) || string.IsNullOrWhiteSpace(db) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass)) {
            var detected = string.Join(", ", new[] {
                $"MYSQL_ADDON_HOST={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQL_ADDON_HOST\"))}",
                $"MYSQL_ADDON_PORT={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQL_ADDON_PORT\"))}",
                $"MYSQL_ADDON_DB={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQL_ADDON_DB\"))}",
                $"MYSQL_ADDON_USER={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQL_ADDON_USER\"))}",
                $"MYSQL_ADDON_PASSWORD={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQL_ADDON_PASSWORD\"))}",
                $"MYSQLHOST={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQLHOST\"))}",
                $"MYSQLPORT={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQLPORT\"))}",
                $"MYSQLDATABASE={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQLDATABASE\"))}",
                $"MYSQLUSER={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQLUSER\"))}",
                $"MYSQLPASSWORD={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQLPASSWORD\"))}",
                $"MYSQL_URL={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"MYSQL_URL\"))}",
                $"DATABASE_URL={!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(\"DATABASE_URL\"))}"
            });
            throw new InvalidOperationException($"Faltan variables de entorno de MySQL. Definir MYSQL_ADDON_*, MYSQL* o MYSQL_URL/DATABASE_URL en el entorno de despliegue. Detectadas: {detected}");
        }

        return $"Server={host};Port={port};Database={db};Uid={user};Pwd={pass};";
    }

    private static string BuildMySqlConnectionFromUrl(string url) {
        var normalized = url.Trim();
        if (!normalized.Contains("://")) {
            normalized = $"mysql://{normalized}";
        }

        var uri = new Uri(normalized);
        var userInfo = uri.UserInfo.Split(':', 2);
        var user = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : string.Empty;
        var pass = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
        var db = uri.AbsolutePath.Trim('/');
        var port = uri.Port > 0 ? uri.Port : 3306;

        if (string.IsNullOrWhiteSpace(uri.Host) || string.IsNullOrWhiteSpace(db) || string.IsNullOrWhiteSpace(user)) {
            throw new InvalidOperationException("MYSQL_URL/DATABASE_URL no tiene el formato esperado para MySQL.");
        }

        return $"Server={uri.Host};Port={port};Database={db};Uid={user};Pwd={pass};";
    }

    private static string? GetEnv(params string[] keys) {
        foreach (var key in keys) {
            var value = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrWhiteSpace(value)) {
                return value;
            }
        }
        return null;
    }

    public virtual DbSet<Accione> Acciones { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cripto> Criptos { get; set; }

    public virtual DbSet<Cuenta> Cuentas { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<HistorialPrecio> HistorialPrecios { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Operacione> Operaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            var connStr = GetConnectionString();
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        }
    }

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

            entity.HasIndex(e => e.ClienteId, "ClienteID");

            entity.HasIndex(e => e.CriptoCode, "CriptoCode");

            entity.Property(e => e.OperacionId).HasColumnName("OperacionID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.CriptoAmount).HasPrecision(18, 8);
            entity.Property(e => e.Money).HasPrecision(18, 2);
            entity.Property(e => e.CriptoCode).HasMaxLength(20);
            entity.Property(e => e.Action).HasMaxLength(20);
            entity.Property(e => e.Datetime).HasColumnType("datetime");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Operaciones)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Operaciones_ibfk_1");

            entity.HasOne(d => d.CriptoCodeNavigation).WithMany(p => p.Operaciones)
                .HasForeignKey(d => d.CriptoCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Operaciones_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}



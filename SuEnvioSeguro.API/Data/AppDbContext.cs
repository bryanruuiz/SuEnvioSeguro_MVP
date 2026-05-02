using Microsoft.EntityFrameworkCore;
using SuEnvioSeguro.API.Models;

namespace SuEnvioSeguro.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<Municipio> Municipios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.DocumentoIdentidad)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Activo)
                .HasDefaultValue(true);

            modelBuilder.Entity<Factura>()
                .HasIndex(f => f.CodigoFactura)
                .IsUnique();

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.FacturasGeneradas)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Envio>()
                .HasIndex(e => e.CodigoEnvio)
                .IsUnique();

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.Factura)
                .WithMany(f => f.Envios)
                .HasForeignKey(e => e.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed inicial de municipios
            SeedMunicipios(modelBuilder);
        }

        private void SeedMunicipios(ModelBuilder modelBuilder)
        {
            var fechaBase = new DateTime(2026, 5, 2);
            modelBuilder.Entity<Municipio>().HasData(
                new Municipio { Id = 1, Nombre = "Medellín", TarifaBase = 5000f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 2, Nombre = "Envigado", TarifaBase = 6000f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 3, Nombre = "Itagüí", TarifaBase = 6000f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 4, Nombre = "Sabaneta", TarifaBase = 6500f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 5, Nombre = "Bello", TarifaBase = 6500f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 6, Nombre = "Caldas", TarifaBase = 7500f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 7, Nombre = "La Estrella", TarifaBase = 6500f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 8, Nombre = "Copacabana", TarifaBase = 7500f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 9, Nombre = "Girardota", TarifaBase = 8000f, Activo = true, FechaCreacion = fechaBase },
                new Municipio { Id = 10, Nombre = "Barbosa", TarifaBase = 10000f, Activo = true, FechaCreacion = fechaBase }
            );
        }
    }
}

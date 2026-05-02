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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // EF Core creará una sola tabla "Personas" con una columna "Discriminator" 
            // para diferenciar entre Clientes y Usuarios (Table-Per-Hierarchy).

            // Configurar relaciones para evitar múltiples caminos de cascada
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany()
                .HasForeignKey(f => f.ClienteDocumento)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.UsuarioDocumento)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
using ApiTransferBank.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTransferBank.Data
{
    public class ContextoBanco : DbContext
    {
        public ContextoBanco(DbContextOptions<ContextoBanco> options) : base(options) { }

        public DbSet<nuevaCuenta> Cuentas { get; set; }
        public DbSet<Transferencia> Transferencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transferencia>()
                .HasOne(t => t.CuentaOrigen)
                .WithMany(c => c.TransferenciasEnviadas)
                .HasForeignKey(t => t.CuentaOrigenId);

            modelBuilder.Entity<Transferencia>()
                .HasOne(t => t.CuentaDestino)
                .WithMany(c => c.TransferenciasRecibidas)
                .HasForeignKey(t => t.CuentaDestinoId);
        }
    }
}

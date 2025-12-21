using ApiTransferBank.Data;
using ApiTransferBank.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTransferBank.Repository
{
    public class RepositorioBancario : IRepositorioBancario
    {
        private readonly ContextoBanco _contexto;

        public RepositorioBancario(ContextoBanco contexto)
        {
            _contexto = contexto;
        }
        public async Task<List<Cuenta>> ObtenerTodasAsync()
        {
            return await _contexto.Cuentas.ToListAsync();
        }

        // Buscar cuenta por su número de cuenta (String ID)
        public async Task<Cuenta?> ObtenerPorIdAsync(string numeroCuenta)
        {
            return await _contexto.Cuentas
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }

        // Buscar para el Login
        public async Task<Cuenta?> ObtenerPorCredencialesAsync(string usuario, string password)
        {
            return await _contexto.Cuentas
                .FirstOrDefaultAsync(c => c.Usuario == usuario && c.Password == password);
        }

        public async Task<bool> CrearCuentaAsync(Cuenta cuenta)
        {
            _contexto.Cuentas.Add(cuenta);
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task RegistrarTransferenciaAsync(Transferencia transferencia)
        {
            _contexto.Transferencias.Add(transferencia);
            await _contexto.SaveChangesAsync();
        }

        public async Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta)
        {
            return await _contexto.Transferencias
                .Where(t => t.CuentaOrigenId == numeroCuenta || t.CuentaDestinoId == numeroCuenta)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }

        public async Task GuardarCambiosAsync()
        {
            await _contexto.SaveChangesAsync();
        }
    }
}
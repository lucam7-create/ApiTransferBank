using ApiTransferBank.Data;
using ApiTransferBank.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTransferBank.Repository
{
    /// <summary> Repositorio que gestiona los datos en la base de datos. </summary>
    public class RepositorioBancario : IRepositorioBancario
    {
        private readonly ContextoBanco _contexto;

        /// <summary> Constructor que recibe el contexto de base de datos. </summary>
        public RepositorioBancario(ContextoBanco contexto)
        {
            _contexto = contexto;
        }

        /// <summary> Obtiene todas las cuentas registradas. </summary>
        public async Task<List<nuevaCuenta>> ObtenerTodasAsync()
        {
            return await _contexto.Cuentas.ToListAsync();
        }

        /// <summary> Busca una cuenta por su número único. </summary>
        public async Task<nuevaCuenta?> ObtenerPorIdAsync(string numeroCuenta)
        {
            return await _contexto.Cuentas
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }

        /// <summary> Busca una cuenta por usuario y contraseña (Login). </summary>
        public async Task<nuevaCuenta?> ObtenerPorCredencialesAsync(string usuario, string password)
        {
            return await _contexto.Cuentas
                .FirstOrDefaultAsync(c => c.Usuario == usuario && c.Password == password);
        }

        /// <summary> Crea una nueva cuenta y guarda los cambios. </summary>
        public async Task<bool> CrearCuentaAsync(nuevaCuenta cuenta)
        {
            _contexto.Cuentas.Add(cuenta);
            return await _contexto.SaveChangesAsync() > 0;
        }

        /// <summary> Registra un movimiento de transferencia. </summary>
        public async Task RegistrarTransferenciaAsync(Transferencia transferencia)
        {
            _contexto.Transferencias.Add(transferencia);
            await _contexto.SaveChangesAsync();
        }

        /// <summary> Obtiene el historial de envíos y recepciones de una cuenta. </summary>
        public async Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta)
        {
            return await _contexto.Transferencias
                .Where(t => t.CuentaOrigenId == numeroCuenta || t.CuentaDestinoId == numeroCuenta)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }

        /// <summary> Guarda cualquier cambio pendiente en la base de datos. </summary>
        public async Task GuardarCambiosAsync()
        {
            await _contexto.SaveChangesAsync();
        }
    }
}
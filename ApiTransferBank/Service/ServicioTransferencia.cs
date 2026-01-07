using ApiTransferBank.DTOs;
using ApiTransferBank.Interface;
using ApiTransferBank.Models;
using ApiTransferBank.Repository;

namespace ApiTransferBank.Service
{
    /// <summary> Lógica de negocio para las operaciones bancarias. </summary>
    public class ServicioTransferencia : IServicioTransferencia
    {
        private readonly IRepositorioBancario _repo;

        /// <summary> Constructor que inyecta el repositorio. </summary>
        public ServicioTransferencia(IRepositorioBancario repo)
        {
            _repo = repo;
        }

        /// <summary> Obtiene la lista de todas las cuentas registradas sin datos sensibles. </summary>
        public async Task<List<RespuestaCuenta>> ObtenerTodasLasCuentasAsync()
        {
            var cuentas = await _repo.ObtenerTodasAsync();

            return cuentas.Select(c => new RespuestaCuenta(
                c.NumeroCuenta,
                c.Titular,
                c.Saldo
            )).ToList();
        }

        /// <summary> Registra una nueva cuenta a partir de los datos de registro. </summary>
        public async Task<RespuestaCuenta> RegistrarCuentaAsync(SolicitudRegistro dto)
        {
            var nueva = new nuevaCuenta
            {
                NumeroCuenta = dto.NumeroCuenta,
                Titular = dto.Titular,
                Saldo = dto.SaldoInicial,
                Usuario = dto.Usuario,
                Password = dto.Password
            };

            await _repo.CrearCuentaAsync(nueva);
            return new RespuestaCuenta(nueva.NumeroCuenta, nueva.Titular, nueva.Saldo);
        }

        /// <summary> Valida saldos y realiza el envío de dinero entre cuentas. </summary>
        public async Task RealizarTransferenciaAsync(SolicitudTransferencia dto)
        {
            var origen = await _repo.ObtenerPorIdAsync(dto.CuentaOrigenId);
            var destino = await _repo.ObtenerPorIdAsync(dto.CuentaDestinoId);

            if (origen == null || destino == null)
                throw new InvalidOperationException("Una o ambas cuentas no existen.");

            if (origen.Saldo < dto.Monto)
                throw new InvalidOperationException("Saldo insuficiente.");

            origen.Saldo -= dto.Monto;
            destino.Saldo += dto.Monto;

            var transferencia = new Transferencia
            {
                CuentaOrigenId = dto.CuentaOrigenId,
                CuentaDestinoId = dto.CuentaDestinoId,
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow
            };

            await _repo.RegistrarTransferenciaAsync(transferencia);
            await _repo.GuardarCambiosAsync();
        }

        /// <summary> Obtiene todos los movimientos realizados por una cuenta. </summary>
        public async Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta)
        {
            return await _repo.ObtenerHistorialAsync(numeroCuenta);
        }
    }
}
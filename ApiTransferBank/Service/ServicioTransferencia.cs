using ApiTransferBank.DTOs;
using ApiTransferBank.Models;
using ApiTransferBank.DTOs;
using ApiTransferBank.Models;
using ApiTransferBank.Repository;

namespace ApiTransferBank.Service
{
    public class ServicioTransferencia : IServicioTransferencia
    {
        private readonly IRepositorioBancario _repo;

        public ServicioTransferencia(IRepositorioBancario repo)
        {
            _repo = repo;
        }
        // Dentro de la clase ServicioTransferencia, agrega este método:

        public async Task<List<RespuestaCuenta>> ObtenerTodasLasCuentasAsync()
        {
            // Usamos el repositorio para traer todas las cuentas de la DB
            var cuentas = await _repo.ObtenerTodasAsync();

            // Convertimos las cuentas a DTOs para no mostrar contraseñas
            return cuentas.Select(c => new RespuestaCuenta(
                c.NumeroCuenta,
                c.Titular,
                c.Saldo
            )).ToList();
        }

        // 1. Registrar Cuenta
        public async Task<RespuestaCuenta> RegistrarCuentaAsync(SolicitudRegistro dto)
        {
            var nueva = new Cuenta
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

        // 2. Realizar Transferencia
        public async Task RealizarTransferenciaAsync(SolicitudTransferencia dto)
        {
            var origen = await _repo.ObtenerPorIdAsync(dto.CuentaOrigenId);
            var destino = await _repo.ObtenerPorIdAsync(dto.CuentaDestinoId);

            if (origen == null || destino == null)
                throw new InvalidOperationException("Una o ambas cuentas no existen.");

            if (origen.Saldo < dto.Monto)
                throw new InvalidOperationException("Saldo insuficiente para realizar la operación.");

            // Lógica contable
            origen.Saldo -= dto.Monto;
            destino.Saldo += dto.Monto;

            // Registrar el movimiento
            var transferencia = new Transferencia
            {
                CuentaOrigenId = dto.CuentaOrigenId,
                CuentaDestinoId = dto.CuentaDestinoId,
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow
            };

            await _repo.RegistrarTransferenciaAsync(transferencia);
            await _repo.GuardarCambiosAsync(); // Asegura que los saldos se actualicen
        }

        // 3. Obtener Historial (ESTE ES EL MÉTODO QUE TE FALTABA)
        public async Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta)
        {
            return await _repo.ObtenerHistorialAsync(numeroCuenta);
        }
    }
}
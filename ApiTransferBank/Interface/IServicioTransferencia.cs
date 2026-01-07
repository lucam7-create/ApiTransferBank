using ApiTransferBank.DTOs;
using ApiTransferBank.DTOs;
using ApiTransferBank.Models;

namespace ApiTransferBank.Interface
{
    /// <summary> Interfaz para el servicio de operaciones bancarias. </summary>
    public interface IServicioTransferencia
    {
        /// <summary> Registra una cuenta nueva. </summary>
        Task<RespuestaCuenta> RegistrarCuentaAsync(SolicitudRegistro dto);

        /// <summary> Mueve dinero entre cuentas. </summary>
        Task RealizarTransferenciaAsync(SolicitudTransferencia dto);

        /// <summary> Obtiene los movimientos de una cuenta. </summary>
        Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta);

        /// <summary> Lista todas las cuentas disponibles. </summary>
        Task<List<RespuestaCuenta>> ObtenerTodasLasCuentasAsync();
    }
}

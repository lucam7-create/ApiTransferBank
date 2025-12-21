using ApiTransferBank.DTOs;

namespace ApiTransferBank.Service
{
    public interface IServicioTransferencia
    {
        // Registra una nueva cuenta y devuelve los datos básicos creados
        Task<RespuestaCuenta> RegistrarCuentaAsync(SolicitudRegistro dto);

        // Realiza el movimiento de dinero entre dos números de cuenta (strings)
        Task RealizarTransferenciaAsync(SolicitudTransferencia dto);

        // Obtiene la lista de movimientos asociados a un número de cuenta
        Task<List<ApiTransferBank.Models.Transferencia>> ObtenerHistorialAsync(string numeroCuenta);
        Task<List<RespuestaCuenta>> ObtenerTodasLasCuentasAsync();
    }
}

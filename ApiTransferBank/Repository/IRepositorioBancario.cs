using ApiTransferBank.Models;

namespace ApiTransferBank.Repository
{
    public interface IRepositorioBancario
    {
        Task<Cuenta?> ObtenerPorIdAsync(string numeroCuenta); // Ahora usa string
        Task<Cuenta?> ObtenerPorCredencialesAsync(string usuario, string password);
        Task<bool> CrearCuentaAsync(Cuenta cuenta);
        Task GuardarCambiosAsync();
        Task RegistrarTransferenciaAsync(Transferencia transferencia);
        Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta);
        Task<List<Cuenta>> ObtenerTodasAsync();
       
    }
}
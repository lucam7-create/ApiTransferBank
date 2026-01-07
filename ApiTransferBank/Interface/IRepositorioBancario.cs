using ApiTransferBank.Models;

public interface IRepositorioBancario
{
    Task<List<nuevaCuenta>> ObtenerTodasAsync();
    /// <summary> Busca una cuenta por su número. </summary>
    Task<nuevaCuenta?> ObtenerPorIdAsync(string numeroCuenta);

    /// <summary> Busca una cuenta para validar el Login. </summary>
    Task<nuevaCuenta?> ObtenerPorCredencialesAsync(string usuario, string password);

    /// <summary> Guarda una nueva cuenta en la base de datos. </summary>
    Task<bool> CrearCuentaAsync(nuevaCuenta cuenta);

    /// <summary> Confirma los cambios realizados. </summary>
    Task GuardarCambiosAsync();

    /// <summary> Guarda un nuevo envío de dinero. </summary>
    Task RegistrarTransferenciaAsync(Transferencia transferencia);

    /// <summary> Lista de movimientos de una cuenta. </summary>
    Task<List<Transferencia>> ObtenerHistorialAsync(string numeroCuenta);
    
}
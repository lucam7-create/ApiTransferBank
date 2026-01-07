namespace ApiTransferBank.DTOs
{
    /// <summary> Datos para registrar un nuevo usuario. </summary>
    public record SolicitudRegistro(string Titular, decimal SaldoInicial, string Usuario, string Password, string NumeroCuenta);

    /// <summary> Datos para iniciar sesión. </summary>
    public record SolicitudLogin(string Usuario, string Password);

    /// <summary> Datos necesarios para mover dinero entre cuentas. </summary>
    public record SolicitudTransferencia(string CuentaOrigenId, string CuentaDestinoId, decimal Monto);

    /// <summary> Información de la cuenta que se envía al cliente. </summary>
    public record RespuestaCuenta(string NumeroCuenta, string Titular, decimal Saldo);
}
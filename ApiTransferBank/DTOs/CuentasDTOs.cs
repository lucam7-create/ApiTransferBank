namespace ApiTransferBank.DTOs
{
    public record SolicitudRegistro(string Titular, decimal SaldoInicial, string Usuario, string Password, string NumeroCuenta);
    public record SolicitudLogin(string Usuario, string Password);
    public record SolicitudTransferencia(string CuentaOrigenId, string CuentaDestinoId, decimal Monto);
    public record RespuestaCuenta(string NumeroCuenta, string Titular, decimal Saldo);
}

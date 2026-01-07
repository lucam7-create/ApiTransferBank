using ApiTransferBank.Models;

public class Transferencia
{
    /// <summary> ID único del registro. </summary>
    public int Id { get; set; }

    /// <summary> Cantidad de dinero enviada. </summary>
    public decimal Monto { get; set; }

    /// <summary> Cuándo se hizo el movimiento. </summary>
    public DateTime Fecha { get; set; }

    /// <summary> Número de cuenta que envía. </summary>
    public string CuentaOrigenId { get; set; } = string.Empty;

    /// <summary> Acceso a los datos de la cuenta origen. </summary>
    public nuevaCuenta CuentaOrigen { get; set; } = null!;

    /// <summary> Número de cuenta que recibe. </summary>
    public string CuentaDestinoId { get; set; } = string.Empty;

    /// <summary> Acceso a los datos de la cuenta destino. </summary>
    public nuevaCuenta CuentaDestino { get; set; } = null!;
}
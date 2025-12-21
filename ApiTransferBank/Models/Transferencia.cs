namespace ApiTransferBank.Models
{
    public class Transferencia
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        // Las llaves foráneas ahora son string para coincidir con NumeroCuenta
        public string CuentaOrigenId { get; set; } = string.Empty;
        public Cuenta CuentaOrigen { get; set; } = null!;

        public string CuentaDestinoId { get; set; } = string.Empty;
        public Cuenta CuentaDestino { get; set; } = null!;
    }
}
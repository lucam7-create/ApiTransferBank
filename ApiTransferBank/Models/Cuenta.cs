using System.ComponentModel.DataAnnotations;

namespace ApiTransferBank.Models
{
    public class Cuenta
    {
        [Key] // El número de cuenta es ahora la llave primaria
        public string NumeroCuenta { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<Transferencia> TransferenciasEnviadas { get; set; } = new();
        public List<Transferencia> TransferenciasRecibidas { get; set; } = new();
    }
}

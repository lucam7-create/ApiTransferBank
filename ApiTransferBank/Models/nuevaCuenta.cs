using System.ComponentModel.DataAnnotations;

namespace ApiTransferBank.Models
{
    public class nuevaCuenta
    {
        /// <summary>
        /// Representa los datos necesarios para una cuenta bancaria.
        /// </summary>
        [Key] 
        public string NumeroCuenta { get; set; } = string.Empty;
        public string Titular { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Representa una lista de transferencias enviadas desde esta cuenta.
        /// </summary>
        public List<Transferencia> TransferenciasEnviadas { get; set; } = new();
        /// <summary>
        /// Representa una lista de transferencias recibidas por esta cuenta.
        /// </summary>
        public List<Transferencia> TransferenciasRecibidas { get; set; } = new();
    }
}

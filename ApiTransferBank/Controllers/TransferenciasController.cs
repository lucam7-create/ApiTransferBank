using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiTransferBank.DTOs;
using ApiTransferBank.Service;

namespace ApiTransferBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciasController : ControllerBase
    {
        private readonly IServicioTransferencia _servicio;

        public TransferenciasController(IServicioTransferencia servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// Registra una nueva cuenta bancaria.
        /// </summary>
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] SolicitudRegistro solicitud)
        {
            try
            {
                var resultado = await _servicio.RegistrarCuentaAsync(solicitud);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


        /// <summary>
        /// Lista todas las cuentas registradas (Útil para pruebas).
        /// </summary>
        [HttpGet("listar-cuentas")]
        public async Task<IActionResult> ListarCuentas()
        {
            var cuentas = await _servicio.ObtenerTodasLasCuentasAsync();
            return Ok(cuentas);
        }

        /// <summary>
        /// Realiza una transferencia entre cuentas. Requiere Token JWT.
        /// </summary>
        [Authorize]
        [HttpPost("transferir")]
        public async Task<IActionResult> Transferir([FromBody] SolicitudTransferencia solicitud)
        {
            try
            {
                await _servicio.RealizarTransferenciaAsync(solicitud);
                return Ok(new { mensaje = "Transferencia realizada con éxito" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado en el servidor");
            }
        }

        /// <summary>
        /// Obtiene el historial de movimientos de una cuenta por su número.
        /// </summary>
        [HttpGet("historial/{numeroCuenta}")]
        public async Task<IActionResult> VerHistorial(string numeroCuenta)
        {
            var historial = await _servicio.ObtenerHistorialAsync(numeroCuenta);

            if (historial == null || historial.Count == 0)
                return NotFound(new { mensaje = "No se encontraron movimientos para esta cuenta" });

            return Ok(historial);
        }
    }
}

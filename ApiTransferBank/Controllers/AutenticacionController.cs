using ApiTransferBank.DTOs;
using ApiTransferBank.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTransferBank.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar la autenticación de usuarios 
    /// y la generación de tokens de seguridad JWT.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IRepositorioBancario _repo;
        private readonly IConfiguration _config;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="AutenticacionController"/>.
        /// </summary>
        /// <param name="repo">Repositorio para el acceso a datos bancarios.</param>
        /// <param name="config">Configuración de la aplicación para obtener claves de seguridad.</param>
        public AutenticacionController(IRepositorioBancario repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        /// <summary>
        /// Autentica a un usuario y genera un token JWT si las credenciales son válidas.
        /// </summary>
        /// <param name="login">Objeto DTO que contiene el usuario y la contraseña.</param>
        /// <returns>
        /// Un objeto JSON con el token generado, el nombre del titular y el número de cuenta si tiene éxito; 
        /// de lo contrario, devuelve un estado 401 Unauthorized.
        /// </returns>
        /// <response code="200">Devuelve el token de acceso para las peticiones subsecuentes.</response>
        /// <response code="401">Si las credenciales proporcionadas son incorrectas.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SolicitudLogin login)
        {
            var cuenta = await _repo.ObtenerPorCredencialesAsync(login.Usuario, login.Password);

            if (cuenta == null)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

            // GENERACIÓN DEL TOKEN
            var jwtKey = _config["Jwt:Key"] ?? "Clave_Super_Secreta_De_Seguridad_2025_Bank";
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // Se utiliza el NumeroCuenta como identificador único en los Claims
                    new Claim(ClaimTypes.NameIdentifier, cuenta.NumeroCuenta),
                    new Claim(ClaimTypes.Name, cuenta.Titular)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                titular = cuenta.Titular,
                numeroCuenta = cuenta.NumeroCuenta
            });
        }
    }
}

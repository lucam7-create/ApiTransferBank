using ApiTransferBank.DTOs;
using ApiTransferBank.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTransferBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IRepositorioBancario _repo;
        private readonly IConfiguration _config;

        public AutenticacionController(IRepositorioBancario repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

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
                    // AQUÍ ESTABA EL ERROR: Cambiamos .Id por .NumeroCuenta
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
                numeroCuenta = cuenta.NumeroCuenta // Devolvemos el número de cuenta también
            });
        }
    }
}

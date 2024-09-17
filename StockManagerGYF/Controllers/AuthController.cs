using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StockManagerGYF.Data.Repositories;
using StockManagerGYF.DTOs;
using StockManagerGYF.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace StockManagerGYF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthController(IConfiguration config, IUsuarioRepository userAuth)
        {
            _config = config;
            _usuarioRepository = userAuth;
        }

        [HttpPost("accesoUsuario")]
        public async Task<IActionResult> AccesoUsuario([FromBody] LoginDTO login)
        {
            login.contrasenia = HashPassword(login.contrasenia);
            var usuario = await AutenticarUsuario(login); // Espera la respuesta asincrónica

            if (usuario != null)
            {
                var token = GenerarJWT(usuario); // Genera el token
                return Ok(new { token });

            }

            return Unauthorized();
        }

        [HttpPost("registrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] LoginDTO usuario)
        {
            usuario.contrasenia = HashPassword(usuario.contrasenia);
            var registrado = await _usuarioRepository.registrarUsuario(usuario.nombre, usuario.contrasenia);

            if (registrado)
            {
                return Ok(new { message = "Usuario registrado exitosamente" });
            }

            return BadRequest(new { message = "Error al registrar el usuario" });
        }

        private async Task<Usuario> AutenticarUsuario(LoginDTO login)
        {
            var usuario = await _usuarioRepository.autenticarUsuario(login.nombre, login.contrasenia);
            return usuario;
        }

        private string GenerarJWT(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StockManagerGYF3000StockManagerGYF3000StockManagerGYF3000333"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.nombre),
            new Claim("Id", usuario.id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:7200/",
                audience: "http://localhost:7200/",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}

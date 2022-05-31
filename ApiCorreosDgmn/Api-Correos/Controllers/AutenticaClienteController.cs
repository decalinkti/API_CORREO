using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api_Correos.Models;
using Api_Correos.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api_Correos.Controllers
{
    /// <summary>
    /// Controlador que gestiona la autenticación de esta capa de negocio mediante JWT.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AutenticaClienteController : ControllerBase
    {
        private readonly MyConfig config;

        /// <summary>
        /// <param name="configu">Inyecta los datos del archivo de configuración.</param>
        /// </summary>
        public AutenticaClienteController(MyConfig configu)
        {
            config = configu;
        }

        /// <summary>
        /// Autentica con las demás capas de la aplicación
        /// </summary>
        /// <param name="nombreAplicacion">Nombre de la capa</param>
        /// <param name="key">Contraseña de la capa</param>
        /// <returns>Acción</returns>
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Index(String nombreAplicacion, String key)
        {
            var _clientInfo = AutenticarApp(nombreAplicacion, key);
            if (_clientInfo != null)
            {
                return Ok(new { token = GenerateJSONWebToken() });
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Método auxiliar para autenticar un cliente API
        /// </summary>
        /// <param name="usuario">Nombre del cliente API</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Objeto de Cliente</returns>
        private clienteAPI AutenticarApp(string usuario, string password)
        {
            if (usuario.Equals(config.ClienteApiCorreo.nombreAplicacion) && password.Equals(config.ClienteApiCorreo.key))
            {
                return new clienteAPI()
                {
                    nombreAplicacion = usuario,
                    key = password
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Método auxiliar para generar el token de autenticación
        /// </summary>
        /// <returns></returns>
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JWT.ClaveSecreta));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config.JWT.Issuer,
                audience: config.JWT.Audience,
                signingCredentials: credentials
                );

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
            return stringToken;
        }
    }
}

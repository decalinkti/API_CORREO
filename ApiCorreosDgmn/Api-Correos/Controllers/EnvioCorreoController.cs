using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api_Correos.Models;
using Api_Correos.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Api_Interoperabilidad.ServicioEmail;

namespace Api_Correos.Controllers
{
    /// <summary>
    /// Controlador para gestionar los datos del usuario relacionados al ingreso, a la edición de datos personales y al registro de nuevo usuario.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EnvioCorreoController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly MyConfig config;
        private readonly ILogger<EnvioCorreoController> _logger;
        static HttpClientHandler httpHandler;

        /// <summary>
        /// Constructor de controlador para el control de acceso y extracción de datos cuando una persona ingresa.
        /// </summary>
        /// <param name="emailSender">Inyecta el objeto que tiene los métodos para el envío de emails.</param>
        /// <param name="configu">Inyecta los datos del archivo de configuración.</param>
        /// <param name="logger">Inyecta objeto que permite guardar mensajes en un archivo de log.</param>
        public EnvioCorreoController(IEmailSender emailSender, MyConfig configu, ILogger<EnvioCorreoController> logger)
        {
            _emailSender = emailSender;
            config = configu;
            _logger = logger;

            httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        }
        
        /// <summary>
        /// Método para almacenar el correo de un usuario en base de datos y enviar correo de segunda clave
        /// </summary>
        /// <param name="datos">Objeto con datos del usuario</param>
        /// <returns>string de confirmación</returns>
        [HttpPost("EnviaMail")]
        public async Task<string> EnviaMail([FromBody] SesionUsuario datos)
        {
            _logger.LogInformation("POST EnviaMail en LoginController");
            //Consulta si tiene que validar la clave 2
            HttpClient client2 = new HttpClient(httpHandler);
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.TokenPersistencia);
            var stringTask2 = client2.GetStringAsync(config.URLPersistencia + ":" + config.PuertoAPIPersistencia + string.Format("/login/obtenerParametro?cod_parametro={0}", 1));
            String valorParametro = await stringTask2;

            if (!valorParametro.Trim().Equals("0"))
            {
                string nombreCompleto = datos.nombres + " " + datos.apellidoPaterno + " " + datos.apellidoMaterno;
                string html = String.Format(@"<!DOCTYPE html>
                                            <html lang='en'>
                                            <head>
                                                <meta charset='UTF-8'>
                                                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                                <meta name='viewport' content='width=device-width, initial-scale=1'>
                                                <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css'>
                                                <script src='https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js'></script>
                                                <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js'></script>
                                                <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js'></script>
                                            </head>
                                            <body>
    
                                            <p> </p>
                                            <table border='0' width='100%' cellspacing='0' cellpadding='0'>
                                            <tbody>
                                            <tr>
                                            <td style='padding: 10px 0 30px 0;'>
                                            <table style='border: 1px solid #dceaf5; border-collapse: collapse;' border='0' width='600' cellspacing='0' cellpadding='0' align='center'>
                                            <tbody>
                                            <tr>
                                            <td style='padding: 30px 0 20px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif; border-bottom: 5px solid #0463ac;' align='center' bgcolor='#e1e3e5'> <img src='https://dgmn.cerofilas.gob.cl/logos/dgmn_logo1.png' alt='' border='0' width='15%' ></a></td>
                                            </tr>
                                            <tr>
                                            <td style='padding: 40px 30px 40px 30px;' bgcolor='#ffffff'>
                                            <table border='0' width='100%' cellspacing='0' cellpadding='0'>
                                            <tbody>
                                            <tr>
                                            <td style='color: #2e54a7; font-family: Open Sans; font-size: 16px; -webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale;'>
                                               <strong>Estimado {0}:</strong>
                                            </td>
                                            </tr>
                                            <tr>
                                            <td style='padding: 20px 0 30px 0; color: #546575; font-family: Open Sans; font-size: 14px; font-weight: 500; line-height: 30px; -webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale;'>
   
  	
                                              <p style='color:black'> Para ingresar al portal de consultas de la Dirección General de Movilización Nacional,
                                              utilice la segunda contraseña que se muestra a continuación: </p>
                                              <div style = 'text-align: center;'>
                                                  <span style = 'font-size:22;font-weight:bold;color:#2e54a7;' > {1} </span>
                                              </div>
  

                                               <p style='font-size: 13px;'>Esta contraseña es válida para 1 uso.</p>

                                            </td>
                                            </tr>

                                            </tbody>
                                            </table>
                                            </td>
                                            </tr>
                                            <tr>
                                            <td style='padding: 10px 20px 10px 30px; font-family: Open Sans; font-size: 11px; color: #8a9ca2; text-align: center;' bgcolor='#dde5e6'>Este correo ha sido enviado de manera automática, favor no contestar al remitente.</td>
                                            </tr>
                                            </tbody>
                                            </table>
                                            </td>
                                            </tr>
                                            </tbody>
                                            </table>
                                            <p> </p>

                                            </body>
                                            </html>", nombreCompleto, datos.clave2);

                var message = new Message(new string[] { datos.correo }, "Clave 2 - Portal Consulta PAEx", html);
                _emailSender.SendEmail(message);
            }
            return "ok";
        }

        [HttpPost("EnvioCorreoDGMNTipo")]
        [Authorize]
        public string EnvioCorreoDGMNTipo([FromBody] ObjetoEnvioCorreo objetoEnvioCorreo)
        {
            //Envía correo
            _logger.LogInformation($"Se enviará un correo con las siguientes características\n:- Tipo: {(objetoEnvioCorreo.tipoEnvio==1?"(1) Gmail":"(2)Office")} \n - Destinatario: {objetoEnvioCorreo.mailReceptor}\n - Emisor: {objetoEnvioCorreo.fromName}\n - Título: {objetoEnvioCorreo.subject}");
            try
            {
                var message = new Message(new string[] { objetoEnvioCorreo.mailReceptor }, objetoEnvioCorreo.subject, objetoEnvioCorreo.htmlBody);
                _emailSender.SendEmailGenericType(message, objetoEnvioCorreo.fromName, objetoEnvioCorreo.tipoEnvio);
                return "OK";
            }
            catch (Exception e)
            {
                _logger.LogError("Ha ocurrido un error. Detalle:\n " + e);
                return "NOOK";
            }
        }
    }
}

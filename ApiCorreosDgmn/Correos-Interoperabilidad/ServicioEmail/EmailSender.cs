using MimeKit;
using MailKit.Net.Smtp;
using System;
using Microsoft.Extensions.Logging;

namespace Api_Interoperabilidad.ServicioEmail
{
    /// <summary>
    /// Clase para con los métodos del módulo de envío de emails.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly EmailConfigurationGmail _emailConfigGmail;
        private readonly EmailConfigurationOffice _emailConfigOffice;
        private readonly ILogger<EmailSender> _logger;

        /// <summary>
        /// Constructor de este controlador.
        /// </summary>
        /// <param name="emailConfig">Inyecta el objeto de configuración para envío de email.</param>
        public EmailSender(EmailConfiguration emailConfig, EmailConfigurationGmail emailConfigGmail, EmailConfigurationOffice emailConfigOffice, ILogger<EmailSender> logger)
        {
            _emailConfig = emailConfig;
            _emailConfigGmail = emailConfigGmail;
            _emailConfigOffice = emailConfigOffice;
            _logger = logger;
        }

        /// <summary>
        /// Metodo privado del paquete MailKit para crear el email.
        /// </summary>
        /// <param name="message">Objeto con el cuerpo del mensaje que se va a enviar.</param>
        /// <returns>El objeto que contiene el cuerpo de email y sus metadatos.</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text =  message.Content };
            return emailMessage;
        }

        private MimeMessage CreateEmailMessageGeneric(Message message,string fromName)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromName, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        private MimeMessage CreateEmailMessageGenericType(Message message, string fromName, int tipoEnvio)
        {
            var emailMessage = new MimeMessage();
            //Google CA
            if (tipoEnvio == 1)
            {
                emailMessage.From.Add(new MailboxAddress(fromName, _emailConfigGmail.From));
            }
            //Office CA
            else if (tipoEnvio == 2) 
            {
                emailMessage.From.Add(new MailboxAddress(fromName, _emailConfigOffice.From));
            }
            
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }

        /// <summary>
        /// Metodo privado del paquete MailKit para enviar el email.
        /// </summary>
        /// <param name="mailMessage"> Objeto con toda la estructura del email y sus metadatos.</param>
        private void Send(MimeMessage mailMessage)
        {
            _logger.LogInformation("Se ha ingresado al método Send en EmailSender");
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError("Se ha producido un error al ingresar el correo");
                    _logger.LogError(e.ToString());
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Metodo privado del paquete MailKit para enviar el email.
        /// </summary>
        /// <param name="mailMessage"> Objeto con toda la estructura del email y sus metadatos.</param>
        private void Sendbytype(MimeMessage mailMessage, int tipoEnvio)
        {
            _logger.LogInformation("Se ha ingresado al método Send en EmailSenderbyType");
            using (var client = new SmtpClient())
            {
                try
                {
                    if (tipoEnvio == 1)
                    {
                        client.Connect(_emailConfigGmail.SmtpServer, _emailConfigGmail.Port, false);
                        //client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_emailConfigGmail.UserName, _emailConfigGmail.Password);
                    }
                    else if (tipoEnvio == 2) 
                    {
                        client.Connect(_emailConfigOffice.SmtpServer, _emailConfigOffice.Port, false);
                        //client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_emailConfigOffice.UserName, _emailConfigOffice.Password);
                    }
                    
                    client.Send(mailMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError("Se ha producido un error al ingresar el correo");
                    _logger.LogError(e.ToString());
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Metodo del paquete MailKit que realiza la acción de crear el email y de enviarlo.
        /// </summary>
        /// <param name="message">Objeto con el cuerpo del mensaje.</param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        /// <summary>
        /// Metodo del paquete MailKit que realiza la acción de crear el email y de enviarlo.
        /// </summary>
        /// <param name="message">Objeto con el cuerpo del mensaje.</param>
        public void SendEmailGeneric(Message message, string fromName)
        {
            var emailMessage = CreateEmailMessageGeneric(message, fromName);
            Send(emailMessage);
        }

        /// <summary>
        /// Metodo del paquete MailKit que realiza la acción de crear el email y de enviarlo.
        /// </summary>
        /// <param name="message">Objeto con el cuerpo del mensaje.</param>
        public void SendEmailGenericType(Message message, string fromName, int tipoEnvio)
        {
            var emailMessage = CreateEmailMessageGenericType(message, fromName,tipoEnvio);
            Sendbytype(emailMessage,tipoEnvio);
        }
    }
}
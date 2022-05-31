

namespace Api_Interoperabilidad.ServicioEmail
{
    /// <summary>
    /// Clase para almacenar datos necesarios en el envío de email.
    /// </summary>
    public class EmailConfigurationGmail
    {
        public string From { get; set; }
        public string FromName { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}

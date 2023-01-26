using MimeKit;
using System.Collections.Generic;
using System.Linq;


namespace Api_Interoperabilidad.ServicioEmail
{
    /// <summary>
    /// Clase con los datos que debe contener el cuerpo de un email.
    /// </summary>
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// Constructor de 
        /// </summary>
        /// <param name="to">A quien va enviado el correo</param>
        /// <param name="subject">Asunto del correo</param>
        /// <param name="content">Contenido del correo</param>
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
        }

    }
}

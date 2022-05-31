

namespace Api_Interoperabilidad.ServicioEmail
{
    /// <summary>
    /// Interfaz de la clase EmailSender que se ocupa de realizar la acción de enviar el email.
    /// </summary>
    public interface IEmailSender
    {
        void SendEmail(Message message);
        void SendEmailGeneric(Message message, string fromName);
        void SendEmailGenericType(Message message, string fromName, int tipoEnvio);
    }
}

using System;


namespace Api_Correos.Models
{
    /// <summary>
    /// Sección de AES del json de configuración : clase hija de la clase config de Configuración del sistema
    /// </summary>
    public class ObjetoEnvioCorreo
    {
        public String fromName { get; set; }
        public String subject { get; set; }
        public String mailReceptor { get; set; }
        public String htmlBody { get; set; }
        public int tipoEnvio { get; set; }

    }
}

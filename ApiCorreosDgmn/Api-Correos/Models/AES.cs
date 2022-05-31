using System;


namespace Api_Correos.Models
{
    /// <summary>
    /// Sección de AES del json de configuración : clase hija de la clase config de Configuración del sistema
    /// </summary>
    public class AES
    {
        public String key { get; set; }
        public String iv { get; set; }

    }
}

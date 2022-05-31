using Api_Correos.Models;


namespace Api_Correos.Util
{
    /// <summary>
    /// Objeto de configuración de la aplicación
    /// </summary>
    public class MyConfig
    {
        public int PuertoAPIPersistencia { get; set; }
        public int PuertoAPIPDFServer { get; set; }
        public string URLPersistencia { get; set; }
        public string URLPDFServer { get; set; }
        public clienteAPI ClienteApiCorreo { get; set; }
        public AES AES { get; set; }
        public JWT JWT { get; set; }
        public string TokenPersistencia { get; set; }
        public string TokenPDFServer { get; set; }
    }
}

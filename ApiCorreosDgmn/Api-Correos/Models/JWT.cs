
namespace Api_Correos.Models
{
    /// <summary>
    /// Sección de configuración con datos de configuración de JWT
    /// </summary>
    public class JWT
    {
        public string ClaveSecreta { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}

using System;


namespace Api_Correos.Models
{
    /// <summary>
    /// Modelo para almacenar datos de una acreditación que se muestran en la capa web.
    /// </summary>
    public class AcrediPersonaModel
    {
        public int cod_acred { get; set; }
        public String tipo_arma_acred { get; set; }
        public DateTime fecha_renovacion_acred { get; set; }
        public DateTime fecha_inicio_acred { get; set; }
        public DateTime fecha_termino_acred { get; set; }
        public String observacion_acred { get; set; }
        public String aprobada_acred { get; set; }
        public String vigente_acred { get; set; }
    }
}

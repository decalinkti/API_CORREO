using System;

namespace Api_Correos.Models
{
    /// <summary>
    /// Modelo que gestiona los datos del usuario logueado para visualizarlo en las vistas. de la capa web.
    /// </summary>
    public class SesionUsuario
    {
        /// <summary>
        /// Constructor de la clase SesionUsuario que carga los datos asociados a un usuario.
        /// </summary>
        /// <param name="loginSesion">Datos personales del usuario logueado.</param>
        public SesionUsuario(SesionUsuario loginSesion)
        {
            if (loginSesion != null)
            {
                this.tipoUsuario = loginSesion.tipoUsuario;
                this.clave2 = loginSesion.clave2;
                this.nombres = loginSesion.nombres;
                this.nombreCorto = loginSesion.nombres.Substring(0, loginSesion.nombres.IndexOf(" "));
                this.cod_login = loginSesion.cod_login;
                this.apellidoPaterno = loginSesion.apellidoPaterno;
                this.apellidoMaterno = loginSesion.apellidoMaterno;
                this.rut = loginSesion.rut;
                this.coleccionistaDepor = loginSesion.coleccionistaDepor;
                this.fechaNacimiento = loginSesion.fechaNacimiento;
                this.fechaUltimoMovimiento = loginSesion.fechaUltimoMovimiento;
                this.sexo = loginSesion.sexo;
                this.telefono = loginSesion.telefono;
                this.correo = loginSesion.correo;
                this.comuna = loginSesion.comuna;
                this.pais = loginSesion.pais;
                this.region = loginSesion.region;
                this.calleDir = loginSesion.calleDir;
                this.numeroDir = loginSesion.numeroDir;
                this.complementoDir = loginSesion.complementoDir;
                this.esUrbano = loginSesion.esUrbano;
                this.codigoPostal=loginSesion.codigoPostal;
                this.fechaUltimaModificacion = loginSesion.fechaUltimaModificacion;

            }

        }

        /// <summary>
        /// Constructor vacío de la clase SesionUsuario.
        /// </summary>
        public SesionUsuario()
        {

        }

        public String rut { get; set; }
        public int  cod_login { get; set; }

        public String tipoUsuario { get; set; }
        public String clave2 { get; set; }
        public String nombres { get; set; }
        public String nombreCorto { get; set; }

        public String apellidoPaterno { get; set; }
        public String apellidoMaterno { get; set; }
        public String coleccionistaDepor { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public DateTime fechaUltimoMovimiento { get; set; }
        public String comuna { get; set; }
        public String pais { get; set; }

        public String sexo { get; set; }

        public String telefono { get; set; }

        public String correo { get; set; }

        public String region { get; set; }

        public String calleDir { get; set; }

        public int numeroDir { get; set; }

        public String complementoDir { get; set; }
    
        public Boolean esUrbano { get; set; }
        public String codigoPostal { get; set; }
        public String fechaUltimaModificacion { get; set; }

        //Datos para persona Jurídica

        public int rut_empresa { get; set; }

        public int cod_sucursal { get; set; }

        public int rut_negocio { get; set; }

        public int cod_negocio { get; set; }
    }
}

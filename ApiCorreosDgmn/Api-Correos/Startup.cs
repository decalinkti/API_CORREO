using System;
using System.Text;
using ChustaSoft.Tools.SecureConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Api_Correos.Util;
using Api_Interoperabilidad.ServicioEmail;

namespace PAEx_Negocio
{
    /// <summary>
    /// Esta clase contiene la configuración del sistema.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor de la clase Startup.
        /// </summary>
        /// <param name="configuration">Inyección de dependencias de la interfaz de sistema de configuración IConfiguration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Este método es llamado en tiempo de ejecución. Se usa para agregar servicios al contenedor.
        /// </summary>
        /// <param name="services">Inyección de dependencia de la interfaz de sistema de servicios IServiceCollection .</param>
        public void ConfigureServices(IServiceCollection services)
        {
            EmailConfiguration EmailConfig = new EmailConfiguration();
            EmailConfig.From = Environment.GetEnvironmentVariable("Email_From");
            EmailConfig.FromName = Environment.GetEnvironmentVariable("Email_FromName");
            EmailConfig.Password = Environment.GetEnvironmentVariable("Email_Password");
            EmailConfig.Port = Int32.Parse(Environment.GetEnvironmentVariable("Email_Port"));
            EmailConfig.SmtpServer = Environment.GetEnvironmentVariable("Email_SmtpServer");
            EmailConfig.UserName = Environment.GetEnvironmentVariable("Email_Username");

            EmailConfigurationGmail EmailConfigGmail = new EmailConfigurationGmail();
            EmailConfigGmail.From = Environment.GetEnvironmentVariable("Email_From_google");
            EmailConfigGmail.FromName = Environment.GetEnvironmentVariable("Email_FromName_google");
            EmailConfigGmail.Password = Environment.GetEnvironmentVariable("Email_Password_google");
            EmailConfigGmail.Port = Int32.Parse(Environment.GetEnvironmentVariable("Email_Port_google"));
            EmailConfigGmail.SmtpServer = Environment.GetEnvironmentVariable("Email_SmtpServer_google");
            EmailConfigGmail.UserName = Environment.GetEnvironmentVariable("Email_Username_google");

            EmailConfigurationOffice EmailConfigOffice = new EmailConfigurationOffice();
            EmailConfigOffice.From = Environment.GetEnvironmentVariable("Email_From_office");
            EmailConfigOffice.FromName = Environment.GetEnvironmentVariable("Email_FromName_office");
            EmailConfigOffice.Password = Environment.GetEnvironmentVariable("Email_Password_office");
            EmailConfigOffice.Port = Int32.Parse(Environment.GetEnvironmentVariable("Email_Port_office"));
            EmailConfigOffice.SmtpServer = Environment.GetEnvironmentVariable("Email_SmtpServer_office");
            EmailConfigOffice.UserName = Environment.GetEnvironmentVariable("Email_Username_office");


            services.AddSingleton(EmailConfig);
            services.AddSingleton(EmailConfigGmail);
            services.AddSingleton(EmailConfigOffice);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAESEncrytDecry, AESEncrytDecry>();


            // Add our Config object so it can be injected

            services.AddControllers();

            string key = Environment.GetEnvironmentVariable("SECRET_KEY");
            services.SetUpSecureConfig<MyConfig>(Configuration, key, "MyConfig");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_Audience"),
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_Issuer"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_ClaveSecreta")))
                };
            });
        }

        /// <summary>
        /// Este método es llamado en tiempo de ejecución. Es usado para configurar el flujo de las consultas HTTP .
        /// </summary>
        /// <param name="app">Inyección de dependencias de la interfaz de sistema IApplicationBuilder.</param>
        /// <param name="env">Inyección de dependencias de la interfaz de sistema IWebHostEnvironment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

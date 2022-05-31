
using ChustaSoft.Tools.SecureConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Api_Correos.Util;
using Serilog;

namespace PAEx_Negocio
{
    /// <summary>
    /// Clase que contiene el programa principal para ejecutar esta capa del sistema PAEx intranet.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Método principal que ejecuta el programa de esta capa.
        /// </summary>
        /// <param name="args">Argumentos de inicialización.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .EncryptSettings<MyConfig>(false, "MyConfig")
                .Run();
        }

        /// <summary>
        /// Método que inicializa el programa principal. 
        /// </summary>
        /// <param name="args">Argumentos de inicialización.</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(
                        loggingBuilder =>
                        {
                            var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json")
                               .Build();
                            var logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(configuration)
                                .CreateLogger();
                            loggingBuilder.AddSerilog(logger, dispose: true);
                        }
                  )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

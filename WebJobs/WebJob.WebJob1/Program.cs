using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WebJob.WebJob1
{
    class Program
    {
        //Declarar una variable que tenga el objeto Timer:
        private static Timer timer;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main() //PONER TODO EL JOB EN EJECUCIÓN. EN BLOQUE.
        {
            ////Inicializar la variable con el objeto:
            //timer = new Timer();
            //timer.Interval = 3000; //3000 segundos.

            ////Código a ejecutar cada esos 3000 segundos:
            //timer.Elapsed += Functions.Func1; //Método a ejecutar.

            //timer.AutoReset = true; //Resetear si hay algun problema.
            //timer.Enabled = true; //Habilitar el timer.

            var config = new JobHostConfiguration(); //Carga de la Configuración del Host -> De app.config.

            if (config.IsDevelopment) config.UseDevelopmentSettings(); //Identificar si estamos en un entorno de desarrollo.

            config.UseTimers(); //Marcar que queremos utilizar el timer.

            var host = new JobHost(config); //Crea el objeto JobHost con la configuración ->
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock(); //Lo pone en funcionamiento. LLAMADAS A FUNCIONES -> De Functions.cs
        }
    }
}

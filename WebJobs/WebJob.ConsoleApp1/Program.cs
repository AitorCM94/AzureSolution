using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WebJob.ConsoleApp1
{
    class Program
    {
        //Declarar una variable que tenga el objeto Timer:
        private static Timer timer;

        static void Main(string[] args)
        {
            //Inicializar la variable con el objeto:
            timer = new Timer();
            timer.Interval = 3000; //3000 segundos.

            //Código a ejecutar cada esos 3000 segundos:
            timer.Elapsed += Timer_Elapsed; //Métodos a ejecutar.

            timer.AutoReset = true; //Resetear si hay algun problema.
            timer.Enabled = true; //Habilitar el timer.

            //Escribir en la consola:
            Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>> {DateTime.Now}");
            Console.ReadLine(); //Para que no finalize la ejecución. Cada 30s invocará el código.
        }

        //Método:
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Marcar inicio y final de la ejecución:
            Console.WriteLine($">>> PROCESO INICIADO {DateTime.Now}");

            //Capturar los ficheros que estan en la carpeta Upload:
            var ficheros = Directory.GetFiles(@"C:\home\site\wwwroot\wwwroot\upload"); //Devuelve un array de string que contienen las direcciones de cada uno de los ficheros.
            Console.WriteLine($">{ficheros.Length} encontrados"); //Mensaje con el número de ficheros encontrados.
            foreach(var path in ficheros)
            {
                var fichero = new FileInfo(path); //Variable con el objeto FileInfo-> Nos da toda la información del fichero. Está en system.io. Lo utilizamos para obtener el nombre por separado.
                Console.WriteLine($"{fichero.FullName}");
                //Mober el fichero:
                File.Move(fichero.FullName, @"C:\home\site\wwwroot\wwwroot\process\" + fichero.Name); //Ruta origen, ruta de destino.
                Console.WriteLine($"P R O C E S A D O");
            }

            Console.WriteLine($">>> PROCESO FINALIZADO {DateTime.Now}");
        }
    }
}
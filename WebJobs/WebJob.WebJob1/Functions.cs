using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WebJob.WebJob1
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written on an Azure Queue called queue -> La que apunta en la cadna de conexión de app.config.
        //public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        //{
        //    log.WriteLine(message);
        //}

        ////Definir una función inicial -> Sin desencadenadro:
        //public static void Func1(object sender, ElapsedEventArgs e) //, TextWriter log) Instanciar el objeto que permite escribir los logs.
        //{
        //    Console.WriteLine("Func1 ejecutada correctamente."); //log.
        //}
        //Definir una función inicial -> Con desencadenadro:
        public static void Func2([TimerTrigger("*/10 * * * * *")]TimerInfo timer, TextWriter log) //Timer -> Atributo para convertirlo en un desencadenador.
        {
            log.WriteLine("Func2 ejecutada correctamente.");
        }

        public static void Func3([BlobTrigger("upload/{name}")]TextReader upload, [Blob("process/{name}")]out string process, TextWriter log)
        {
            log.WriteLine("PROCESO INICIADO");
            log.WriteLine($"...");
            process = upload.ReadToEnd();
            log.WriteLine("PROCESO FINALIZADO");
        }
    }
}

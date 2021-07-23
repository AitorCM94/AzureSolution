using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table; //Donde estan todos los objetos...

namespace FunctionApp1
{
    public class Register //Clase que representa los registros:
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }

    public static class Function1
    {
        [FunctionName("Function1")] //Decorador que marca el nombre de la funci�n.
        public static async Task<IActionResult> Run( //M�todo como retorno -> IActionResult (Interfaz): Respuesta de tipo http.
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] //Decorador que indica que es un desencadenador. M�todos de uso, etc.
            HttpRequest req, //Mensaje de petici�n.
            ILogger log, //Acceso al log -> Consola donde escribimos la informaci�n.
            //DEFINIR EL ACCESO A LA TABLA:
            [Table("operations", Connection = "AzureWebJobsStorage")] ICollector<Register> outputTable) //Apuntamos a la cadena de conexi�n para coger la tabla.
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //BOLCAR EL RESULTADO DE TODA LA INFORMACI�N EN UNA TABLA:
            //Instanciar el objeto creado:
            var register = new Register();
            //Dar valor a las propiedades:
            register.PartitionKey = "http";
            register.RowKey = Guid.NewGuid().ToString();
            register.Name = name;
            register.Message = responseMessage;
            //A�adir el registro a la tabla:
            outputTable.Add(register); //Salida de la informaci�n en la tabla.

            return new OkObjectResult(responseMessage); //Salida predeterminada -> Enviar una respuesta a trav�s del Canal web.
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace Formacion.Web.App.Controllers
{
    public class WebJobController : Controller
    {

        [BindProperty] //Marcada para bindear...
        public IFormFile UploadFileContent { get; set; } //Propiedad/variable para manejar el fichero. De tipo interfaz IFormFile.

        private IConfiguration config;

        public WebJobController(IConfiguration config) //Constructor config... Inyección de dependencias
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadFile() //Método para el Upload -> Código que permite hacer la carga del fichero.
        {
            try //Cuando hacemos cargas siempre hay que poner el try Catch.
            {
                //Comporbación inicial para saber si el fichero tiene contenido:
                if (UploadFileContent == null || UploadFileContent.Length == 0) return Content("Fichero no seleccionado.");

                //Ruta donde almacenar el fichero:
                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", UploadFileContent.FileName); //Combina los valores para crear el path.

                //Grabar el fichero dentro de la carpeta:
                var stream = new FileStream(ruta, FileMode.Create); //FileStream-> Representa un fichero. Ruta, Modo de acceder al fichero.
                UploadFileContent.CopyTo(stream);
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }

            return RedirectToAction("ficheros"); //Devolver una vista o REDIRIJIR A al terminar el método (uploadfile).
        }

        public IActionResult Ficheros()
        {
            return View();
        }

        public IActionResult UploadFile2()
        {
            try
            {
                var connectionString = config.GetSection("AzureStorageConnectionString").Value; //Tenemos la cadena de conexión. Instanciado para toda la aplicación...

                BlobServiceClient blobClientService = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobClientService.GetBlobContainerClient("upload"); //Apuntamos el contenedor.
                if (!containerClient.Exists()) { containerClient = blobClientService.CreateBlobContainer("upload"); } //Creamos el contenedor.
                BlobClient blobClient = containerClient.GetBlobClient(UploadFileContent.FileName);
                blobClient.Upload(UploadFileContent.OpenReadStream());

                return RedirectToAction("ficheros");
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
        }
    }
}

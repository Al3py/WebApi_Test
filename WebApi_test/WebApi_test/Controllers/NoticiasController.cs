using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebApi_test.Clases;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        /// <summary>
        /// Metodo que obtiene todas las noticias
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok(Noticias(false));
        //}

        /// <summary>
        /// Metodo que obtiene la lista de noticias por el filtro ingresado
        /// </summary>
        /// <param name="q">Texto a buscar en la lista de noticias</param>
        /// <param name="f">Parametro para obtener la foto y el tipo de contenido del mismo</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string? q, bool f)
        {
            try
            {
                if (string.IsNullOrEmpty(q))
                    return BadRequest(new { codigo = "g268", error = "Parámetros inválidos" });

                //var noticias = Noticias(f).FindAll(x => x.titulo.Contains(q) || x.resumen.Contains(q));
                var noticias = prueba(q, f);
                
                if (noticias?.data?.Count == 0)
                    return NotFound(new { codigo = "g267", error = $"No se encuentran noticias para el texto: {q}" });
                
                return Ok(noticias);
            }
            catch (Exception)
            {
                return StatusCode(500, new { codigo = "g100", error = "Error interno del servidor" });
            }
        }

        private DatosNoticia prueba(string busqueda, bool descargar)
        {
            string urlBusqueda = "https://www.abc.com.py/buscar/";

            var client  = new RestClient($"{urlBusqueda}{busqueda.Replace(" ","%20")}");
            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            var datos = response.Content;

            datos = datos.Substring(datos.IndexOf("globalContent=") + 14);
            datos = datos.Substring(0, datos.IndexOf(";"));

            var prueba = JsonConvert.DeserializeObject<DatosNoticia>(datos);

            if (descargar)
                foreach (var item in prueba.data)
                {
                    if (!string.IsNullOrEmpty(item.promo_items.basic.url))
                    {
                        var foto = new WebClient().DownloadData(new Uri(item.promo_items.basic.url));

                        item.ImagenBase64 = Convert.ToBase64String(foto);
                        item.ContentType  = GetContentType(item.promo_items.basic.url);
                    }
                }

            return prueba;
        }

        /// <summary>
        /// Obtiene el Content Type de un archivo.
        /// </summary>
        /// <returns></returns>
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        /// <summary>
        /// Content Types.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.ms-excel" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" },
                { ".odc", "text/x-ms-odc" },
                { ".ods", "application/oleobjec" },
                { ".rtf", "application/rtf" },
                { ".rtx", "text/richtext" }
            };
        }
    }
}
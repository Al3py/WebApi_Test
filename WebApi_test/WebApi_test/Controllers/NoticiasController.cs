using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebApi_test.Clases;

namespace WebApi_test.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private string imagen = "https://www.abc.com.py/resizer/xQOy3oSO0PH_soYZZiFQ73_fUHo=/fit-in/770x495/smart/cloudfront-us-east-1.images.arcpublishing.com/abccolor/GJTXDNXEPRAEJNMRXP2BEFM6LE.jpg";
        private string enlace = "https://www.abc.com.py/internacionales/2020/09/22/descubren-una-nueva-alteracion-en-el-cerebro-de-las-personas-con-alzheimer/";

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

                var noticias = Noticias(f).FindAll(x => x.titulo.Contains(q) || x.resumen.Contains(q));

                if (noticias?.Count == 0)
                    return NotFound(new { codigo = "g267", error = $"No se encuentran noticias para el texto: {q}" });

                return Ok(noticias);
            }
            catch (Exception)
            {
                return StatusCode(500, new { codigo = "g100", error = "Error interno del servidor" });
            }
        }

        private List<Noticia> Noticias(bool descargar)
        {
            var noticias = new List<Noticia>();

            for (int i = 0; i < 5; i++)
            {
                var foto = descargar ? new WebClient().DownloadData(new Uri(imagen)) : null;

                noticias.Add(new Noticia
                {
                    fecha             = DateTime.Now,
                    titulo            = $"Encabezado {i}",
                    resumen           = $"resumen {i}",
                    enlace_foto       = imagen,
                    contenido_foto    = descargar ? Convert.ToBase64String(foto) : string.Empty,
                    enlace            = enlace,
                    content_type_foto = descargar ? GetContentType(imagen) : string.Empty
                });
            }

            return noticias;
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
using System;
using System.Collections.Generic;

namespace WebApi_test.Clases
{
    public class DatosNoticia
    {
        public DatosNoticia()
        {
            data = new List<NoticiaLista>();
        }

        public string _id { get; set; }

        public List<NoticiaLista> data { get; set; }
    }
}

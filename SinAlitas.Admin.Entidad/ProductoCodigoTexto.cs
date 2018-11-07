using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ProductoCodigoTexto
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public bool EstaPagado { get; set; }

        public string UrlAbrir { get; set; }
        public bool MostrarAbrir { get; set; }
    }
}

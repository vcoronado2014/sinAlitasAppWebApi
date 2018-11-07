using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ClienteBusqueda
    {
        public int Id { get; set; }
        public string NombreFormateado { get; set; }
        public string Comuna { get; set; }
    }
    public class ClienteFormateado : ClienteBusqueda
    {
        public string Region { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string Direccion { get; set; }
    }
}

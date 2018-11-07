using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class CupoLiviano
    {
        public string Id { get; set; }
        public int IdCupo { get; set; }
        public int SghId { get; set; }
        public int FechaEntera { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public bool EsSeleccionado { get; set; }

        public bool EsDisponible { get; set; }
        public string Descripcion { get; set; }

        public string ClaseBorde { get; set; }
    }
    public class CupoClienteMostrar : Entidad.Cupo
    {
        public string FechaStrInicio { get; set; }
        public string FechaStrTermino { get; set; }

        public string Estado { get; set; }
        public string NombreCliente { get; set; }
        public string ClaseIcono { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class EnvoltorioAlumno : Entidad.FichaAlumno
    {
        public string DetalleProblemasCardiacos { get; set; }
        public string DetalleAsma { get; set; }
        public string DetalleProblemasMotores { get; set; }
    }
}

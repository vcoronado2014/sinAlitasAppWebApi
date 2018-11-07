using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class EnvoltorioCalificaciones : CalificacionesProfesor
    {
        public string FechaStr { get; set; }
        public string NombreCliente { get; set; }
    }
}

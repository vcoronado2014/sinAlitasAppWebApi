using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class EnvoltorioProfesor
    {
        public Profesor Profesor { get; set; }
        public Region Region { get; set; }
        public Pais Pais { get; set; }
        public Comuna Comuna { get; set; }

        public Nodo Nodo { get; set; }
        public Rol Rol { get; set; }

    }
}

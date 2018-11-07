using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class FichaPack
    {
        public int Id { get; set; }
        public int IdPack { get; set; }
        public int IdAlumno { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Activo { get; set; }
        public int Eliminado { get; set; }
    }
}

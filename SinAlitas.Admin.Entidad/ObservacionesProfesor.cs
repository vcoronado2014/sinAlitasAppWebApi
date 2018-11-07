using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ObservacionesProfesor
    {
        public int Id { get; set; }
        public string Observacion { get; set; }
        public int CpoId { get; set; }
        public int PcoId { get; set; }
    }
}

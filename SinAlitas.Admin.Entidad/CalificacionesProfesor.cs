using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class CalificacionesProfesor
    {
        public int Id { get; set; }
        public int Calificacion { get; set; }
        public string Observacion { get; set; }
        public int PcoId { get; set; }
        public DateTime Fecha { get; set; }
        public int ProfId { get; set; }
    }
}

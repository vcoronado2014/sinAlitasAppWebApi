using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class Semanas
    {
        public DateTime FechaInicioSemana { get; set; }
        public DateTime FechaTerminoSemana { get; set; }
        public string SemanaString { get; set; }
        public int DiasDisponibles { get; set; }
        public int FechaInicioEntera { get; set; }
        public int DiasAgendados { get; set; }
        public string ClaseBoton { get; set; }

        public int PuedeAgendar { get; set; }
    }
}

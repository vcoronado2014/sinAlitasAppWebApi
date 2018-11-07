using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ReporteClases
    {
        public DateTime FechaCreacion { get; set; }
        public DateTime PrimeraClase { get; set; }
        public string NombreProfesor { get; set; }
        public string Nombre { get; set; }
        public string CodigoCliente { get; set; }
        public int Pendientes { get; set; }
        public int Cerradas { get; set; }
        public int Programadas { get; set; }
        public string EstaPagado { get; set; }

    }
}

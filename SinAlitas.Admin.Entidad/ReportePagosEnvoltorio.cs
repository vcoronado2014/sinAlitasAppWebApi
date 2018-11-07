using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ReportePagosEnvoltorio
    {
        public Entidad.Profesor Profesor { get; set; }
        public List<DatosPago> DatosPack { get; set; }

    }
    public class DatosPago
    {
        public string NombreProfesor { get; set; }
        public string CodigoPack { get; set; }
        public string NombreCliente { get; set; }
        public int ClasesCerradas { get; set; }
        public int ClasesPendientes { get; set; }
        public DateTime FechaPrimeraClase { get; set; }
        public int TotalClases { get; set; }

        public string EstaPagado{ get; set; }

        public string EstiloPagado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class FichaAlumno
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public int TieneProblemasMotores { get; set; }
        public string CualesProblemasMotores { get; set; }
        public int TieneProblemasCardiacos { get; set; }
        public string CualesProblemasCardiacos { get; set; }
        public int TieneAsma { get; set; }
        public string OtraEnfermedad { get; set; }
        public string NumeroEmergencia { get; set; }
        public string DondeAcudir { get; set; }
        public string Observacion { get; set; }
    }
}

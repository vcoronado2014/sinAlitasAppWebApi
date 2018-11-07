using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class EnvoltorioProfesorCupo
    {
        public Entidad.Profesor Profesor { get; set; }

        public List<Entidad.Cupo> CuposProfesor { get; set; }

        public int EstrellasProfesor { get; set; }

        public int CantidadVotos { get; set; }
        public string ProximosDias { get; set; }
        public DateTime PrimerCupo { get; set; }

        public bool DeshabilitarControlFecha { get; set; }

        public string IdControl { get; set; }

        public List<CupoMostrarProfesor> CuposMostrar { get; set; }
        public string Comunas { get; set; }
        public string Semanas { get; set; }
        public List<Entidad.Semanas> SemanasArr { get; set; }
    }
    public class CupoMostrarProfesor
    {
        public string FechaLetras { get; set; }

        public List<Entidad.DetalleCupo> Cupos { get; set; }
        public int IdSegmento { get; set; }
        public bool Colapsado { get; set; }

        public int FechaEnteraInicio { get; set; }

        public string NombreListBox { get; set; }
    }
    public class DetalleCupo
    {
        public Entidad.Cupo Cupo { get; set; }
        public string TextoMostrar { get; set; }
        public int IdSegmento { get; set; }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ItemProfesor
    {
        public string NombreProfesor { get; set; }

        public List<ItemDetalle> Detalle { get; set; }
    }
    public class ItemDetalle
    {
        public DateTime Fecha { get; set; }
        public string SegmentoUno { get; set; }
        public string SegmentoDos { get; set; }
        public string SegmentoTres { get; set; }
        public string SegmentoCuatro { get; set; }
        public string SegmentoCinco { get; set; }
        public string SegmentoSeis { get; set; }
        public string SegmentoSiete { get; set; }
        public string SegmentoOcho { get; set; }
    }
    public class PlanillaProfesor
    {
        public int IdProfesor { get; set; }
        public string NombreProfesor { get; set; }
        public DateTime Fecha { get; set; }
        public string SegmentoUno { get; set; }
        public string SegmentoDos { get; set; }
        public string SegmentoTres { get; set; }
        public string SegmentoCuatro { get; set; }
        public string SegmentoCinco { get; set; }
        public string SegmentoSeis { get; set; }
        public string SegmentoSiete { get; set; }
        public string SegmentoOcho { get; set; }
    }
    public class ResultadosProfesor
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string CodigoCliente { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraTermino { get; set; }

    }
}

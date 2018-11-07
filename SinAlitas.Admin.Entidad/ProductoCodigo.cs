using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class ProductoCodigo
    {
        public int Id { get; set; }
        public string CodigoCliente { get; set; }
        public int ClieId { get; set; }
        public int ProId { get; set; }
        public int CantidadClases { get; set; }
        public int PorcentajeDescuento { get; set; }
        public int TotalPack { get; set; }

        public int CantidadAlumnos { get; set; }
        public int Estado { get; set; }
        public int Activo { get; set; }
        public int Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int EstaPagado { get; set; }
    }
}

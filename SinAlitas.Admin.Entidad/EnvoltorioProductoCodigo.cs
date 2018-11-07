using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class EnvoltorioProductoCodigo : Entidad.Producto
    {
        public int CantidadClases { get; set; }
        public int PorcentajeDescuento { get; set; }
        public int TotalPack { get; set; }
        public int PcoId { get; set; }
    }
}

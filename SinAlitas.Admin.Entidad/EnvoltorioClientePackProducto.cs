using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class EnvoltorioClientePackProducto
    {
        public Entidad.Cliente Cliente { get; set; }
        public Entidad.AceptaCondiciones Condiciones { get; set; }
        public Entidad.ProductoCodigo ProductoCodigo { get; set; }
        public Entidad.Comuna Comuna { get; set; }

        public List<Entidad.FichaPack> FichaPack { get; set; }

        public List<Entidad.Cupo> CuposTomados { get; set; }
    }
}

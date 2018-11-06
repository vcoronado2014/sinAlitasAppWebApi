using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class EnvoltorioProducto
    {
        public List<VCFramework.Entidad.Producto> Productos { get; set; }
        public int UltimoId { get; set; }

    }
}

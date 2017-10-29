using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class EnvoltorioFicha : FichaAlumno
    {
        public int IdFichaPack { get; set; }
        public int IdPack { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

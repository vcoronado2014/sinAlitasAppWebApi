using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class LogsSistema
    {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Mensaje { get; set; }
        public int TipoMensaje { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class AceptaCondiciones
    {
        public int Id { get; set; }
        public int PcoId { get; set; }
        public DateTime FechaAcepta { get; set; }
        public int EsAceptado { get; set; }
        public int EsCompletado { get; set; }
        public int Activo { get; set; }
    }
}

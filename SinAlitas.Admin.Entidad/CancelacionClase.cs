using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class CancelacionClase
    {
        public int Id { get; set; }
        public DateTime FechaCancelacion { get; set; }
        public int PcoId { get; set; }
        public int CpoId { get; set; }
        public int ProfId { get; set; }
        public string Motivo { get; set; }
    }
}

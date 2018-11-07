using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class SegmentoHorario
    {
        public int Id { get; set; }
        public int NodId { get; set; }
        public int DuracionAtencion { get; set; }
        public DateTime FechaHoraInicial { get; set; }
        public DateTime FechaHoraFinal { get; set; }
        public int Activo { get; set; }
        public int Eliminado { get; set; }
    }
}

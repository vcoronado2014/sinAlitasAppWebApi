using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class HistorialMovimiento
    {
        public int Id { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public int UsuarioMovimiento { get; set; }
        public int UsuarioOrigen { get; set; }
    }
}

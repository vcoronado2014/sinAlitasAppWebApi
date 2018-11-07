using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class Notificacion
    {
        public TipoNotificacion  TipoNotificacion { get; set; }
        public string FechaHora { get; set; }
        public string Detalle { get; set; }
    }
    public enum TipoNotificacion
    {
        Tarea = 1,
        Mensaje = 2,
        ClaseCancelada = 3
    }
}

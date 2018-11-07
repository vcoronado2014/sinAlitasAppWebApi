using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class Tareas : Cupo
    {
        public string FechaHoraInicioTexto { get; set; }
        public string FechaHoraTerminoTexto { get; set; }
        public string NombreCliente { get; set; }
        public string DireccionCliente { get; set; }
        public string FotoCliente { get; set; }

        public string FechaTexto { get; set; }
        public int IdPopup { get; set; }
        public string ClaseVisible { get; set; }
        public string ClaseVisibleCancelar { get; set; }
        public string ClaseVisibleReagendar { get; set; }

        public string CodigoCliente { get; set; }
        public string TelefonoCliente { get; set; }
    }
    public class TareasEnvoltorio
    {
        public int IdPopup { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaTexto { get; set; }
        public List<Tareas> TareasMostrar { get; set; }
    }
}

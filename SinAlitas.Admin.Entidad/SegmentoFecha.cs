using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class SegmentoFecha
    {
        public int Mes { get; set; }
        public int Anno { get; set; }


        public List<SegmentoDia> Dias { get; set; }
    }

    public class SegmentoMostrarFecha
    {
        public int Mes { get; set; }
        public int Anno { get; set; }


        public List<SegmentoMostrar> Dias { get; set; }
    }
    public class SegmentoDia
    {
        public DateTime Fecha { get; set; }

        public string DiaSemana { get; set; }

        public int CuposDisponibles { get; set; }

        public List<SegmentoHora> Horas { get; set; }

        public string ClaseBorde { get; set; }
        public int DiaSemanaInt { get; set; }
    }
    public class SegmentoHora
    {
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public int Estado { get; set; }

        public Entidad.Cupo Cupo { get; set; }
    }
    public class SegmentoMostrar
    {
        public DateTime Fecha { get; set; }

        public int FechaEntera { get; set; }

        public string DiaSemana { get; set; }

        public int CuposDisponibles { get; set; }

        public string ClaseBorde { get; set; }
        public int DiaSemanaInt { get; set; }

        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public int Estado { get; set; }
        public string Comentario { get; set; }

        public int NodId { get; set; }
        public int ProfId { get; set; }
        public int ClieId { get; set; }
        public int CpoId { get; set; }

        public string ClaseBotonCrearTodos { get; set; }
        public string ClaseBotonCrearUno { get; set; }
        public string ClaseBotonEliminarTodos { get; set; }
        public string ClaseBotonEliminarUno { get; set; }

        public int DeshabilitaCrearTodo { get; set; }
        public int DeshabilitaCrearUno { get; set; }
        public int DeshabilitaBorrarTodo { get; set; }
        public int DeshabilitaBorrarUno { get; set; }

        public string ClaseDia { get; set; }

        public bool EsDisponible { get; set; }
        public bool EsSeleccionado { get; set; }
        public int SghId { get; set; }
        public string NombreCliente { get; set; }
        public string DireccionCliente { get; set; }

        public string TelefonosCliente { get; set; }
        public string CorreoCliente { get; set; }

        public bool NombreClienteVisible { get; set; }
        public bool TelefonoClienteVisible { get; set; }
        public bool EmailClienteVisible { get; set; }


    }
}

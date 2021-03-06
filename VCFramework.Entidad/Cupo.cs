﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class Cupo
    {
        public int Id { get; set; }
        public int SghId { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraTermino { get; set; }
        public int EstadCupo { get; set; }
        public string Descripcion { get; set; }
        public int Eliminado { get; set; }
        public int Activo { get; set; }
        public int NodId { get; set; }
        public int ProfId { get; set; }
        public int ClieId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int PcoId { get; set; }

    }
}

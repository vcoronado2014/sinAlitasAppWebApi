using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class Envoltorio
    {
        //el envoltorio lleva todo
        public ProductoCodigo ProductoCodigo { get; set; }
        public Producto Producto { get; set; }
        public Cliente Cliente { get; set; }
        public Region RegionCliente { get; set; }
        public Comuna ComunaCliente { get; set; }
        public bool TieneAceptaCondiciones { get; set; }
        public AceptaCondiciones AceptaCondiciones { get; set; }
        public List<Entidad.Cupo> Cupos { get; set;  }
        public Entidad.Profesor Profesor { get; set; }
        public List<Entidad.EnvoltorioFicha> FichaAlumnos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public int CantidadAlumnos { get; set; }
        public int Eliminado { get; set; }
        public int Activo { get; set; }
        public int TopeClases { get; set; }
        public int ValorClase { get; set; }
    }
}

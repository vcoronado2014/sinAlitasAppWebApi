using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class UsuarioAps
    {
        public int Id { get; set; }
        public int NodId { get; set; }
        //public int FamId { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Rut { get; set; }
        public int IdRyf { get; set; }

        public int RolId { get; set; }
    }
}

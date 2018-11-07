using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Entidad
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Rut { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Fotografia { get; set; }
        public string Codigo { get; set; }
        public int PaiId { get; set; }
        public int RegId { get; set; }
        public int ComId { get; set; }
        public string Direccion { get; set; }
        public string Sexo { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public int Activo { get; set; }
        public int Eliminado { get; set; }
        public string TelefonosContacto { get; set; }
        public string Email { get; set; }

        public int NodId { get; set; }
    }
}

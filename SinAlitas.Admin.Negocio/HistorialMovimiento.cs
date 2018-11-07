using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class HistorialMovimiento
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static int Insetar(SinAlitas.Admin.Entidad.HistorialMovimiento historial)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            historial.FechaMovimiento = DateTime.Now;
            int id = fac.Insertar<SinAlitas.Admin.Entidad.HistorialMovimiento>(historial, setCnsWebLun);
            return id;
        }
    }
}

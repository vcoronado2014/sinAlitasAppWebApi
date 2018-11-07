using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class CancelacionClase
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static int Insetar(SinAlitas.Admin.Entidad.CancelacionClase cupo)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            int idCupo = fac.Insertar<SinAlitas.Admin.Entidad.CancelacionClase>(cupo, setCnsWebLun);
            return idCupo;
        }

    }
}

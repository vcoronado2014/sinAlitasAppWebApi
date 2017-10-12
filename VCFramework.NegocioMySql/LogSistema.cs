using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class LogsSistema
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];
        public static int Insertar(VCFramework.Entidad.LogsSistema entidad)
        {
            Factory fac = new Factory();
            return fac.Insertar<VCFramework.Entidad.LogsSistema>(entidad, setCnsWebLun);
        }
        public static List<VCFramework.Entidad.LogsSistema> Listar()
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "TIPO_MENSAJE";
            filtro.TipoDato = TipoDatoGeneral.Entero;
            filtro.Valor = "1";

            List<object> lista = fac.Leer<VCFramework.Entidad.LogsSistema>(filtro, setCnsWebLun);
            List<VCFramework.Entidad.LogsSistema> lista2 = new List<VCFramework.Entidad.LogsSistema>();
            if (lista != null)
            {
                lista2 = lista.Cast<VCFramework.Entidad.LogsSistema>().ToList();
            }

            return lista2;
        }
    }
}

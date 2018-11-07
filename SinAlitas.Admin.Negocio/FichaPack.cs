using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class FichaPack
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static int Insetar(SinAlitas.Admin.Entidad.FichaPack alumno)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();

            int id = fac.Insertar<SinAlitas.Admin.Entidad.FichaPack>(alumno, setCnsWebLun);
            return id;
        }
        public static List<SinAlitas.Admin.Entidad.FichaPack> ObtenerFichasPack(int idPack)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ACTIVO";
            filtro.Valor = "1";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ID_PACK";
            filtro1.Valor = idPack.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.FichaPack>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.FichaPack> lista2 = new List<Entidad.FichaPack>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.FichaPack>().ToList();
            }

            return lista2;
        }
    }
}

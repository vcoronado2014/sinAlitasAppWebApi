using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class ObservacionesProfesor
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static int Insetar(SinAlitas.Admin.Entidad.ObservacionesProfesor observaciones)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            int idObs = fac.Insertar<SinAlitas.Admin.Entidad.ObservacionesProfesor>(observaciones, setCnsWebLun);
            return idObs;
        }
        public static List<SinAlitas.Admin.Entidad.ObservacionesProfesor> ObtenerObservacionesCpoId(int cpoId)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "CPO_ID";
            filtro.Valor = cpoId.ToString();
            filtro.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);


            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.ObservacionesProfesor>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.ObservacionesProfesor> lista2 = new List<Entidad.ObservacionesProfesor>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.ObservacionesProfesor>().ToList();
            }

            return lista2;
        }
    }
}

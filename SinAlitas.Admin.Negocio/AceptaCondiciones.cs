using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinAlitas.Admin;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class AceptaCondiciones
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static int Insetar(SinAlitas.Admin.Entidad.AceptaCondiciones condiciones)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();

            int id = fac.Insertar<SinAlitas.Admin.Entidad.AceptaCondiciones>(condiciones, setCnsWebLun);
            return id;
        }
        public static SinAlitas.Admin.Entidad.AceptaCondiciones ObtenerAceptaCondicionesPcoId(int pcoId)
        {
            SinAlitas.Admin.Entidad.AceptaCondiciones entidad = new Entidad.AceptaCondiciones();
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ACTIVO";
            filtro.Valor = "1";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "PCO_ID";
            filtro1.Valor = pcoId.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.AceptaCondiciones>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.AceptaCondiciones> lista2 = new List<Entidad.AceptaCondiciones>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.AceptaCondiciones>().ToList();
            }
            if (lista2 != null && lista.Count == 1)
                entidad = lista2[0];

            return entidad;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class Producto
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static List<SinAlitas.Admin.Entidad.Producto> ObtenerProductos()
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ACTIVO";
            filtro.Valor = "1";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);


            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Producto>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Producto> lista2 = new List<Entidad.Producto>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Producto>().ToList();
            }

            return lista2;
        }
        public static Entidad.Producto ObtenerProductoPorId(int id)
        {
            Entidad.Producto retorno = ObtenerProductos().Find(p => p.Id == id);

            return retorno;
        }
    }
}

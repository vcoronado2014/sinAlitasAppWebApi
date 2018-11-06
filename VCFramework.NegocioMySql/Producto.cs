using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class Producto
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];
        public static List<VCFramework.Entidad.Producto> Listar()
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.TipoDato = TipoDatoGeneral.Entero;
            filtro.Valor = "0";

            List<object> lista = fac.Leer<VCFramework.Entidad.Producto>(filtro, setCnsWebLun);
            List<VCFramework.Entidad.Producto> lista2 = new List<VCFramework.Entidad.Producto>();
            if (lista != null)
            {
                lista2 = lista.Cast<VCFramework.Entidad.Producto>().ToList();
            }

            return lista2;
        }
        public static VCFramework.Entidad.Producto ObtenerPorNombrePack(string nombrePack)
        {
            VCFramework.Entidad.Producto entidad = new Entidad.Producto();

            try
            {
                string valor = nombrePack.ToUpper();
                List<VCFramework.Entidad.Producto> lista = Listar();
                if (lista != null && lista.Count > 0)
                {
                    if (lista.Exists(p=>p.Nombre.ToUpper() == valor))
                    {
                        entidad = lista.Find(p => p.Nombre.ToUpper() == valor);
                    }
                }


            }
            catch(Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }

            return entidad;
        }

        public static VCFramework.Entidad.Producto ObtenerPorId(int id)
        {
            VCFramework.Entidad.Producto entidad = new Entidad.Producto();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = id.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.Producto>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.Producto> lista2 = new List<VCFramework.Entidad.Producto>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.Producto>().ToList();
                }
                if (lista2 != null && lista2.Count == 1)
                    entidad = lista2[0];


            }
            catch (Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }

            return entidad;
        }

    }
}

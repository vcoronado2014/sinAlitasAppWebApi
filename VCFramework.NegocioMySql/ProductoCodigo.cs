using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;


namespace VCFramework.NegocioMySql
{
    public class ProductoCodigo
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];
        public static List<VCFramework.Entidad.ProductoCodigo> ListarTodo()
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();

            List<object> lista = fac.Leer<VCFramework.Entidad.ProductoCodigo>(setCnsWebLun);
            List<VCFramework.Entidad.ProductoCodigo> lista2 = new List<VCFramework.Entidad.ProductoCodigo>();
            if (lista != null)
            {
                lista2 = lista.Cast<VCFramework.Entidad.ProductoCodigo>().ToList();
            }

            return lista2;
        }
        public static int UltimoId()
        {
            int max = 0;
            if (ListarTodo() != null && ListarTodo().Count > 0 )
            {
                max = ListarTodo().Max(p => p.Id) + 1;
            }
            else
            {
                max = 1;
            }
            

            return max;
        }
        public static List<VCFramework.Entidad.ProductoCodigo> Listar()
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.TipoDato = TipoDatoGeneral.Entero;
            filtro.Valor = "0";

            List<object> lista = fac.Leer<VCFramework.Entidad.ProductoCodigo>(filtro, setCnsWebLun);
            List<VCFramework.Entidad.ProductoCodigo> lista2 = new List<VCFramework.Entidad.ProductoCodigo>();
            if (lista != null)
            {
                lista2 = lista.Cast<VCFramework.Entidad.ProductoCodigo>().ToList();
            }

            return lista2;
        }
        public static VCFramework.Entidad.ProductoCodigo ObtenerPorCodigoPack(string nombrePack)
        {
            VCFramework.Entidad.ProductoCodigo entidad = new Entidad.ProductoCodigo();

            try
            {
                string valor = nombrePack.ToUpper();
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "CODIGO_CLIENTE";
                filtro.TipoDato = TipoDatoGeneral.Varchar;
                filtro.Valor = valor;

                List<object> lista = fac.Leer<VCFramework.Entidad.ProductoCodigo>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.ProductoCodigo> lista2 = new List<VCFramework.Entidad.ProductoCodigo>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.ProductoCodigo>().ToList();
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

        public static VCFramework.Entidad.ProductoCodigo ObtenerPorCodigoPack(int idPack)
        {
            VCFramework.Entidad.ProductoCodigo entidad = new Entidad.ProductoCodigo();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = idPack.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.ProductoCodigo>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.ProductoCodigo> lista2 = new List<VCFramework.Entidad.ProductoCodigo>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.ProductoCodigo>().ToList();
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

        public static int Insertar(Entidad.ProductoCodigo pro)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Insertar<Entidad.ProductoCodigo>(pro, setCnsWebLun);
        }
        public static int Modificar(Entidad.ProductoCodigo pro)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Update<Entidad.ProductoCodigo>(pro, setCnsWebLun);
        }

    }
}

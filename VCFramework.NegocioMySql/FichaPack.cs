using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class FichaPack
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];

        public static List<VCFramework.Entidad.FichaPack> ObtenerPorIdpack(int idPack)
        {
            List<VCFramework.Entidad.FichaPack> lista2 = new List<VCFramework.Entidad.FichaPack>();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                List<FiltroGenerico> filtros = new List<FiltroGenerico>();

                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID_PACK";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = idPack.ToString();

                FiltroGenerico filtro1 = new FiltroGenerico();
                filtro1.Campo = "ELIMINADO";
                filtro1.TipoDato = TipoDatoGeneral.Entero;
                filtro1.Valor = "0";

                FiltroGenerico filtro2 = new FiltroGenerico();
                filtro2.Campo = "ACTIVO";
                filtro2.TipoDato = TipoDatoGeneral.Entero;
                filtro2.Valor = "1";
                filtros.Add(filtro);
                filtros.Add(filtro1);
                filtros.Add(filtro2);

                List<object> lista = fac.Leer<VCFramework.Entidad.FichaPack>(filtros, setCnsWebLun);

                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.FichaPack>().ToList();
                }


            }
            catch (Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }

            return lista2;
        }

        public static List<VCFramework.Entidad.FichaPack> ObtenerPorIdpackTodo(int idPack)
        {
            List<VCFramework.Entidad.FichaPack> lista2 = new List<VCFramework.Entidad.FichaPack>();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();

                List<FiltroGenerico> filtros = new List<FiltroGenerico>();

                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID_PACK";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = idPack.ToString();


                filtros.Add(filtro);
 
                List<object> lista = fac.Leer<VCFramework.Entidad.FichaPack>(filtros, setCnsWebLun);

                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.FichaPack>().ToList();
                }


            }
            catch (Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }

            return lista2;
        }

        public static int Insertar(Entidad.FichaPack ficha)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Insertar<Entidad.FichaPack>(ficha, setCnsWebLun);
        }
        public static int Modificar(Entidad.FichaPack ficha)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Update<Entidad.FichaPack>(ficha, setCnsWebLun);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class Profesor
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];

        public static VCFramework.Entidad.Profesor ObtenerPorId(int id)
        {
            VCFramework.Entidad.Profesor entidad = new Entidad.Profesor();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = id.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.Profesor>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.Profesor> lista2 = new List<VCFramework.Entidad.Profesor>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.Profesor>().ToList();
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

        public static VCFramework.Entidad.Profesor Obtener(string nombreUsuario, string password)
        {
            VCFramework.Entidad.Profesor entidad = new Entidad.Profesor();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "NOMBRE_USUARIO";
                filtro.TipoDato = TipoDatoGeneral.Varchar;
                filtro.Valor = nombreUsuario;

                FiltroGenerico filtro2 = new FiltroGenerico();
                filtro2.Campo = "PASSWORD";
                filtro2.TipoDato = TipoDatoGeneral.Varchar;
                filtro2.Valor = password;

                List<FiltroGenerico> filtros = new List<FiltroGenerico>();
                filtros.Add(filtro);
                filtros.Add(filtro2);

                List<object> lista = fac.Leer<VCFramework.Entidad.Profesor>(filtros, setCnsWebLun);
                List<VCFramework.Entidad.Profesor> lista2 = new List<VCFramework.Entidad.Profesor>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.Profesor>().ToList();
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

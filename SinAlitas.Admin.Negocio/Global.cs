using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class Global
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static SinAlitas.Admin.Entidad.Rol ObtenerRolPorId(int id)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico
            {
                Campo = "ELIMINADO",
                Valor = "0",
                TipoDato = TipoDatoGeneral.Entero
            };

            FiltroGenerico filtro1 = new FiltroGenerico
            {
                Campo = "ID",
                Valor = id.ToString(),
                TipoDato = TipoDatoGeneral.Entero
            };

            List<FiltroGenerico> filtros = new List<FiltroGenerico>
            {
                filtro,
                filtro1
            };


            SinAlitas.Admin.Entidad.Rol retorno = new Entidad.Rol();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Rol>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Rol> lista2 = new List<Entidad.Rol>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Rol>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static Entidad.EnvoltorioProductoCodigo RetornaEnvoltorioMostrar(int proId)
        {
            Entidad.EnvoltorioProductoCodigo retorno = new Entidad.EnvoltorioProductoCodigo();
            if (proId > 0)
            {
                //empezamos a construir el objeto, se supone que es una nueva construcción por lo tanto partimos de 0

                Entidad.Producto producto = Negocio.Producto.ObtenerProductoPorId(proId);
                if (producto.Id > 0)
                {
                    retorno.CantidadAlumnos = producto.CantidadAlumnos;
                    retorno.Descripcion = producto.Descripcion;
                    retorno.Id = producto.Id;
                    retorno.Nombre = producto.Nombre;
                    retorno.Precio = producto.Precio;
                    retorno.ValorClase = producto.ValorClase;
                    retorno.TopeClases = producto.TopeClases;
                }
            }
            return retorno;
        }
        public static string RetornaCodigoCliente(string nombrePack, int clieId, int cantidadClases, int alumnosAsisten)
        {
            string retorno = "";
            int ultimoIdPCO = Negocio.ProductoCodigo.UltimoId();
            //retorno = ultimoIdPCO.ToString() + "-" + nombrePack.ToUpper().Replace(" ", "") + "-" + clieId.ToString() + "-" + cantidadClases.ToString() + "-" + alumnosAsisten.ToString();
            retorno = ultimoIdPCO.ToString() + "-" + nombrePack.ToUpper().Replace(" ", "");
            return retorno;
        }
    }
    
}

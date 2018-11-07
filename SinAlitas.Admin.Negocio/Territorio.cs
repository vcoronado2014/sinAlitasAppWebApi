using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class Territorio
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static SinAlitas.Admin.Entidad.Pais ObtenerPaisPorId(int id)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ID";
            filtro1.Valor = id.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            SinAlitas.Admin.Entidad.Pais retorno = new Entidad.Pais();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Pais>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Pais> lista2 = new List<Entidad.Pais>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Pais>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static SinAlitas.Admin.Entidad.Region ObtenerRegionPorId(int id)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ID";
            filtro1.Valor = id.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            SinAlitas.Admin.Entidad.Region retorno = new Entidad.Region();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Region>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Region> lista2 = new List<Entidad.Region>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Region>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static SinAlitas.Admin.Entidad.Comuna ObtenerComunanPorId(int id)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ID";
            filtro1.Valor = id.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            SinAlitas.Admin.Entidad.Comuna retorno = new Entidad.Comuna();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Comuna>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Comuna> lista2 = new List<Entidad.Comuna>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Comuna>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static SinAlitas.Admin.Entidad.Nodo ObtenerNodoPorId(int id)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ID";
            filtro1.Valor = id.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            SinAlitas.Admin.Entidad.Nodo retorno = new Entidad.Nodo();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Nodo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Nodo> lista2 = new List<Entidad.Nodo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Nodo>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static List<Entidad.Region> ListarRegiones()
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Region>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Region> lista2 = new List<Entidad.Region>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Region>().ToList();
            }
            if (lista2 != null)
            {
                lista2 = lista2.FindAll(p => p.Id == 5 || p.Id == 13);
                Entidad.Region reg = new Entidad.Region();
                reg.Id = 0;
                reg.Nombre = "Seleccione";
                reg.PaiId = 1;
                lista2.Insert(0, reg);
            }
            return lista2;
        }
        public static List<Entidad.Comuna> ListarComunasPorRegionSin(int idReg)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "REG_ID";
            filtro1.Valor = idReg.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            SinAlitas.Admin.Entidad.Comuna retorno = new Entidad.Comuna();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Comuna>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Comuna> lista2 = new List<Entidad.Comuna>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Comuna>().ToList();

            }

            return lista2;
        }
        public static List<Entidad.Comuna> ListarComunasPorRegion(int idReg)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "REG_ID";
            filtro1.Valor = idReg.ToString();
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);


            SinAlitas.Admin.Entidad.Comuna retorno = new Entidad.Comuna();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Comuna>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Comuna> lista2 = new List<Entidad.Comuna>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Comuna>().ToList();
                Entidad.Comuna reg = new Entidad.Comuna();
                reg.Id = 0;
                reg.Nombre = "Seleccione";
                reg.RegId = idReg;
                lista2.Insert(0, reg);
            }

            return lista2;
        }

        public static List<Entidad.Comuna> ListarComunas()
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);

            SinAlitas.Admin.Entidad.Comuna retorno = new Entidad.Comuna();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Comuna>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Comuna> lista2 = new List<Entidad.Comuna>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Comuna>().ToList();
            }

            return lista2;
        }
    }


}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;


namespace VCFramework.NegocioMySql
{
    public class Cliente
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];

        public static VCFramework.Entidad.Cliente ObtenerPorId(int id)
        {
            VCFramework.Entidad.Cliente entidad = new Entidad.Cliente();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = id.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.Cliente>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.Cliente> lista2 = new List<VCFramework.Entidad.Cliente>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.Cliente>().ToList();
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
        public static List<VCFramework.Entidad.Cliente> Listar()
        {
            List<VCFramework.Entidad.Cliente> lista2 = new List<VCFramework.Entidad.Cliente>();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();

                List<object> lista = fac.Leer<VCFramework.Entidad.Cliente>(setCnsWebLun);

                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.Cliente>().ToList();
                }


            }
            catch (Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }

            return lista2;
        }
        public static int Insertar(Entidad.Cliente cliente)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Insertar<Entidad.Cliente>(cliente, setCnsWebLun);
        }
        public static int Modificar(Entidad.Cliente cliente)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Update<Entidad.Cliente>(cliente, setCnsWebLun);
        }
    }
}

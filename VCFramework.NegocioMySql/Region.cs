﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class Region
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];

        public static VCFramework.Entidad.Region ObtenerPorId(int id)
        {
            VCFramework.Entidad.Region entidad = new Entidad.Region();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = id.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.Region>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.Region> lista2 = new List<VCFramework.Entidad.Region>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.Region>().ToList();
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

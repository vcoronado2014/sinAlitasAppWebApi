﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class AceptaCondiciones
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];

        public static VCFramework.Entidad.AceptaCondiciones ObtenerPorPcoId(int pcoId)
        {
            VCFramework.Entidad.AceptaCondiciones entidad = new Entidad.AceptaCondiciones();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "PCO_ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = pcoId.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.AceptaCondiciones>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.AceptaCondiciones> lista2 = new List<VCFramework.Entidad.AceptaCondiciones>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.AceptaCondiciones>().ToList();
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
        public static int Insertar(Entidad.AceptaCondiciones acepta)
        {
            VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
            return fac.Insertar<Entidad.AceptaCondiciones>(acepta, setCnsWebLun);
        }
    }
}

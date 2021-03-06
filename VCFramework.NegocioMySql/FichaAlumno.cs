﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCFramework.Negocio.Factory;

namespace VCFramework.NegocioMySql
{
    public class FichaAlumno
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BDColegioSql"];

        public static VCFramework.Entidad.FichaAlumno ObtenerPorId(int id)
        {
            VCFramework.Entidad.FichaAlumno entidad = new Entidad.FichaAlumno();

            try
            {
                VCFramework.Negocio.Factory.Factory fac = new VCFramework.Negocio.Factory.Factory();
                FiltroGenerico filtro = new FiltroGenerico();
                filtro.Campo = "ID";
                filtro.TipoDato = TipoDatoGeneral.Entero;
                filtro.Valor = id.ToString();

                List<object> lista = fac.Leer<VCFramework.Entidad.FichaAlumno>(filtro, setCnsWebLun);
                List<VCFramework.Entidad.FichaAlumno> lista2 = new List<VCFramework.Entidad.FichaAlumno>();
                if (lista != null)
                {
                    lista2 = lista.Cast<VCFramework.Entidad.FichaAlumno>().ToList();
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

        public static List<VCFramework.Entidad.FichaAlumno> ObtenerAlumnosPorCliente(int idCliente)
        {
            List<VCFramework.Entidad.FichaAlumno> entidad = new List<Entidad.FichaAlumno>();


            try
            {
                List<VCFramework.Entidad.ProductoCodigo> producots = VCFramework.NegocioMySql.ProductoCodigo.ListarTodo();
                if (producots != null && producots.Count > 0)
                {
                    foreach(Entidad.ProductoCodigo pro in producots)
                    {
                        if (pro.ClieId == idCliente)
                        {
                            List<VCFramework.Entidad.FichaPack> fpack = VCFramework.NegocioMySql.FichaPack.ObtenerPorIdpackTodo(pro.Id);
                            if (fpack != null && fpack.Count > 0)
                            {
                                foreach (Entidad.FichaPack fic in fpack)
                                {
                                    int idAlumno = fic.IdAlumno;
                                    Entidad.FichaAlumno ficha = NegocioMySql.FichaAlumno.ObtenerPorId(idAlumno);
                                    if (ficha.Id > 0)
                                    {
                                        if (!entidad.Exists(p => p.NombreCompleto == ficha.NombreCompleto))
                                        {
                                            //si no existe agregalo
                                            entidad.Add(ficha);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }

            return entidad;
        }

    }
}

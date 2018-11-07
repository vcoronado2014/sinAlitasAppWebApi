using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinAlitas.Admin;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class CalificacionesProfesor
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static int Insetar(SinAlitas.Admin.Entidad.CalificacionesProfesor calificaciones)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();

            int id = fac.Insertar<SinAlitas.Admin.Entidad.CalificacionesProfesor>(calificaciones, setCnsWebLun);
            return id;
        }
        public static List<SinAlitas.Admin.Entidad.CalificacionesProfesor> ObtenerCalificacionesPorProfId(int profId)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "PROF_ID";
            filtro.Valor = profId.ToString();
            filtro.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);


            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.CalificacionesProfesor>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.CalificacionesProfesor> lista2 = new List<Entidad.CalificacionesProfesor>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.CalificacionesProfesor>().ToList();
            }

            return lista2;
        }
        public static string PromedioCalificaciones(int profId)
        {
            string retorno = "0";
            List<SinAlitas.Admin.Entidad.CalificacionesProfesor> lista = ObtenerCalificacionesPorProfId(profId);
            if (lista != null && lista.Count > 0)
            {
                int total = lista.Count;
                int suma = lista.Sum(p => p.Calificacion);
                int promedio = suma / total;
                retorno = promedio.ToString();
            }


            return retorno;
        }

        public static List<SinAlitas.Admin.Entidad.CalificacionesProfesor> UltimasDiezObservaciones(int profId)
        {
            List<SinAlitas.Admin.Entidad.CalificacionesProfesor> lista = ObtenerCalificacionesPorProfId(profId);
            if (lista != null && lista.Count > 0)
            {
                lista = lista.OrderByDescending(p => p.Fecha).ToList();
                if (lista != null && lista.Count > 0)
                    lista = lista.GetRange(0, 10).ToList();

            }
            return lista;
        }
        public static List<Entidad.EnvoltorioCalificaciones> ListarCalificaciones(int profId)
        {
            List<Entidad.EnvoltorioCalificaciones> lista = new List<Entidad.EnvoltorioCalificaciones>();

            List<Entidad.CalificacionesProfesor> listaCal = ObtenerCalificacionesPorProfId(profId);
            if (listaCal != null && listaCal.Count > 0)
            {
                foreach (Entidad.CalificacionesProfesor cal in listaCal)
                {
                    Entidad.EnvoltorioCalificaciones env = new Entidad.EnvoltorioCalificaciones();
                    env.Calificacion = cal.Calificacion;
                    env.Fecha = cal.Fecha;
                    env.FechaStr = cal.Fecha.ToShortDateString();
                    env.Id = cal.Id;
                    env.Observacion = cal.Observacion;
                    env.PcoId = cal.PcoId;
                    env.ProfId = cal.ProfId;
                    //buscamos al cliente, pero antes buscamos producto codigo
                    List<Entidad.ProductoCodigo> prod = Negocio.ProductoCodigo.ObtenerProductosCodigoPorId(env.PcoId);
                    if (prod != null && prod.Count == 1)
                    {
                        Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(prod[0].ClieId);
                        if (cliente != null && cliente.Id > 0)
                        {
                            env.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;

                        }
                    }
                    lista.Add(env);
                }
            }
            if (lista != null && lista.Count > 0)
                lista = lista.OrderByDescending(p => p.Fecha).ToList();
            return lista;    
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using VCFramework.Negocio.Factory;

using System.Runtime.Caching;


namespace SinAlitas.Admin.Negocio
{
    public class Cupo
    {
        //cache ******************************************************************
        private static ObjectCache cacheLeerFiltro = MemoryCache.Default;
        private static DateTimeOffset tiempoCacheLeerFiltro = Cache.Fuerte();
        //************************************************************************

        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposDelProfesorTareas(int fechaConsultaEntera, int nodId, int profId)
        {

            CultureInfo cultura = new CultureInfo("es-CL");

            int cantidadDiasConsultar = Entidad.Utiles.CANTIDAD_DIAS_CONSULTA_TAREAS();
            //proecsamos la fecha
            DateTime fechaProfesar = Entidad.Utiles.RetornaFecha(fechaConsultaEntera).AddDays(-7);
            DateTime fechaInicio = Convert.ToDateTime(fechaProfesar.ToShortDateString() + " 06:00");
            DateTime fechaTermino = Convert.ToDateTime(fechaProfesar.ToShortDateString() + " 23:00").AddDays(cantidadDiasConsultar);
            //se le agregan 30 días para revisar las tareas pendientes.
            fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino);

            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposDelProfesorTareas(int fechaConsultaEntera, int nodId, int profId, int cantidadDias)
        {

            //solo para pruebas
            //acá vamos a manejar el caché
            //ListarCuposDelProfesorTareas
            //profid_valor, nodid_valor
            //List<KeyValuePair<string, object>> caches = cacheLeerFiltro.ToList();

            //if (caches != null && caches.Count > 0)
            //{
            //    foreach(KeyValuePair<string, object> key in caches)
            //    {
                    
            //    }
            //}

            //string nombreArchivoCache = "ListarCuposDelProfesorTareas_fechaconsultaentera_" + fechaConsultaEntera.ToString() + "_nodid_" + nodId.ToString() + "_profid_" + profId.ToString() + "_cantidaddias_" + cantidadDias.ToString();


            CultureInfo cultura = new CultureInfo("es-CL");

            int cantidadDiasConsultar = cantidadDias;
            //proecsamos la fecha
            DateTime fechaProfesar = Entidad.Utiles.RetornaFecha(fechaConsultaEntera);
            DateTime fechaInicio = Convert.ToDateTime(fechaProfesar.ToShortDateString() + " 06:00");
            DateTime fechaTermino = Convert.ToDateTime(fechaProfesar.ToShortDateString() + " 23:00").AddDays(cantidadDiasConsultar);
            //se le agregan 30 días para revisar las tareas pendientes.
            fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();

            //List<SinAlitas.Admin.Entidad.Cupo> listaCache = (List<SinAlitas.Admin.Entidad.Cupo>)cacheLeerFiltro.Get(nombreArchivoCache);

            //if (listaCache == null)
            //{
                List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);

                if (lista != null)
                {

                    lista2 = lista.Cast<Entidad.Cupo>().ToList();
                }
                if (lista2 != null && lista2.Count > 0)
                {
                    lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino);
                    //caché  ****************************************************************
                    //almacenamos los resultados que tengan más de un registro
                    //if (lista2 != null && lista2.Count > 1)
                    //{
                    //    CacheItemPolicy policy = new CacheItemPolicy();
                    //    policy.AbsoluteExpiration = tiempoCacheLeerFiltro;
                    //    cacheLeerFiltro.Add(nombreArchivoCache, lista2, policy);
                    //}
                    //***********************************************************************
                }
            //}
            //else
            //    lista2 = listaCache;
            return lista2;
        }
        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposDelProfesor(int nodId, int profId, int sghId)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro4 = new FiltroGenerico();
            filtro4.Campo = "SGH_ID";
            filtro4.Valor = sghId.ToString();
            filtro4.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            filtros.Add(filtro4);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }

            return lista2;
        }

        public static SinAlitas.Admin.Entidad.Cupo ObtenerCupo(int nodId, int profId, int sghId, DateTime fechaHoraInicio)
        {
            SinAlitas.Admin.Entidad.Cupo cupo = new Entidad.Cupo();
            List<SinAlitas.Admin.Entidad.Cupo> cuposProfesor = ListarCuposDelProfesor(nodId, profId, sghId);
            if (cuposProfesor !=null && cuposProfesor.Count > 0)
            {
                cupo = cuposProfesor.FirstOrDefault(p => p.FechaHoraInicio == fechaHoraInicio);
            }
            return cupo;
        }

        public static int Insetar(SinAlitas.Admin.Entidad.Cupo cupo)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            int idCupo = fac.Insertar<SinAlitas.Admin.Entidad.Cupo>(cupo, setCnsWebLun);
            return idCupo;
        }

        public static int Actualizar(SinAlitas.Admin.Entidad.Cupo cupo)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            int idCupo = fac.Update<SinAlitas.Admin.Entidad.Cupo>(cupo, setCnsWebLun);
            return idCupo;
        }
        public static int Eliminar(SinAlitas.Admin.Entidad.Cupo cupo)
        {
            Factory fac = new Factory();
            return fac.Delete<SinAlitas.Admin.Entidad.Cupo>(cupo, setCnsWebLun);
        }
        public static SinAlitas.Admin.Entidad.Cupo ObtenerCupoPorId(int cpoId)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "ID";
            filtro2.Valor = cpoId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            SinAlitas.Admin.Entidad.Cupo entidad = new Entidad.Cupo();
            if (lista2 != null && lista2.Count == 1)
                entidad = lista2[0];

            return entidad;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposParaClientes(int nodId, int profId)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo los 7 días siguientes

            int cantidadDiasConsultar = 60;
            //proecsamos la fecha
            DateTime fechaProfesar = DateTime.Now.AddHours(23);
            DateTime fechaInicio = Convert.ToDateTime(fechaProfesar.ToShortDateString() + " 06:00");
            DateTime fechaTermino = Convert.ToDateTime(fechaInicio.ToShortDateString() + " 23:00").AddDays(cantidadDiasConsultar);
            //se le agregan 30 días para revisar las tareas pendientes.
            //fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino);

            return lista2;
        }
        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposEliminar(int profId, int sgh_id, int fechaInicioEntera, int idOriginal)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo los 7 días siguientes

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "SGH_ID";
            filtro2.Valor = sgh_id.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => Entidad.Utiles.RetornaFechaEntera(p.FechaHoraInicio) == fechaInicioEntera && p.Id != idOriginal);

            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposEliminar(int profId, int sgh_id, int fechaInicioEntera)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo los 7 días siguientes

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "SGH_ID";
            filtro2.Valor = sgh_id.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => Entidad.Utiles.RetornaFechaEntera(p.FechaHoraInicio) == fechaInicioEntera);

            return lista2;
        }
        public static List<Entidad.DetalleCupo> ListaProximosCuposDisponiblesDetalleSemana(int nodId, int profId, int cantidadCupos, int cupoIdReagendar)
        {
            List<Entidad.DetalleCupo> listaRetornar = new List<Entidad.DetalleCupo>();

            List<SinAlitas.Admin.Entidad.Cupo> cupos = ListarProximosCuposDisponiblesSemana(nodId, profId, cantidadCupos, cupoIdReagendar);
            if (cupos != null && cupos.Count > 0)
            {
                foreach (Entidad.Cupo cpo in cupos)
                {
                    Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                    detalle.Cupo = cpo;
                    detalle.TextoMostrar = cpo.FechaHoraInicio.ToShortDateString() + " de " + cpo.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cpo.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs.";
                    listaRetornar.Add(detalle);
                }
            }

            return listaRetornar;
        }
        public static List<Entidad.DetalleCupo> ListaProximosCuposDisponiblesDetalle(int nodId, int profId, int cantidadCupos, int cupoIdReagendar)
        {
            List<Entidad.DetalleCupo> listaRetornar = new List<Entidad.DetalleCupo>();

            List<SinAlitas.Admin.Entidad.Cupo> cupos = ListarProximosCuposDisponibles(nodId, profId, cantidadCupos, cupoIdReagendar);
            if (cupos != null && cupos.Count > 0)
            {
                foreach(Entidad.Cupo cpo in cupos)
                {
                    Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                    detalle.Cupo = cpo;
                    detalle.TextoMostrar = cpo.FechaHoraInicio.ToShortDateString() +  " de " + cpo.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cpo.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs."; 
                    listaRetornar.Add(detalle);
                }
            }

            return listaRetornar;
        }
        public static List<SinAlitas.Admin.Entidad.Cupo> ListarProximosCuposDisponibles(int nodId, int profId, int cantidadCupos, int cupoIdReagendar)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo la cantidadCupos disponibles de los días siguientes

            int cantidadDiasConsultar = 30;
            Entidad.Cupo cupoReagendar =  Negocio.Cupo.ObtenerCupoPorId(cupoIdReagendar);
            //proecsamos la fecha
            DateTime fechaProfesar = DateTime.MinValue;
            if (cupoReagendar != null && cupoReagendar.Id > 0)
                fechaProfesar = cupoReagendar.FechaHoraInicio.AddHours(3);


            DateTime fechaInicio = fechaProfesar;
            DateTime fechaTermino = Convert.ToDateTime(fechaInicio.ToShortDateString() + " 23:00").AddDays(cantidadDiasConsultar);
            //se le agregan 30 días para revisar las tareas pendientes.
            //fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro4 = new FiltroGenerico();
            filtro4.Campo = "PCO_ID";
            filtro4.Valor = "0";
            filtro4.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            filtros.Add(filtro4);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino);

            if (lista2 != null && lista2.Count > 0)
            {
                if (lista2.Count < cantidadCupos)
                    cantidadCupos = lista2.Count;
                lista2 = lista2.GetRange(0, cantidadCupos);
            }

            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarProximosCuposDisponiblesSemana(int nodId, int profId, int cantidadCupos, int cupoIdReagendar)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo la cantidadCupos disponibles de los días siguientes

            int cantidadDiasConsultar = 60;
            Entidad.Cupo cupoReagendar = Negocio.Cupo.ObtenerCupoPorId(cupoIdReagendar);
            //proecsamos la fecha
            DateTime fechaProfesar = DateTime.MinValue;
            if (cupoReagendar != null && cupoReagendar.Id > 0)
                //fechaProfesar = cupoReagendar.FechaHoraInicio.AddHours(3);
                fechaProfesar = DateTime.Now.AddHours(2);

            DateTime fechaInicio = fechaProfesar;
            DateTime fechaTermino = Convert.ToDateTime(fechaInicio.ToShortDateString() + " 23:00").AddDays(cantidadDiasConsultar);
            //se le agregan 30 días para revisar las tareas pendientes.
            //fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro4 = new FiltroGenerico();
            filtro4.Campo = "PCO_ID";
            filtro4.Valor = "0";
            filtro4.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);
            filtros.Add(filtro4);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino);

            //if (lista2 != null && lista2.Count > 0)
            //{
            //    if (lista2.Count < cantidadCupos)
            //        cantidadCupos = lista2.Count;
            //    lista2 = lista2.GetRange(0, cantidadCupos);
            //}

            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposParaClientes(int nodId, int profId, int cantidadClases, DateTime fechaInicioSemana)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo los 7 días siguientes
            
            int cantidadDiasConsultar = 6;
            //if (cantidadClases >= 8)
            //    cantidadDiasConsultar = cantidadClases - 1;
            //proecsamos la fecha
            //DateTime fechaProfesar = fechaInicioSemana.AddHours(23);
            DateTime fechaProfesar = fechaInicioSemana;
            DateTime fechaInicio = Convert.ToDateTime(fechaProfesar.ToShortDateString() + " 06:00");
            DateTime fechaTermino = Convert.ToDateTime(fechaInicio.ToShortDateString() + " 23:00").AddDays(cantidadDiasConsultar);
            //se le agregan 30 días para revisar las tareas pendientes.
            //fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            List<SinAlitas.Admin.Entidad.Cupo> lista3 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino && p.PcoId == 0);
            if (lista2 != null && lista2.Count > 0)
            {
                //lista2.Where(p=>p.SghId)
                //lista3 = lista2.Distinct().ToList();
                var DistinctItems = lista2.GroupBy(x => x.FechaHoraInicio).Select(y => y.First());
                if (DistinctItems != null)
                {
                    foreach (var item in DistinctItems)
                    {
                        //Add to other List
                        lista3.Add(item);
                    }
                }
            }
            List<Entidad.Cupo> listaCuposDuplicados = new List<Entidad.Cupo>();
            if (lista3 != null && lista3.Count > 0)
            {
                //debemos verificar si tiene sus segmentos ocupados o no antes de mostrarlos
                foreach(SinAlitas.Admin.Entidad.Cupo cpPro in lista3)
                {
                    //verifica segmento
                    List<Entidad.Cupo> cupoQuitar = SinAlitas.Admin.Negocio.Cupo.ListarCuposEliminar(cpPro.ProfId, cpPro.SghId, Entidad.Utiles.RetornaFechaEntera(cpPro.FechaHoraInicio));
                    if (cupoQuitar != null && cupoQuitar.Count > 1)
                    {
                        if (cupoQuitar.Exists(p=>p.ClieId > 0))
                        {
                            //ahora hay que eliminar el cupo con clie id 0 de la lista 3
                            Entidad.Cupo cupoEliminar = cupoQuitar.Find(p => p.ClieId == 0);
                            if (cupoEliminar != null && cupoEliminar.Id > 0)
                            {
                                listaCuposDuplicados.Add(cupoEliminar);
                                //lista3.Remove(cupoEliminar);
                            }
                        }
                    }
                }
            }
            if (listaCuposDuplicados != null && listaCuposDuplicados.Count > 0)
            {
                lista3.RemoveAll(r => listaCuposDuplicados.Any(a => a.Id == r.Id));

                foreach (SinAlitas.Admin.Entidad.Cupo cpProw in listaCuposDuplicados)
                {
                    Negocio.Cupo.Eliminar(cpProw);
                }
            }
            if (lista3 != null && lista3.Count > 0)
                return lista3;
            else
                return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposProductoCodigo(int pcoId)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo los 7 días siguientes

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "PCO_ID";
            filtro2.Valor = pcoId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;
            
            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            
            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }

            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ListarCuposReportePagos(DateTime fechaInicio, DateTime fechaTermino, int nodId, int profId)
        {

            CultureInfo cultura = new CultureInfo("es-CL");
            //en este caso de los clientes se muestran solo los 7 días siguientes

            int cantidadDiasConsultar = 6;
            //proecsamos la fecha
            DateTime fechaProfesar = DateTime.Now.AddHours(23);
            //se le agregan 30 días para revisar las tareas pendientes.
            //fechaTermino = fechaTermino.AddDays(30);

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOD_ID";
            filtro2.Valor = nodId.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = profId.ToString();
            filtro3.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }
            if (lista2 != null && lista2.Count > 0)
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= fechaInicio && p.FechaHoraTermino <= fechaTermino && p.PcoId > 0);

            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ObtenerCuposNotificaciones(int profId)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            List<SinAlitas.Admin.Entidad.Cupo> lista = new List<Entidad.Cupo>();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from CPO_CUPO where convert(varchar, FECHA_HORA_INICIO, 103) = convert(varchar, getdate(), 103) and PROF_ID = @prof_id", conn);
            cmd.Parameters.AddWithValue("@prof_id", profId);
            conn.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int SGH_ID = rdr.GetOrdinal("SGH_ID");
                int FECHA_HORA_INICIO = rdr.GetOrdinal("FECHA_HORA_INICIO");
                int FECHA_HORA_TERMINO = rdr.GetOrdinal("FECHA_HORA_TERMINO");
                int ESTADO_CUPO = rdr.GetOrdinal("ESTADO_CUPO");
                int DESCRIPCION = rdr.GetOrdinal("DESCRIPCION");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int NOD_ID = rdr.GetOrdinal("NOD_ID");
                int PROF_ID = rdr.GetOrdinal("PROF_ID");
                int CLIE_ID = rdr.GetOrdinal("CLIE_ID");
                int FECHA_HORA_REGISTRO = rdr.GetOrdinal("FECHA_HORA_REGISTRO");
                int PCO_ID = rdr.GetOrdinal("PCO_ID");

                try
                {
                    while (rdr.Read())
                    {
                        Entidad.Cupo entidad = new Entidad.Cupo();
                        entidad.Id = rdr.IsDBNull(ID) ? 0 : rdr.GetInt32(ID);
                        entidad.SghId = rdr.IsDBNull(SGH_ID) ? 0 : rdr.GetInt32(SGH_ID);
                        entidad.FechaHoraInicio = rdr.IsDBNull(FECHA_HORA_INICIO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_HORA_INICIO);
                        entidad.FechaHoraTermino = rdr.IsDBNull(FECHA_HORA_TERMINO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_HORA_TERMINO);
                        entidad.EstadoCupo = rdr.IsDBNull(ESTADO_CUPO) ? 0 : rdr.GetInt32(ESTADO_CUPO);
                        entidad.NodId = rdr.IsDBNull(NOD_ID) ? 0 : rdr.GetInt32(NOD_ID);
                        entidad.Eliminado = 0;
                        entidad.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        entidad.ProfId = rdr.IsDBNull(PROF_ID) ? 0 : rdr.GetInt32(PROF_ID);
                        entidad.ClieId = rdr.IsDBNull(CLIE_ID) ? 0 : rdr.GetInt32(CLIE_ID);
                        entidad.FechaHoraRegistro = rdr.IsDBNull(FECHA_HORA_REGISTRO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_HORA_REGISTRO);
                        entidad.PcoId = rdr.IsDBNull(PCO_ID) ? 0 : rdr.GetInt32(PCO_ID);
                        lista.Add(entidad);

                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                conn.Close();
            }
            return lista;
        }
    }
}

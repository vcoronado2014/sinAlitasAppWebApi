using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;
using SinAlitas.Admin;
using System.Runtime.Caching;

namespace SinAlitas.Admin.Negocio
{
    public class SegmentoHorario
    {

        private static ObjectCache cacheLeerFiltro = MemoryCache.Default;
        private static DateTimeOffset tiempoCacheLeerFiltro = Cache.Fuerte();

        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static List<SinAlitas.Admin.Entidad.SegmentoHorario> ListarSegmentos(int nodId)
        {
            string nombreArchivoCache = "ListarSegmentos_nod_id_" + nodId.ToString();

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

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            List<SinAlitas.Admin.Entidad.SegmentoHorario> lista2 = new List<Entidad.SegmentoHorario>();

            List<SinAlitas.Admin.Entidad.SegmentoHorario> listaCache = (List<SinAlitas.Admin.Entidad.SegmentoHorario>)cacheLeerFiltro.Get(nombreArchivoCache);

            if (listaCache == null)
            {
                List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.SegmentoHorario>(filtros, setCnsWebLun);

                if (lista != null)
                {

                    lista2 = lista.Cast<Entidad.SegmentoHorario>().ToList();
                    //caché  ****************************************************************
                    //almacenamos los resultados que tengan más de un registro
                    if (lista2 != null && lista2.Count > 1)
                    {
                        CacheItemPolicy policy = new CacheItemPolicy();
                        policy.AbsoluteExpiration = tiempoCacheLeerFiltro;
                        cacheLeerFiltro.Add(nombreArchivoCache, lista2, policy);
                    }
                    //***********************************************************************
                }
            }
            else
                lista2 = listaCache;

            return lista2;
        }
        private static string RetornaMesDia(int mes)
        {
            string retorno = "01";
            if (mes < 10)
                retorno = "0" + mes.ToString();
            else
                retorno = mes.ToString();

            return retorno;
        }

        public static Entidad.SegmentoMostrarFecha ContruirSegmentosFechaNuevo(int nodId, int mesInicio, int profId, int anno)
        {
            //como regla de negocio se establece que la agenda solo puede estar editable entre 15
            //y 25 días antes de cada mes, solo en ese periodo.
            bool puedeAgendar = Entidad.Utiles.PuedeAgendar(mesInicio);

            SinAlitas.Admin.Entidad.SegmentoMostrarFecha retorno = new Admin.Entidad.SegmentoMostrarFecha();
            if (puedeAgendar)
            {

                List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();

                List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);

                if (sgh != null && sgh.Count > 0)
                {
                    DateTime fechaIni = Convert.ToDateTime("01-" + RetornaMesDia(mesInicio) + "-" + anno.ToString());
                    DateTime fechaTer = Convert.ToDateTime(DateTime.DaysInMonth(anno, mesInicio).ToString() + "-" + RetornaMesDia(mesInicio) + "-" + anno.ToString());

                    retorno.Anno = fechaIni.Year;
                    retorno.Mes = fechaIni.Month;
                    retorno.Dias = new List<Entidad.SegmentoMostrar>();

                    List<SinAlitas.Admin.Entidad.Cupo> cupos = Negocio.Cupo.ListarCuposDelProfesorTareas(Entidad.Utiles.RetornaFechaEntera(fechaIni), nodId, profId, DateTime.DaysInMonth(anno, mesInicio));

                    while (fechaIni <= fechaTer)
                    {

                        Entidad.SegmentoMostrar dia = new Entidad.SegmentoMostrar();
                        dia.Fecha = fechaIni;
                        dia.FechaEntera = int.Parse(fechaIni.Year.ToString() + RetornaMesDia(fechaIni.Month) + RetornaMesDia(fechaIni.Day));
                        //dia.Horas = new List<Entidad.SegmentoHora>();
                        int total = sgh.Count;

                        int contadorCupos = 0;

                        foreach (SinAlitas.Admin.Entidad.SegmentoHorario seg in sgh)
                        {
                            DateTime fechaHoraSegmento = Convert.ToDateTime(fechaIni.ToShortDateString() + " " + RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));

                            string inicioSegmento = RetornaMesDia(sgh[0].FechaHoraInicial.Hour) + ":" + RetornaMesDia(sgh[0].FechaHoraInicial.Minute);
                            string terminoSegmento = RetornaMesDia(sgh.Last().FechaHoraInicial.Hour) + ":" + RetornaMesDia(sgh.Last().FechaHoraInicial.Minute);
                            dia.HoraInicio = inicioSegmento;
                            dia.HoraTermino = terminoSegmento;

                            //List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p => p.FechaHoraInicio == fechaHoraSegmento);
                            List<Entidad.Cupo> cupo = cupos.FindAll(p=> p.SghId == seg.Id && p.FechaHoraInicio == fechaHoraSegmento);

                            //definir este estado mediante una llamada de negocio al Profesor
                            dia.Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                            if (cupo != null && cupo.Count > 0)
                            {
                                Entidad.Cupo cupoActual = cupo[0];
                                if (cupoActual.Id > 0)
                                {
                                    dia.Estado = cupoActual.EstadoCupo;
                                    dia.ClieId = cupoActual.ClieId;
                                    dia.CpoId = cupoActual.Id;
                                    //dia.ProfId = cupoActual.ProfId;
                                    contadorCupos++;
                                }
                            }
                            dia.NodId = nodId;
                            dia.ProfId = profId;
                            //dia.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                            //dia.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                            dia.DiaSemana = fechaIni.ToString("dddd", new CultureInfo("es-CL"));
                            dia.DiaSemanaInt = fechaIni.Day;
                            //dia.Horas.Add(hora);

                            dia.CuposDisponibles = total - contadorCupos;

                            //evaluamos el dia completo
                            DateTime fechaHoraActual = DateTime.Now;
                            dia.ClaseDia = "btn btn-primary btn-circle";
                            //ahora puede agendar llos fines de semana
                            if (dia.DiaSemana == "domingo" || dia.DiaSemana == "sábado")
                            {
                                dia.ClaseDia = "btn btn-danger btn-circle";
                            }
                            int fechaEnteraSegmento = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaHoraSegmento);
                            int fechaEnteraHoy = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(DateTime.Now);

                            if (fechaEnteraHoy == fechaEnteraSegmento)
                                dia.ClaseDia = "btn btn-warning btn-circle";

                            //PUEDE PERMITIR BORRAR TODO EN CASO DE
                            //HAYAN CUPOS TOMADOS, 
                            //ahora puede agendar los fines de semana
                            //if (contadorCupos > 0 && fechaHoraSegmento > fechaHoraActual.AddHours(24) && (dia.DiaSemana != "sábado" || dia.DiaSemana != "domingo"))
                            if (contadorCupos > 0 && fechaHoraSegmento > fechaHoraActual.AddHours(24))
                            {
                                dia.DeshabilitaBorrarTodo = 1;

                            }
                            else
                            {
                                dia.DeshabilitaBorrarTodo = 0;
                            }

                            //if (total == contadorCupos || dia.DiaSemana == "sábado" || dia.DiaSemana == "domingo" || fechaHoraSegmento < fechaHoraActual.AddHours(24))
                            if (fechaHoraSegmento < fechaHoraActual.AddHours(1))
                            {
                                dia.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                                //desactivar
                                dia.DeshabilitaCrearTodo = 0;
                                dia.DeshabilitaCrearUno = 1;
                                //dia.DeshabilitaBorrarTodo = 0;
                                dia.DeshabilitaBorrarUno = 1;
                            }
                            else
                            {
                                dia.ClaseBorde = "col-xs-12 bg-aquamarine border-left-activo border-bottom-burlywood border-right-activo no-padding";
                                //activar
                                dia.DeshabilitaCrearTodo = 1;
                                dia.DeshabilitaCrearUno = 1;
                                //dia.DeshabilitaBorrarTodo = 1;
                                dia.DeshabilitaBorrarUno = 1;

                            }
                            //retorno.Dias.Add(dia);


                        }

                        retorno.Dias.Add(dia);
                        fechaIni = fechaIni.AddDays(1);
                    }



                }
            }
            else
            {
                //MOSTRAR UN MENSAJE PARA EL USUARIO QUE NO PUEDE AGENDAR
                retorno = new Entidad.SegmentoMostrarFecha();
            }

            return retorno;
        }
        public static Entidad.SegmentoMostrarFecha ContruirSegmentosFecha(int nodId, int mesInicio, int profId)
        {
            //como regla de negocio se establece que la agenda solo puede estar editable entre 15
            //y 25 días antes de cada mes, solo en ese periodo.
            bool puedeAgendar = Entidad.Utiles.PuedeAgendar(mesInicio);

            SinAlitas.Admin.Entidad.SegmentoMostrarFecha retorno = new Admin.Entidad.SegmentoMostrarFecha();
            if (puedeAgendar)
            {

                List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();

                List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);
                if (sgh != null && sgh.Count > 0)
                {
                    DateTime fechaIni = Convert.ToDateTime("01-" + RetornaMesDia(mesInicio) + "-" + DateTime.Now.Year.ToString());
                    DateTime fechaTer = Convert.ToDateTime(DateTime.DaysInMonth(DateTime.Now.Year, mesInicio).ToString() + "-" + RetornaMesDia(mesInicio) + "-" + DateTime.Now.Year.ToString());

                    retorno.Anno = fechaIni.Year;
                    retorno.Mes = fechaIni.Month;
                    retorno.Dias = new List<Entidad.SegmentoMostrar>();

                    while (fechaIni <= fechaTer)
                    {

                        Entidad.SegmentoMostrar dia = new Entidad.SegmentoMostrar();
                        dia.Fecha = fechaIni;
                        dia.FechaEntera = int.Parse(fechaIni.Year.ToString() + RetornaMesDia(fechaIni.Month) + RetornaMesDia(fechaIni.Day));
                        //dia.Horas = new List<Entidad.SegmentoHora>();
                        int total = sgh.Count;

                        int contadorCupos = 0;

                        foreach (SinAlitas.Admin.Entidad.SegmentoHorario seg in sgh)
                        {
                            DateTime fechaHoraSegmento = Convert.ToDateTime(fechaIni.ToShortDateString() + " " + RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));

                            string inicioSegmento = RetornaMesDia(sgh[0].FechaHoraInicial.Hour) + ":" + RetornaMesDia(sgh[0].FechaHoraInicial.Minute);
                            string terminoSegmento = RetornaMesDia(sgh.Last().FechaHoraInicial.Hour) + ":" + RetornaMesDia(sgh.Last().FechaHoraInicial.Minute);
                            dia.HoraInicio = inicioSegmento;
                            dia.HoraTermino = terminoSegmento;

                            List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p => p.FechaHoraInicio == fechaHoraSegmento);
                            //definir este estado mediante una llamada de negocio al Profesor
                            dia.Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                            if (cupo != null && cupo.Count > 0)
                            {
                                Entidad.Cupo cupoActual = cupo[0];
                                if (cupoActual.Id > 0)
                                {
                                    dia.Estado = cupoActual.EstadoCupo;
                                    dia.ClieId = cupoActual.ClieId;
                                    dia.CpoId = cupoActual.Id;
                                    //dia.ProfId = cupoActual.ProfId;
                                    contadorCupos++;
                                }
                            }
                            dia.NodId = nodId;
                            dia.ProfId = profId;
                            //dia.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                            //dia.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                            dia.DiaSemana = fechaIni.ToString("dddd", new CultureInfo("es-CL"));
                            dia.DiaSemanaInt = fechaIni.Day;
                            //dia.Horas.Add(hora);

                            dia.CuposDisponibles = total - contadorCupos;

                            //evaluamos el dia completo
                            DateTime fechaHoraActual = DateTime.Now;
                            dia.ClaseDia = "label round size-16";
                            if (dia.DiaSemana == "domingo" || dia.DiaSemana == "sábado")
                            {
                                dia.ClaseDia = "label round alert size-16";
                            }
                            int fechaEnteraSegmento = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaHoraSegmento);
                            int fechaEnteraHoy = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(DateTime.Now);

                            if (fechaEnteraHoy == fechaEnteraSegmento)
                                dia.ClaseDia = "label round warning size-16";

                            //PUEDE PERMITIR BORRAR TODO EN CASO DE
                            //HAYAN CUPOS TOMADOS, 
                            if (contadorCupos > 0 && fechaHoraSegmento > fechaHoraActual.AddHours(24) && (dia.DiaSemana != "sábado" || dia.DiaSemana != "domingo"))
                            {
                                dia.DeshabilitaBorrarTodo = 1;

                            }
                            else
                            {
                                dia.DeshabilitaBorrarTodo = 0;
                            }

                            if (total == contadorCupos || dia.DiaSemana == "sábado" || dia.DiaSemana == "domingo" || fechaHoraSegmento < fechaHoraActual.AddHours(24))
                            {
                                dia.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                                //desactivar
                                dia.DeshabilitaCrearTodo = 0;
                                dia.DeshabilitaCrearUno = 1;
                                //dia.DeshabilitaBorrarTodo = 0;
                                dia.DeshabilitaBorrarUno = 1;
                            }
                            else
                            {
                                dia.ClaseBorde = "row bg-aquamarine border-left-activo border-bottom-burlywood";
                                //activar
                                dia.DeshabilitaCrearTodo = 1;
                                dia.DeshabilitaCrearUno = 1;
                                //dia.DeshabilitaBorrarTodo = 1;
                                dia.DeshabilitaBorrarUno = 1;

                            }
                            //retorno.Dias.Add(dia);


                        }

                        retorno.Dias.Add(dia);
                        fechaIni = fechaIni.AddDays(1);
                    }



                }
            }
            else
            {
                //MOSTRAR UN MENSAJE PARA EL USUARIO QUE NO PUEDE AGENDAR
                retorno = new Entidad.SegmentoMostrarFecha();
            }

            return retorno;
        }

        public static List<Entidad.SegmentoMostrar> BuscarSegmentoPorFecha(DateTime fecha, int nodId, int profId, bool esBloquedo)
        {
            List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();
            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);
            int contadorCupos = 0;
            int Estado = 0;
            int total = sgh.Count;
            if (sgh != null && sgh.Count > 0)
            {
                foreach (Entidad.SegmentoHorario seg in sgh)
                {
                    Entidad.SegmentoMostrar entidad = new Entidad.SegmentoMostrar();

                    //construimos las fechas
                    DateTime fechaInicio = Convert.ToDateTime(fecha.ToShortDateString() + " " + RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));
                    //si tenemos una fecha de inicio por cada segmento ahora buscamos los cupos que ya estan en ese segmento
                    List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p => p.FechaHoraInicio == fechaInicio);
                    Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                    if (cupo != null && cupo.Count > 0)
                    {
                        Entidad.Cupo cupoActual = cupo[0];
                        if (cupoActual.Id > 0)
                        {
                            entidad.Estado = cupoActual.EstadoCupo;
                            entidad.Comentario = cupoActual.Descripcion;
                            entidad.ClieId = cupoActual.ClieId;
                            entidad.CpoId = cupoActual.Id;
                            entidad.EsSeleccionado = true;
                            //entidad.ProfId = cupoActual.ProfId;
                            contadorCupos++;
                        }
                    }

                    entidad.NodId = nodId;
                    entidad.ProfId = profId;

                    entidad.DiaSemana = fechaInicio.ToString("dddd", new CultureInfo("es-CL"));
                    entidad.DiaSemanaInt = fechaInicio.Day;

                    entidad.CuposDisponibles = total - contadorCupos;

                    entidad.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                    entidad.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                    entidad.Fecha = fecha;
                    entidad.FechaEntera = int.Parse(fecha.Year.ToString() + RetornaMesDia(fecha.Month) + RetornaMesDia(fecha.Day));
                    DateTime fechaHoraActual = DateTime.Now;
                    string motivoNo = "";
                    bool utilizable = true;
                    if (total == contadorCupos || entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo" || fechaInicio < fechaHoraActual || entidad.Estado != 0)
                    {
                        if (total == contadorCupos)
                            motivoNo = " Están todos los cupos tomados.";
                        else if (entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo")
                            motivoNo = " No se puede tomar cupos el fin de semana.";
                        else if (fechaInicio < fechaHoraActual)
                            motivoNo = " La Hora es menor a la hora actual.";
                        else if (entidad.Estado != 0)
                            motivoNo = " Cupo ya agendado.";

                        entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                        utilizable = false;
                    }
                    else
                        entidad.ClaseBorde = "row bg-aquamarine border-left-activo border-bottom-burlywood";


                    if (utilizable == false)
                        entidad.Comentario = entidad.Comentario + motivoNo;

                    if (esBloquedo)
                    {
                        entidad.EsDisponible = false;
                    }
                    lista.Add(entidad);
                }
            }


            return lista;
        }
        public static List<Entidad.SegmentoMostrar> BuscarSegmentoPorFecha(DateTime fecha, int nodId, int profId)
        {
            List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();
            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);
            int contadorCupos = 0;
            int Estado = 0;
            int total = sgh.Count;
            if (sgh != null && sgh.Count > 0)
            {
                foreach (Entidad.SegmentoHorario seg in sgh)
                {
                    Entidad.SegmentoMostrar entidad = new Entidad.SegmentoMostrar();

                    //construimos las fechas
                    DateTime fechaInicio = Convert.ToDateTime(fecha.ToShortDateString() + " " + RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));
                    //si tenemos una fecha de inicio por cada segmento ahora buscamos los cupos que ya estan en ese segmento
                    List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p => p.FechaHoraInicio == fechaInicio);
                    Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                    if (cupo != null && cupo.Count > 0)
                    {
                        Entidad.Cupo cupoActual = cupo[0];
                        if (cupoActual.Id > 0)
                        {
                            entidad.Estado = cupoActual.EstadoCupo;
                            entidad.Comentario = cupoActual.Descripcion;
                            entidad.ClieId = cupoActual.ClieId;
                            entidad.CpoId = cupoActual.Id;
                            //entidad.ProfId = cupoActual.ProfId;
                            contadorCupos++;
                        }
                    }

                    entidad.NodId = nodId;
                    entidad.ProfId = profId;

                    entidad.DiaSemana = fechaInicio.ToString("dddd", new CultureInfo("es-CL"));
                    entidad.DiaSemanaInt = fechaInicio.Day;

                    entidad.CuposDisponibles = total - contadorCupos;

                    entidad.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                    entidad.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                    entidad.Fecha = fecha;
                    entidad.FechaEntera =  int.Parse(fecha.Year.ToString() + RetornaMesDia(fecha.Month) + RetornaMesDia(fecha.Day));
                    DateTime fechaHoraActual = DateTime.Now;
                    string motivoNo = "";
                    bool utilizable = true;
                    if (total == contadorCupos || entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo" || fechaInicio < fechaHoraActual || entidad.Estado != 0)
                    {
                        if (total == contadorCupos)
                            motivoNo = " Están todos los cupos tomados.";
                        else if (entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo")
                            motivoNo = " No se puede tomar cupos el fin de semana.";
                        else if (fechaInicio < fechaHoraActual)
                            motivoNo = " La Hora es menor a la hora actual.";
                        else if (entidad.Estado != 0)
                            motivoNo = " Cupo ya agendado.";

                        entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                        utilizable = false;
                    }
                    else
                        entidad.ClaseBorde = "row bg-aquamarine border-left-activo border-bottom-burlywood";


                    if (utilizable == false)
                        entidad.Comentario = entidad.Comentario + motivoNo;

                    lista.Add(entidad);
                }
            }


           return lista;
        }

        /// <summary>
        /// aca voy
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="nodId"></param>
        /// <param name="profId"></param>
        /// <param name="esBloquedo"></param>
        /// <returns></returns>
        public static List<Entidad.SegmentoMostrar> BuscarSegmentoPorFechaNuevo(int fechaEntera, int nodId, int profId, int esBloquedo)
        {
            List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();
            //los segmentos del nodo
            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);
            //los cupos del Profe
            List<SinAlitas.Admin.Entidad.Cupo> cupos = Negocio.Cupo.ListarCuposDelProfesorTareas(fechaEntera, nodId, profId, 1);
            int contadorCupos = 0;
            int Estado = 0;
            int total = sgh.Count;
            DateTime fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(fechaEntera);
            bool yaTieneCliente = false;
            if (sgh != null && sgh.Count > 0)
            {
                foreach (Entidad.SegmentoHorario seg in sgh)
                {
                    Entidad.SegmentoMostrar entidad = new Entidad.SegmentoMostrar();
                    entidad.EsDisponible = true;
                    entidad.SghId = seg.Id;
                    //construimos las fechas
                    DateTime fechaInicio = Convert.ToDateTime(fecha.ToShortDateString() + " " + RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));
                    //si tenemos una fecha de inicio por cada segmento ahora buscamos los cupos que ya estan en ese segmento
                    List<Entidad.Cupo> cupo = cupos.FindAll(p => p.FechaHoraInicio == fechaInicio && p.SghId == seg.Id);
                    Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                    if (cupo != null && cupo.Count > 0)
                    {
                        Entidad.Cupo cupoActual = cupo[0];
                        if (cupoActual.Id > 0)
                        {
                            entidad.Estado = cupoActual.EstadoCupo;
                            entidad.Comentario = cupoActual.Descripcion;
                            entidad.ClieId = cupoActual.ClieId;
                            entidad.CpoId = cupoActual.Id;
                            //entidad.ProfId = cupoActual.ProfId;
                            entidad.EsSeleccionado = true;
                            if (cupoActual.ClieId > 0)
                            {
                                yaTieneCliente = true;
                                //buscamos al cliente
                                Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(cupoActual.ClieId);
                                if (cliente != null && cliente.Id > 0)
                                {
                                    entidad.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                                    if (entidad.NombreCliente.Length > 0)
                                        entidad.NombreClienteVisible = true;

                                    entidad.TelefonosCliente = cliente.TelefonosContacto;
                                    if (entidad.TelefonosCliente.Length > 0)
                                        entidad.TelefonoClienteVisible = true;

                                    entidad.CorreoCliente = cliente.Email;
                                    if (entidad.CorreoCliente.Length > 0)
                                        entidad.EmailClienteVisible = true;
                                }

                            }
                            contadorCupos++;
                        }
                    }

                    entidad.NodId = nodId;
                    entidad.ProfId = profId;

                    entidad.DiaSemana = fechaInicio.ToString("dddd", new CultureInfo("es-CL"));
                    entidad.DiaSemanaInt = fechaInicio.Day;

                    entidad.CuposDisponibles = total - contadorCupos;

                    entidad.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                    entidad.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                    entidad.Fecha = fecha;
                    entidad.FechaEntera = int.Parse(fecha.Year.ToString() + RetornaMesDia(fecha.Month) + RetornaMesDia(fecha.Day));
                    //la fecha hora actual debe aumentar en 24 hh ya que no se pueden agendar cupos con menos de 24 hh de anticipación
                    DateTime fechaHoraActualTope = DateTime.Now.AddHours(0);
                    DateTime fechaHoraActual = DateTime.Now;
                    string motivoNo = "";
                    //if (entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo")
                    //{
                    //    motivoNo = " No se puede tomar cupos el fin de semana.";
                    //    entidad.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                    //    entidad.EsDisponible = false;
                    //}
                    //else
                    if (fechaInicio < fechaHoraActual)
                    {
                        if (entidad.CpoId > 0)
                        {
                            motivoNo = " Ya está agendada (Sin Modificación por fecha hora).";
                            entidad.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                            entidad.EsDisponible = false;
                        }
                        else
                        {
                            motivoNo = " No se puede agendar porque la fecha es menor a la actual.";
                            entidad.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                            entidad.EsDisponible = false;
                        }

                    }
                    else if (fechaInicio > fechaHoraActual && fechaInicio < fechaHoraActualTope)
                    {
                        if (entidad.CpoId > 0)
                        {
                            motivoNo = " Ya está agendada (Sin Modificación por fecha hora).";
                            entidad.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                            entidad.EsDisponible = false;
                        }
                        else
                        {
                            motivoNo = " Solo se pueden agendar con 24 horas de anticipación.";
                            entidad.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                            entidad.EsDisponible = false;
                        }
                    }
                    else if (yaTieneCliente)
                    {
                        if (entidad.ClieId > 0)
                        {
                            motivoNo = " No se puede agendar porque ya se encuentra tomada por un Cliente.";
                            entidad.ClaseBorde = "col-xs-12 bg-panel border-left-inactivo border-bottom-burlywood border-right-inactivo no-padding";
                            entidad.EsDisponible = false;
                        }
                        else
                        {
                            motivoNo = " Ya está agendada (Puede modificar).";
                            entidad.ClaseBorde = "col-xs-12 bg-aquamarine border-left-activo border-bottom-burlywood border-right-activo no-padding";
                        }

                    }
                    else
                    {
                        if (entidad.CpoId > 0)
                        {
                            motivoNo = "Ya está agendada (Puede modificar)";
                        }
                        else
                        {
                            motivoNo = "Disponible para agendar.";
                        }
                        entidad.ClaseBorde = "col-xs-12 bg-aquamarine border-left-activo border-bottom-burlywood border-right-activo no-padding";
                    }

                    if (esBloquedo == 1)
                        entidad.EsDisponible = false;


                    entidad.Comentario = motivoNo;


                    lista.Add(entidad);


                }
            }


            return lista;
        }
        public static List<Entidad.SegmentoMostrar> BuscarSegmentoPorFecha(int fechaEntera, int nodId, int profId, int esBloquedo)
        {
            List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();
            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);
            int contadorCupos = 0;
            int Estado = 0;
            int total = sgh.Count;
            DateTime fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(fechaEntera);
            bool yaTieneCliente = false;

            if (sgh != null && sgh.Count > 0)
            {
                foreach (Entidad.SegmentoHorario seg in sgh)
                {
                    Entidad.SegmentoMostrar entidad = new Entidad.SegmentoMostrar();
                    entidad.EsDisponible = true;
                    entidad.SghId = seg.Id;
                    //construimos las fechas
                    DateTime fechaInicio = Convert.ToDateTime(fecha.ToShortDateString() + " " + RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));
                    //si tenemos una fecha de inicio por cada segmento ahora buscamos los cupos que ya estan en ese segmento
                    List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p => p.FechaHoraInicio == fechaInicio);
                    Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                    if (cupo != null && cupo.Count > 0)
                    {
                        Entidad.Cupo cupoActual = cupo[0];
                        if (cupoActual.Id > 0)
                        {
                            entidad.Estado = cupoActual.EstadoCupo;
                            entidad.Comentario = cupoActual.Descripcion;
                            entidad.ClieId = cupoActual.ClieId;
                            entidad.CpoId = cupoActual.Id;
                            //entidad.ProfId = cupoActual.ProfId;
                            entidad.EsSeleccionado = true;
                            if (cupoActual.ClieId > 0)
                            {
                                yaTieneCliente = true;
                                //buscamos al cliente
                                Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(cupoActual.ClieId);
                                if (cliente != null && cliente.Id > 0)
                                {
                                    entidad.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                                    if (entidad.NombreCliente.Length > 0)
                                        entidad.NombreClienteVisible = true;

                                    entidad.TelefonosCliente = cliente.TelefonosContacto;
                                    if (entidad.TelefonosCliente.Length > 0)
                                        entidad.TelefonoClienteVisible = true;

                                    entidad.CorreoCliente = cliente.Email;
                                    if (entidad.CorreoCliente.Length > 0)
                                        entidad.EmailClienteVisible = true;
                                }

                            }
                            contadorCupos++;
                        }
                    }

                    entidad.NodId = nodId;
                    entidad.ProfId = profId;

                    entidad.DiaSemana = fechaInicio.ToString("dddd", new CultureInfo("es-CL"));
                    entidad.DiaSemanaInt = fechaInicio.Day;

                    entidad.CuposDisponibles = total - contadorCupos;

                    entidad.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                    entidad.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                    entidad.Fecha = fecha;
                    entidad.FechaEntera = int.Parse(fecha.Year.ToString() + RetornaMesDia(fecha.Month) + RetornaMesDia(fecha.Day));
                    //la fecha hora actual debe aumentar en 24 hh ya que no se pueden agendar cupos con menos de 24 hh de anticipación
                    DateTime fechaHoraActualTope = DateTime.Now.AddHours(24);
                    DateTime fechaHoraActual = DateTime.Now;
                    string motivoNo = "";
                    if (entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo")
                    {
                        motivoNo = " No se puede tomar cupos el fin de semana.";
                        entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                        entidad.EsDisponible = false;
                    }
                    else if (fechaInicio < fechaHoraActual)
                    {
                        if (entidad.CpoId > 0)
                        {
                            motivoNo = " Ya está agendada (Sin Modificación por fecha hora).";
                            entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                            entidad.EsDisponible = false;
                        }
                        else
                        {
                            motivoNo = " No se puede agendar porque la fecha es menor a la actual.";
                            entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                            entidad.EsDisponible = false;
                        }
                        
                    }
                    else if (fechaInicio > fechaHoraActual && fechaInicio < fechaHoraActualTope)
                    {
                        if (entidad.CpoId > 0)
                        {
                            motivoNo = " Ya está agendada (Sin Modificación por fecha hora).";
                            entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                            entidad.EsDisponible = false;
                        }
                        else
                        {
                            motivoNo = " Solo se pueden agendar con 24 horas de anticipación.";
                            entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                            entidad.EsDisponible = false;
                        }
                    }
                    else if (yaTieneCliente)
                    {

                        motivoNo = " No se puede agendar porque ya se encuentra tomada por un Cliente.";
                        entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                        entidad.EsDisponible = false;

                    }
                    else
                    {
                        if (entidad.CpoId > 0)
                        {
                            motivoNo = "Ya está agendada (Puede modificar)";
                        }
                        else
                        {
                            motivoNo = "Disponible para agendar.";
                        }
                        entidad.ClaseBorde = "row bg-aquamarine border-left-activo border-bottom-burlywood";
                    }

                    #region comentado
                    //if (total == contadorCupos || entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo" || fechaInicio < fechaHoraActual)
                    //{
                    //    if (total == contadorCupos)
                    //        motivoNo = " Están todos los cupos tomados.";
                    //    else if (entidad.DiaSemana == "sábado" || entidad.DiaSemana == "domingo")
                    //        motivoNo = " No se puede tomar cupos el fin de semana.";
                    //    else if (fechaInicio < fechaHoraActual)
                    //        motivoNo = " La Hora es menor a la hora actual.";
                    //    else if (entidad.Estado != 0)
                    //        motivoNo = " Cupo ya agendado.";

                    //    entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                    //    utilizable = false;
                    //    entidad.EsDisponible = false;
                    //}
                    //else
                    //    entidad.ClaseBorde = "row bg-aquamarine border-left-activo border-bottom-burlywood";
                    #endregion

                    if (esBloquedo == 1)
                        entidad.EsDisponible = false;


                   entidad.Comentario = motivoNo;
                        

                    lista.Add(entidad);
                }
            }


            return lista;
        }

        public static Entidad.SegmentoFecha ContruirSegmentos(int nodId, int mesInicio, int profId)
        {
            Entidad.SegmentoFecha retorno = new Entidad.SegmentoFecha();
            List<Entidad.SegmentoFecha> lista = new List<Entidad.SegmentoFecha>();

            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = ListarSegmentos(nodId);
            if (sgh != null && sgh.Count > 0)
            {

                DateTime fechaIni = Convert.ToDateTime("01-" + RetornaMesDia(mesInicio) + "-" + DateTime.Now.Year.ToString());
                DateTime fechaTer = Convert.ToDateTime(DateTime.DaysInMonth(DateTime.Now.Year, mesInicio).ToString() + "-" + RetornaMesDia(mesInicio) + "-" + DateTime.Now.Year.ToString());

                retorno.Anno = fechaIni.Year;
                retorno.Mes = fechaIni.Month;
                retorno.Dias = new List<Entidad.SegmentoDia>();

                while (fechaIni <= fechaTer)
                {
                    Entidad.SegmentoDia dia = new Entidad.SegmentoDia();
                    dia.Fecha = fechaIni;
                    dia.Horas = new List<Entidad.SegmentoHora>();

                    int total = sgh.Count;
                    
                    int contadorCupos = 0;

                    foreach (SinAlitas.Admin.Entidad.SegmentoHorario seg in sgh)
                    {
                        DateTime fechaHoraSegmento = Convert.ToDateTime(fechaIni.ToShortDateString() + " " +  RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute));
                        Entidad.SegmentoHora hora = new Entidad.SegmentoHora();
                        hora.Cupo = new Entidad.Cupo();
                        

                        List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p=>p.FechaHoraInicio == fechaHoraSegmento);
                        //definir este estado mediante una llamada de negocio al Profesor
                        hora.Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                        if (cupo != null && cupo.Count > 0)
                        {
                            Entidad.Cupo cupoActual = cupo[0];
                            if (cupoActual.Id > 0)
                            {
                                hora.Estado = cupoActual.EstadoCupo;
                                hora.Cupo = cupoActual;
                                
                                contadorCupos++;
                            }
                        }

                        hora.HoraInicio = RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + RetornaMesDia(seg.FechaHoraInicial.Minute);
                        hora.HoraTermino = RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + RetornaMesDia(seg.FechaHoraFinal.Minute);

                        dia.DiaSemana = fechaIni.ToString("dddd", new CultureInfo("es-CL"));
                        dia.DiaSemanaInt = fechaIni.Day;
                        
                        dia.Horas.Add(hora);

                        dia.CuposDisponibles = total - contadorCupos;

                        //evaluamos el dia completo
                        if (total == contadorCupos || dia.DiaSemana == "sábado" || dia.DiaSemana == "domingo")
                            dia.ClaseBorde = "border-left-inactivo";
                        else
                            dia.ClaseBorde = "border-left-activo";


                    }

                    

                    retorno.Dias.Add(dia);
                    fechaIni = fechaIni.AddDays(1);


                }


            }

            return retorno;
        }

        public static string GuardarSegmentoCompleto(int nodId, int profId, int fechaEntera)
        {
            StringBuilder sb = new StringBuilder();
            string retoro = "";
            DateTime fechaEvaluar = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(fechaEntera);

            try
            {
                //lo primero es traer los segmentos
                List<SinAlitas.Admin.Entidad.SegmentoHorario> segmentos = ListarSegmentos(nodId);
                if (segmentos != null && segmentos.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.SegmentoHorario seg in segmentos)
                    {
                        //ahora por cada segmento se debe crear un cupo
                        DateTime fechaHoraInicio = Convert.ToDateTime(fechaEvaluar.ToShortDateString() + " " + seg.FechaHoraInicial.ToShortTimeString());
                        DateTime fechaHoraTermino = Convert.ToDateTime(fechaEvaluar.ToShortDateString() + " " + seg.FechaHoraFinal.ToShortTimeString());

                        SinAlitas.Admin.Entidad.Cupo cupo = new Entidad.Cupo();
                        cupo.Activo = 1;
                        cupo.ClieId = 0;
                        cupo.Descripcion = string.Empty;
                        cupo.Eliminado = 0;
                        cupo.EstadoCupo = 1;
                        cupo.FechaHoraInicio = fechaHoraInicio;
                        cupo.FechaHoraRegistro = DateTime.Now;
                        cupo.FechaHoraTermino = fechaHoraTermino;
                        cupo.NodId = nodId;
                        cupo.ProfId = profId;
                        cupo.SghId = seg.Id;

                        int id = Negocio.Cupo.Insetar(cupo);

                        sb.Append(id.ToString());

                    }
                }
                retoro = "Agendado con éxito el día " + fechaEvaluar.ToShortDateString();
            }
            catch(Exception ex)
            {
                retoro = ex.Message;
            }

            return retoro;
        }

        public static string BorrarSegmentoCompleto(int nodId, int profId, int fechaEntera)
        {
            StringBuilder sb = new StringBuilder();
            string retoro = "";
            DateTime fechaEvaluar = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(fechaEntera);

            try
            {
                int contadorEliminados = 0;
                List<Entidad.Cupo> cuposBorrar = Negocio.Cupo.ListarCuposDelProfesorTareas(fechaEntera, nodId, profId, 1);
                bool tieneAgenda = false;
                //si existe más de un cupo asignado al profesor debe ser borrado
                if (cuposBorrar != null && cuposBorrar.Count > 0)
                {
                    if (cuposBorrar.Exists(p=>p.PcoId > 0))
                    {
                        tieneAgenda = true;

                    }
                    if (tieneAgenda == false)
                    {
                        foreach (Entidad.Cupo cupoB in cuposBorrar)
                        {
                            Negocio.Cupo.Eliminar(cupoB);
                            contadorEliminados++;
                        }
                    }
                    else
                    {
                        retoro = "No se puede eliminar su agenda para el día " + fechaEvaluar.ToShortDateString() + ", ya que tiene cupos tomados por el cliente.";
                    }
                }

                retoro = "Se eliminaron " + contadorEliminados.ToString() + " Cupos del Profesor";
            }
            catch (Exception ex)
            {
                retoro = ex.Message;
            }

            return retoro;
        }

    }
}

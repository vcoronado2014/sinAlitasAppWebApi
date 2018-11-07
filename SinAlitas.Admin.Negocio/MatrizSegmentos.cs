using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Negocio
{
    public class MatrizSegmentos
    {


        public static Entidad.SegmentoMostrarFecha ContruirSegmentosParaCliente(int nodId, int profId)
        {

            SinAlitas.Admin.Entidad.SegmentoMostrarFecha retorno = new Admin.Entidad.SegmentoMostrarFecha();


            List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();

            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = SegmentoHorario.ListarSegmentos(nodId);

            if (sgh != null && sgh.Count > 0)
            {
                //la fecha de inicio del periodo corresponde a la fecha actual mas un dia (24 horas)
                //la fecha de termino debería ser semanalmente

                DateTime fechaIni = DateTime.Now.AddHours(24);
                DateTime fechaTer = fechaIni.AddDays(30);

                retorno.Anno = fechaIni.Year;
                retorno.Mes = fechaIni.Month;
                retorno.Dias = new List<Entidad.SegmentoMostrar>();

                List<SinAlitas.Admin.Entidad.Cupo> cupos = Negocio.Cupo.ListarCuposDelProfesorTareas(Entidad.Utiles.RetornaFechaEntera(fechaIni), nodId, profId, 30);

                while (fechaIni <= fechaTer)
                {

                    Entidad.SegmentoMostrar dia = new Entidad.SegmentoMostrar();
                    dia.Fecha = fechaIni;
                    dia.FechaEntera = int.Parse(fechaIni.Year.ToString() + Entidad.Utiles.RetornaMesDia(fechaIni.Month) + Entidad.Utiles.RetornaMesDia(fechaIni.Day));
                    //dia.Horas = new List<Entidad.SegmentoHora>();
                    int total = sgh.Count;

                    int contadorCupos = 0;

                    foreach (SinAlitas.Admin.Entidad.SegmentoHorario seg in sgh)
                    {
                        DateTime fechaHoraSegmento = Convert.ToDateTime(fechaIni.ToShortDateString() + " " + Entidad.Utiles.RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + Entidad.Utiles.RetornaMesDia(seg.FechaHoraInicial.Minute));

                        string inicioSegmento = Entidad.Utiles.RetornaMesDia(sgh[0].FechaHoraInicial.Hour) + ":" + Entidad.Utiles.RetornaMesDia(sgh[0].FechaHoraInicial.Minute);
                        string terminoSegmento = Entidad.Utiles.RetornaMesDia(sgh.Last().FechaHoraInicial.Hour) + ":" + Entidad.Utiles.RetornaMesDia(sgh.Last().FechaHoraInicial.Minute);
                        dia.HoraInicio = inicioSegmento;
                        dia.HoraTermino = terminoSegmento;

                        //List<Entidad.Cupo> cupo = Negocio.Cupo.ListarCuposDelProfesor(nodId, profId, seg.Id).FindAll(p => p.FechaHoraInicio == fechaHoraSegmento);
                        List<Entidad.Cupo> cupo = cupos.FindAll(p => p.SghId == seg.Id && p.FechaHoraInicio == fechaHoraSegmento);

                        //definir este estado mediante una llamada de negocio al Profesor
                        dia.Estado = 0; //definir estados para la hora lo mas probable que sea ocupada o no
                        if (cupo != null && cupo.Count > 0)
                        {
                            Entidad.Cupo cupoActual = cupo[0];
                            if (cupoActual.Id > 0 && cupoActual.PcoId <= 0)
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

                        //dia.CuposDisponibles = total - contadorCupos;
                        dia.CuposDisponibles = contadorCupos;

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
                        if (contadorCupos == 0)
                        {
                            dia.DeshabilitaBorrarTodo = 1;

                        }
                        else
                        {
                            dia.DeshabilitaBorrarTodo = 0;
                        }

                        if (contadorCupos == 0)
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


            return retorno;
        }

        public static List<Entidad.SegmentoMostrar> BuscarSegmentoProfesor(int fechaEntera, int nodId, int profId, int esBloquedo, int clieIdLog)
        {
            List<Entidad.SegmentoMostrar> lista = new List<Entidad.SegmentoMostrar>();
            //los segmentos del nodo
            List<SinAlitas.Admin.Entidad.SegmentoHorario> sgh = SegmentoHorario.ListarSegmentos(nodId);
            //los cupos del Profe
            List<SinAlitas.Admin.Entidad.Cupo> cupos = Negocio.Cupo.ListarCuposDelProfesorTareas(fechaEntera, nodId, profId, 1);
            int contadorCupos = 0;
            int total = sgh.Count;
            DateTime fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(fechaEntera);
            if (sgh != null && sgh.Count > 0)
            {
                foreach (Entidad.SegmentoHorario seg in sgh)
                {
                    Entidad.SegmentoMostrar entidad = new Entidad.SegmentoMostrar();
                    entidad.EsDisponible = true;
                    entidad.SghId = seg.Id;
                    //construimos las fechas
                    DateTime fechaInicio = Convert.ToDateTime(fecha.ToShortDateString() + " " + Entidad.Utiles.RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + Entidad.Utiles.RetornaMesDia(seg.FechaHoraInicial.Minute));
                    //si tenemos una fecha de inicio por cada segmento ahora buscamos los cupos que ya estan en ese segmento
                    List<Entidad.Cupo> cupo = cupos.FindAll(p => p.FechaHoraInicio == fechaInicio && p.SghId == seg.Id);
                    string nombreComuna = string.Empty;
                    if (cupo != null && cupo.Count > 0)
                    {
                        //si tiene cupo
                        Entidad.Cupo cupoActual = cupo[0];
                        if (cupoActual.Id > 0)
                        {
                            entidad.Estado = cupoActual.EstadoCupo;
                            entidad.Comentario = cupoActual.Descripcion;
                            entidad.ClieId = cupoActual.ClieId;
                            entidad.CpoId = cupoActual.Id;
                            //si el cupo tiene un cliente
                            if (cupoActual.PcoId > 0)
                            {
                                //si el cliente es el mismo que esta logueado
                                
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

                                    if(cliente.ComId != 0)
                                    {
                                        Entidad.Comuna comuna = Territorio.ObtenerComunanPorId(cliente.ComId);
                                        if (comuna != null && comuna.Id > 0)
                                            nombreComuna= comuna.Nombre;
                                    }
                                }
                                if (cupoActual.ClieId == clieIdLog)
                                {
                                    entidad.EsSeleccionado = true;
                                    entidad.EsDisponible = true;
                                    entidad.Comentario = "Disponible " + nombreComuna;
                                }
                                else
                                {
                                    //es otro cliente
                                    entidad.EsSeleccionado = false;
                                    entidad.EsDisponible = false;
                                    entidad.Comentario = "Ocupada " + nombreComuna;
                                }
                            }
                            else
                            {
                                entidad.EsSeleccionado = false;
                                entidad.EsDisponible = true;
                                entidad.Comentario = "Disponible " + nombreComuna;
                                contadorCupos++;
                            }
                            
                        }
                        else
                        {
                            //si por alguna razon el cupo no está nulo
                            //pero tiene un id = 0
                            entidad.EsSeleccionado = false;
                            entidad.EsDisponible = false;
                            entidad.Comentario = "No Agendada " + nombreComuna;
                        }
                    }
                    else
                    {
                        //si no tiene cupo entonces no es seleccionado y no esta disponible
                        entidad.EsSeleccionado = false;
                        entidad.EsDisponible = false;
                        entidad.Comentario = "No Agendada " + nombreComuna;
                    }

                    entidad.NodId = nodId;
                    entidad.ProfId = profId;

                    entidad.DiaSemana = fechaInicio.ToString("dddd", new CultureInfo("es-CL"));
                    entidad.DiaSemanaInt = fechaInicio.Day;

                    entidad.CuposDisponibles = total - contadorCupos;

                    entidad.HoraInicio = Entidad.Utiles.RetornaMesDia(seg.FechaHoraInicial.Hour) + ":" + Entidad.Utiles.RetornaMesDia(seg.FechaHoraInicial.Minute);
                    entidad.HoraTermino = Entidad.Utiles.RetornaMesDia(seg.FechaHoraFinal.Hour) + ":" + Entidad.Utiles.RetornaMesDia(seg.FechaHoraFinal.Minute);

                    entidad.Fecha = fecha;
                    entidad.FechaEntera = int.Parse(fecha.Year.ToString() + Entidad.Utiles.RetornaMesDia(fecha.Month) + Entidad.Utiles.RetornaMesDia(fecha.Day));
                    //la fecha hora actual debe aumentar en 24 hh ya que no se pueden agendar cupos con menos de 24 hh de anticipación
                    DateTime fechaHoraActualTope = DateTime.Now.AddHours(24);
                    DateTime fechaHoraActual = DateTime.Now;
                    string motivoNo = "";
                    if (entidad.EsDisponible)
                    {
                        //motivoNo = "Disponible.";
                        entidad.ClaseBorde = "row bg-aquamarine border-left-activo border-bottom-burlywood";
                    }
                    else
                    {
                            //motivoNo = "Ya está agendada.";
                            entidad.ClaseBorde = "row bg-panel border-left-inactivo border-bottom-burlywood";
                    }
                    //entidad.Comentario = motivoNo + " " + entidad.Comentario;


                    lista.Add(entidad);


                }
            }


            return lista;
        }

        public static List<Entidad.CupoLiviano> EntregaSegmentos(int fechaEntera, int nodId, int profId, int esBloquedo, int clieIdLog)
        {
            int id = 1;
            List<Entidad.CupoLiviano> lista = new List<Entidad.CupoLiviano>();

            List<Entidad.SegmentoMostrar> listaProcesar = BuscarSegmentoProfesor(fechaEntera, nodId, profId, esBloquedo, clieIdLog);
            if (listaProcesar != null && listaProcesar.Count > 0)
            {
                foreach(Entidad.SegmentoMostrar seg in listaProcesar)
                {
                    Entidad.CupoLiviano cupo = new Entidad.CupoLiviano();
                    cupo.Descripcion = seg.Comentario;
                    cupo.EsDisponible = seg.EsDisponible;
                    cupo.EsSeleccionado = seg.EsSeleccionado;
                    cupo.FechaEntera = seg.FechaEntera;
                    cupo.HoraInicio = seg.HoraInicio;
                    cupo.HoraTermino = seg.HoraTermino;
                    cupo.IdCupo = seg.CpoId;
                    cupo.SghId = seg.SghId;
                    //el id debe ser una composicion
                    int ini = int.Parse(cupo.HoraInicio.Replace(":", ""));
                    int ter = int.Parse(cupo.HoraTermino.Replace(":", ""));
                    string idStr = fechaEntera.ToString() + ini.ToString() + ter.ToString();
                    cupo.Id = idStr;
                    cupo.ClaseBorde = seg.ClaseBorde;
                    lista.Add(cupo);
                    id++;
                }
            }

            return lista;
        }
    }
}

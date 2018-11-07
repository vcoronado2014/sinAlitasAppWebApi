using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class TareasProfesor
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static List<Entidad.Notificacion> Notificaciones(int fechaInicioEntera, int nodId, int profId)
        {
            int contadorNotificaciones = 0;
            List<Entidad.Notificacion> lista = new List<Entidad.Notificacion>();
            
            List<Entidad.Tareas> tareasFuturas = ObtenerTareasProfesor(fechaInicioEntera, nodId, profId);
            if (tareasFuturas != null && tareasFuturas.Count > 0)
            {
                foreach (Entidad.Tareas tarea in tareasFuturas)
                {
                    if (!tarea.Descripcion.Contains("No hay Tareas Pendientes."))
                    {
                        StringBuilder sb = new StringBuilder();
                        Entidad.Notificacion not = new Entidad.Notificacion();
                        sb.Append("Desde: ");
                        sb.Append(tarea.FechaHoraInicioTexto);
                        sb.Append(" Hasta: ");
                        sb.Append(tarea.FechaHoraTerminoTexto);
                        sb.Append(" para el día ");
                        sb.Append(tarea.FechaTexto);
                        not.FechaHora = sb.ToString();
                        not.TipoNotificacion = Entidad.TipoNotificacion.Tarea;
                        not.Detalle = " Tiene una clase con " + tarea.NombreCliente + " en " + tarea.DireccionCliente;
                        lista.Add(not);
                        contadorNotificaciones++;
                    }
                }
            }
            else
            {
                Entidad.Notificacion not = new Entidad.Notificacion();
                not.Detalle = "No hay Tareas Pendientes.";
                not.FechaHora = string.Empty;
                not.TipoNotificacion = Entidad.TipoNotificacion.Tarea;
                lista.Add(not);
            }

            return lista;
        }
        public static List<Entidad.Tareas> ObtenerTareasProfesor(int fechaInicioEntera, int nodId, int profId)
        {
            int cantidadTareas = 0;
            List<Entidad.Tareas> listaRetornar = new List<Entidad.Tareas>();

            List<Entidad.Cupo> cupos = Cupo.ListarCuposDelProfesorTareas(fechaInicioEntera, nodId, profId);
            if (cupos != null && cupos.Count > 0)
            {
                foreach(Entidad.Cupo cupo in cupos)
                {
                    //ojo acá solo se muestran las tareas con un idCliente mayor a 0
                    if (cupo.ClieId > 0 && cupo.EstadoCupo == 2)
                    {
                        Entidad.Tareas tarea = new Entidad.Tareas();
                        tarea.Activo = cupo.Activo;
                        tarea.ClieId = cupo.ClieId;
                        Entidad.Cliente cliente = Cliente.ObtenerClientePorId(cupo.ClieId);
                        tarea.Descripcion = cupo.Descripcion;
                        Entidad.Pais paisCliente = Territorio.ObtenerPaisPorId(cliente.PaiId);
                        Entidad.Region regionCliente = Territorio.ObtenerRegionPorId(cliente.RegId);
                        Entidad.Comuna comunaCliente = Territorio.ObtenerComunanPorId(cliente.ComId);
                        //tarea.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre + " " + regionCliente.Nombre + " " + paisCliente.Nombre;
                        tarea.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre;
                        tarea.Eliminado = cupo.Eliminado;
                        tarea.EstadoCupo = cupo.EstadoCupo;
                        tarea.FechaHoraInicio = cupo.FechaHoraInicio;
                        tarea.FechaHoraInicioTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Minute);
                        tarea.FechaHoraRegistro = cupo.FechaHoraRegistro;
                        tarea.FechaHoraTermino = cupo.FechaHoraTermino;
                        tarea.FechaHoraTerminoTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Minute);
                        tarea.FotoCliente = "";
                        tarea.Id = cupo.Id;
                        tarea.NodId = cupo.NodId;
                        tarea.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                        tarea.ProfId = cupo.ProfId;
                        tarea.SghId = cupo.SghId;
                        tarea.FechaTexto = SinAlitas.Admin.Entidad.Utiles.EntregaFechaLetras(cupo.FechaHoraInicio);
                        listaRetornar.Add(tarea);
                        cantidadTareas++;
                    }
                }
            }
            if (cantidadTareas == 0)
            {
                Entidad.Tareas tarea = new Entidad.Tareas();
                tarea.Descripcion = "No hay Tareas Pendientes.";
                tarea.FechaHoraInicioTexto = "";
                tarea.FechaHoraTerminoTexto = "";
                tarea.FechaTexto = "";
                listaRetornar.Add(tarea);
            }

            return listaRetornar;
        }
        public static List<Entidad.TareasEnvoltorio> ObtenerTareasProfesorEnvoltorioSupervisor(int fechaInicioEntera, int nodId, int profId)
        {
            int id = 1;
            List<DateTime> fechasprocesar = new List<DateTime>();
            List<Entidad.TareasEnvoltorio> listaRetornar = new List<Entidad.TareasEnvoltorio>();

            List<Entidad.Cupo> cupos = Cupo.ListarCuposDelProfesorTareas(fechaInicioEntera, nodId, profId);

            if (cupos != null && cupos.Count > 0)
            {
                foreach (Entidad.Cupo cupo in cupos)
                {
                    DateTime fechaProc = Convert.ToDateTime(cupo.FechaHoraInicio.ToShortDateString());
                    if (!fechasprocesar.Exists(p => p.Date == fechaProc))
                    {
                        if (cupo.ClieId > 0 && cupo.EstadoCupo == 2)
                            fechasprocesar.Add(fechaProc);
                    }
                }
            }
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            if (fechasprocesar != null && fechasprocesar.Count > 0)
            {
                foreach (DateTime fec in fechasprocesar)
                {
                    Entidad.TareasEnvoltorio tarea = new Entidad.TareasEnvoltorio();
                    tarea.Fecha = fec;
                    tarea.FechaTexto = Entidad.Utiles.EntregaFechaLetras(fec);
                    tarea.TareasMostrar = new List<Entidad.Tareas>();
                    List<Entidad.Cupo> cuposFecha = cupos.FindAll(p => Convert.ToDateTime(p.FechaHoraInicio.ToShortDateString()) == fec && p.EstadoCupo == 2 && p.PcoId > 0);
                    if (cuposFecha != null && cuposFecha.Count > 0)
                    {
                        foreach (Entidad.Cupo cupo in cuposFecha)
                        {
                            Entidad.Tareas tarea1 = new Entidad.Tareas();
                            tarea1.Activo = cupo.Activo;
                            tarea1.ClieId = cupo.ClieId;
                            Entidad.Cliente cliente = Cliente.ObtenerClientePorId(cupo.ClieId);
                            List<Entidad.ProductoCodigo> prodCod = ProductoCodigo.ObtenerProductosCodigoPorId(cupo.PcoId);
                            tarea1.Descripcion = cupo.Descripcion;
                            Entidad.Pais paisCliente = Territorio.ObtenerPaisPorId(cliente.PaiId);
                            Entidad.Region regionCliente = Territorio.ObtenerRegionPorId(cliente.RegId);
                            Entidad.Comuna comunaCliente = Territorio.ObtenerComunanPorId(cliente.ComId);
                            //tarea.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre + " " + regionCliente.Nombre + " " + paisCliente.Nombre;
                            tarea1.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre;
                            tarea1.Eliminado = cupo.Eliminado;
                            tarea1.EstadoCupo = cupo.EstadoCupo;
                            tarea1.FechaHoraInicio = cupo.FechaHoraInicio;
                            tarea1.FechaHoraInicioTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Minute);
                            tarea1.FechaHoraRegistro = cupo.FechaHoraRegistro;
                            tarea1.FechaHoraTermino = cupo.FechaHoraTermino;
                            tarea1.FechaHoraTerminoTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Minute);
                            tarea1.FotoCliente = "";
                            tarea1.Id = cupo.Id;
                            tarea1.NodId = cupo.NodId;
                            tarea1.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                            tarea1.ProfId = cupo.ProfId;
                            tarea1.SghId = cupo.SghId;
                            tarea1.FechaTexto = SinAlitas.Admin.Entidad.Utiles.EntregaFechaLetras(cupo.FechaHoraInicio);
                            tarea1.IdPopup = id;
                            if (tarea1.FechaHoraTermino < cstTime)
                                tarea1.ClaseVisible = "fa fa-2x fa-hand-o-right pull-right";
                            else
                                tarea1.ClaseVisible = "fa fa-2x fa-hand-o-right pull-right hidden";

                            if (prodCod != null && prodCod.Count == 1)
                                tarea1.CodigoCliente = prodCod[0].CodigoCliente;

                            //ahora evaluamos las tareas que el profesor podría cancelar,
                            //son todas aquellas que tienen una fecha y hora de inicio mayor a 24
                            //horas al momento de la consulta, esta estaría en codiciones de ser
                            //cancelada
                            if (tarea1.FechaHoraInicio >= cstTime)
                                tarea1.ClaseVisibleCancelar = "fa fa-2x fa-trash pull-left";
                            else
                                tarea1.ClaseVisibleCancelar = "fa fa-2x fa-trash pull-left hidden";

                            tarea.TareasMostrar.Add(tarea1);
                        }
                    }
                    tarea.IdPopup = id;
                    id++;
                    listaRetornar.Add(tarea);
                }
            }

            return listaRetornar;
        }
        public static List<Entidad.TareasEnvoltorio> ObtenerTareasProfesorEnvoltorio(int fechaInicioEntera, int nodId, int profId)
        {
            int id = 1;
            List<DateTime> fechasprocesar = new List<DateTime>();
            List<Entidad.TareasEnvoltorio> listaRetornar = new List<Entidad.TareasEnvoltorio>();

            List<Entidad.Cupo> cupos = Cupo.ListarCuposDelProfesorTareas(fechaInicioEntera, nodId, profId);

            if (cupos != null && cupos.Count > 0)
            {
                foreach (Entidad.Cupo cupo in cupos)
                {
                    DateTime fechaProc = Convert.ToDateTime(cupo.FechaHoraInicio.ToShortDateString());
                    if (!fechasprocesar.Exists( p=>p.Date == fechaProc))
                    {
                        if (cupo.ClieId > 0 && cupo.EstadoCupo == 2)
                            fechasprocesar.Add(fechaProc);
                    }
                }
            }
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            if (fechasprocesar != null && fechasprocesar.Count > 0)
            {
                foreach(DateTime fec in fechasprocesar)
                {
                    Entidad.TareasEnvoltorio tarea = new Entidad.TareasEnvoltorio();
                    tarea.Fecha = fec;
                    tarea.FechaTexto = Entidad.Utiles.EntregaFechaLetras(fec);
                    tarea.TareasMostrar = new List<Entidad.Tareas>();
                    List<Entidad.Cupo> cuposFecha = cupos.FindAll(p => Convert.ToDateTime(p.FechaHoraInicio.ToShortDateString()) == fec && p.EstadoCupo == 2 && p.PcoId > 0);
                    if (cuposFecha != null && cuposFecha.Count > 0)
                    {
                        foreach (Entidad.Cupo cupo in cuposFecha)
                        {
                            Entidad.Tareas tarea1 = new Entidad.Tareas();
                            tarea1.Activo = cupo.Activo;
                            tarea1.ClieId = cupo.ClieId;
                            Entidad.Cliente cliente = Cliente.ObtenerClientePorId(cupo.ClieId);
                            List<Entidad.ProductoCodigo> prodCod = ProductoCodigo.ObtenerProductosCodigoPorId(cupo.PcoId);
                            tarea1.Descripcion = cupo.Descripcion;
                            Entidad.Pais paisCliente = Territorio.ObtenerPaisPorId(cliente.PaiId);
                            Entidad.Region regionCliente = Territorio.ObtenerRegionPorId(cliente.RegId);
                            Entidad.Comuna comunaCliente = Territorio.ObtenerComunanPorId(cliente.ComId);
                            //tarea.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre + " " + regionCliente.Nombre + " " + paisCliente.Nombre;
                            tarea1.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre;
                            tarea1.Eliminado = cupo.Eliminado;
                            tarea1.EstadoCupo = cupo.EstadoCupo;
                            tarea1.FechaHoraInicio = cupo.FechaHoraInicio;
                            tarea1.FechaHoraInicioTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Minute);
                            tarea1.FechaHoraRegistro = cupo.FechaHoraRegistro;
                            tarea1.FechaHoraTermino = cupo.FechaHoraTermino;
                            tarea1.FechaHoraTerminoTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Minute);
                            tarea1.FotoCliente = "";
                            tarea1.Id = cupo.Id;
                            tarea1.NodId = cupo.NodId;
                            tarea1.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                            tarea1.ProfId = cupo.ProfId;
                            tarea1.SghId = cupo.SghId;
                            tarea1.FechaTexto = SinAlitas.Admin.Entidad.Utiles.EntregaFechaLetras(cupo.FechaHoraInicio);
                            tarea1.IdPopup = id;
                            tarea1.PcoId = cupo.PcoId;
                            tarea1.TelefonoCliente = cliente.TelefonosContacto;
                            if (tarea1.FechaHoraTermino <= cstTime)
                            {
                                tarea1.ClaseVisible = "fa fa-2x fa-hand-o-right";
                            }
                            else
                            {
                                tarea1.ClaseVisible = "fa fa-2x fa-hand-o-right hidden";
                            }
                            if (prodCod != null && prodCod.Count == 1)
                                tarea1.CodigoCliente = prodCod[0].CodigoCliente;

                            //ahora evaluamos las tareas que el profesor podría cancelar,
                            //son todas aquellas que tienen una fecha y hora de inicio mayor a 24
                            //horas al momento de la consulta, esta estaría en codiciones de ser
                            //cancelada
                            if (tarea1.FechaHoraInicio <= cstTime)
                            {
                                //no se debe mostrar la opcion de cancelar la hora
                                tarea1.ClaseVisibleCancelar = "fa fa-2x fa-trash hidden";
                                tarea1.ClaseVisibleReagendar = "fa fa-2x fa-calendar-o hidden";
                            }
                            else
                            {
                                //el profesor puede cancelar una hora con la cliente 
                                //con 4 horas de anticipacion
                                if (tarea1.FechaHoraInicio.AddHours(4) > cstTime)
                                {
                                    tarea1.ClaseVisibleCancelar = "fa fa-2x fa-trash";
                                    tarea1.ClaseVisibleReagendar = "fa fa-2x fa-calendar-o";
                                }
                                else
                                {
                                    tarea1.ClaseVisibleCancelar = "fa fa-2x fa-trash hidden";
                                    tarea1.ClaseVisibleReagendar = "fa fa-2x fa-calendar-o hidden";
                                }
                            }
                            tarea.TareasMostrar.Add(tarea1);
                        }
                    }
                    tarea.IdPopup = id;
                    id++;
                    listaRetornar.Add(tarea);
                }
            }
            if (listaRetornar != null && listaRetornar.Count > 0)
                listaRetornar = listaRetornar.OrderByDescending(p => p.Fecha).ToList();
           
            return listaRetornar;
        }

        public static List<Entidad.TareasEnvoltorio> ObtenerTareasProfesorEnvoltorioRealizadas(int fechaInicioEntera, int nodId, int profId)
        {
            int id = 1;
            List<DateTime> fechasprocesar = new List<DateTime>();
            List<Entidad.TareasEnvoltorio> listaRetornar = new List<Entidad.TareasEnvoltorio>();

            List<Entidad.Cupo> cupos = Cupo.ListarCuposDelProfesorTareas(fechaInicioEntera, nodId, profId);

            if (cupos != null && cupos.Count > 0)
            {
                foreach (Entidad.Cupo cupo in cupos)
                {
                    DateTime fechaProc = Convert.ToDateTime(cupo.FechaHoraInicio.ToShortDateString());
                    if (!fechasprocesar.Exists(p => p.Date == fechaProc))
                    {
                        if (cupo.ClieId > 0 && cupo.EstadoCupo == 3)
                            fechasprocesar.Add(fechaProc);
                    }
                }
            }
            if (fechasprocesar != null && fechasprocesar.Count > 0)
            {
                foreach (DateTime fec in fechasprocesar)
                {
                    Entidad.TareasEnvoltorio tarea = new Entidad.TareasEnvoltorio();
                    tarea.Fecha = fec;
                    tarea.FechaTexto = Entidad.Utiles.EntregaFechaLetras(fec);
                    tarea.TareasMostrar = new List<Entidad.Tareas>();
                    List<Entidad.Cupo> cuposFecha = cupos.FindAll(p => Convert.ToDateTime(p.FechaHoraInicio.ToShortDateString()) == fec && p.EstadoCupo == 3 && p.PcoId > 0);
                    if (cuposFecha != null && cuposFecha.Count > 0)
                    {
                        foreach (Entidad.Cupo cupo in cuposFecha)
                        {
                            Entidad.Tareas tarea1 = new Entidad.Tareas();
                            List<Entidad.ProductoCodigo> pcos =  ProductoCodigo.ObtenerProductosCodigoPorId(cupo.PcoId);
                            if (pcos != null && pcos.Count == 1)
                                tarea1.CodigoCliente = pcos[0].CodigoCliente;
                            tarea1.Activo = cupo.Activo;
                            tarea1.ClieId = cupo.ClieId;
                            Entidad.Cliente cliente = Cliente.ObtenerClientePorId(cupo.ClieId);
                            tarea1.Descripcion = cupo.Descripcion;
                            Entidad.Pais paisCliente = Territorio.ObtenerPaisPorId(cliente.PaiId);
                            Entidad.Region regionCliente = Territorio.ObtenerRegionPorId(cliente.RegId);
                            Entidad.Comuna comunaCliente = Territorio.ObtenerComunanPorId(cliente.ComId);
                            //tarea.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre + " " + regionCliente.Nombre + " " + paisCliente.Nombre;
                            tarea1.DireccionCliente = cliente.Direccion + " " + comunaCliente.Nombre;
                            tarea1.Eliminado = cupo.Eliminado;
                            tarea1.EstadoCupo = cupo.EstadoCupo;
                            tarea1.FechaHoraInicio = cupo.FechaHoraInicio;
                            tarea1.FechaHoraInicioTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraInicio.Minute);
                            tarea1.FechaHoraRegistro = cupo.FechaHoraRegistro;
                            tarea1.FechaHoraTermino = cupo.FechaHoraTermino;
                            tarea1.FechaHoraTerminoTexto = Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Hour) + ":" + Entidad.Utiles.RetornaMesDia(cupo.FechaHoraTermino.Minute);
                            tarea1.FotoCliente = "";
                            tarea1.Id = cupo.Id;
                            tarea1.NodId = cupo.NodId;
                            tarea1.NombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                            tarea1.ProfId = cupo.ProfId;
                            tarea1.SghId = cupo.SghId;
                            tarea1.FechaTexto = SinAlitas.Admin.Entidad.Utiles.EntregaFechaLetras(cupo.FechaHoraInicio);
                            tarea1.IdPopup = id;
                            tarea1.ClaseVisible = "fa fa-2x fa-hand-o-right pull-right hidden";
                            //cambiamos la descripción por la que aparece en las observaciones
                            List<SinAlitas.Admin.Entidad.ObservacionesProfesor> obsProfe = SinAlitas.Admin.Negocio.ObservacionesProfesor.ObtenerObservacionesCpoId(cupo.Id);
                            if (obsProfe != null && obsProfe.Count == 1)
                                tarea1.Descripcion = obsProfe[0].Observacion;
                            else
                                tarea1.Descripcion = "";

                            tarea.TareasMostrar.Add(tarea1);
                        }
                    }
                    tarea.IdPopup = id;
                    id++;
                    listaRetornar.Add(tarea);
                }
            }
            if (listaRetornar != null && listaRetornar.Count > 0)
                listaRetornar = listaRetornar.OrderByDescending(p => p.Fecha).ToList();

            return listaRetornar;
        }

    }
}

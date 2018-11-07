using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class EnvoltorioProfesorCupo
    {

        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static List<Entidad.EnvoltorioProfesorCupo> ListaProfesoresYCupos(int idComuna)
        {
            //por defecto consulta desde 23 horas más hasta 30 días en adelante

            List<Entidad.EnvoltorioProfesorCupo> listaRetornar = new List<Entidad.EnvoltorioProfesorCupo>();
            //comenzamos buscando a los profesores de la comuna
            List<Entidad.Profesor> profesores = Negocio.Profesor.ObtenerProfesores();
            if (profesores != null && profesores.Count > 0)
            {
                foreach (Entidad.Profesor profe in profesores)
                {
                    bool agrega = false;
                    if (profe.ComIdAsignada == "*")
                    {
                        //califica de inmediato
                        agrega = true;
                    }
                    else
                    {
                        string[] ids = profe.ComIdAsignada.Split(',');
                        if (ids != null && ids.Length > 0)
                            if (ids.Contains(idComuna.ToString()))
                                agrega = true;

                    }
                    
                    if (agrega)
                    {
                        StringBuilder sbInfo = new StringBuilder();
                        Entidad.EnvoltorioProfesorCupo entidad = new Entidad.EnvoltorioProfesorCupo();
                        entidad.Profesor = profe;
                        //buscamos los cupos disponibles
                        entidad.CuposProfesor = Negocio.Cupo.ListarCuposParaClientes(profe.NodId, profe.Id);
                        entidad.CuposMostrar = new List<Entidad.CupoMostrarProfesor>();
                        if (entidad.CuposProfesor != null && entidad.CuposProfesor.Count > 0)
                        {
                            //el profesor debe tener al menos 7 días disponibles
                            if (entidad.CuposProfesor.Count < 1)
                            {
                                break;
                            }
                            var fechas = entidad.CuposProfesor.GroupBy(x => x.FechaHoraInicio.ToString("dd-MM-yyyy"));
                            int contador = 1;
                            int idList = 0;
                            foreach (var group in fechas)
                            {
                                
                                DateTime fechaG = Convert.ToDateTime(group.Key.ToString());
                                int fechaEntera = Entidad.Utiles.RetornaFechaEntera(fechaG);
                                CultureInfo ci = new CultureInfo("Es-CL");
                                string dia = ci.DateTimeFormat.GetDayName(fechaG.DayOfWeek);
                                string mes = fechaG.Day.ToString();
                                string diaMes = dia + " " + mes;
                                string mesito = ci.DateTimeFormat.GetMonthName(fechaG.Month);
                                sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3} bloques</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;' onclick='Abrir({0},{1})'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3}</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //Console.WriteLine(group.Key + " " + group.Count());
                                Entidad.CupoMostrarProfesor cpo = new Entidad.CupoMostrarProfesor();
                                cpo.FechaLetras = Entidad.Utiles.EntregaFechaLetras(fechaG);
                                DateTime fechaConsultaInicio = Convert.ToDateTime(fechaG.ToShortDateString() + " 06:00");
                                DateTime fechaConsultaTermino = Convert.ToDateTime(fechaG.ToShortDateString() + " 23:00");
                                List<Entidad.Cupo> cpoProfe = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                cpo.Cupos = new List<Entidad.DetalleCupo>();
                                if (cpoProfe != null && cpoProfe.Count > 0)
                                {
                                    foreach(Entidad.Cupo cupitos in cpoProfe)
                                    {
                                        Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                                        detalle.IdSegmento = contador;
                                        detalle.Cupo = cupitos;
                                        detalle.TextoMostrar = "De " + cupitos.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cupitos.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs.";
                                        cpo.Cupos.Add(detalle);
                                        //date1.ToString("t",CultureInfo.CreateSpecificCulture("en-us"))
                                    }
                                }
                                cpo.IdSegmento = contador;
                                cpo.NombreListBox = "rptCupos_rpPrincipal_" + idList.ToString() + "_listBoxCupos_" + idList.ToString();
                                if (contador > 1)
                                    cpo.Colapsado = true;
                                contador++;
                                idList++;
                                //cpo.Cupos = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                entidad.CuposMostrar.Add(cpo);
                            }
                        }
                        entidad.ProximosDias = sbInfo.ToString();
                        //obtener las estrellas del profesor
                        
                        entidad.EstrellasProfesor = EntregaEstrellas(profe.Id);
                        entidad.CantidadVotos = 0;
                        listaRetornar.Add(entidad);


                    }

                }
            }



            return listaRetornar;
        }

        public static List<Entidad.EnvoltorioProfesorCupo> ListaProfesoresYCupos(int idComuna, int pcoId)
        {
            //por defecto consulta desde 23 horas más hasta 30 días en adelante
            
            List<Entidad.EnvoltorioProfesorCupo> listaRetornar = new List<Entidad.EnvoltorioProfesorCupo>();
            int idProfe = 0;
            //comenzamos buscando a los profesores de la comuna
            if (pcoId > 0)
            {
                List<Entidad.Cupo> pco = Negocio.Cupo.ListarCuposProductoCodigo(pcoId);
                //List<Entidad.ProductoCodigo> pco = Negocio.ProductoCodigo.ObtenerProductosCodigoPorId(pcoId);
                if (pco != null && pco.Count > 0)
                {
                    idProfe = pco.FirstOrDefault().ProfId;
                }

            }

            List<Entidad.Profesor> profesores = new List<Entidad.Profesor>();
            if (idProfe == 0)
                profesores =  Negocio.Profesor.ObtenerProfesores();
            else
            {
                profesores.Add(Negocio.Profesor.ObtenerProfesorPord(idProfe));
            }
            
            if (profesores != null && profesores.Count > 0)
            {
                int contadorF = 0;
                foreach (Entidad.Profesor profe in profesores)
                {
                    StringBuilder comunasAsignadas = new StringBuilder();
                    bool agrega = false;
                    if (profe.ComIdAsignada == "*")
                    {
                        //califica de inmediato
                        agrega = true;
                        comunasAsignadas.Append("Todas");
                    }
                    else
                    {
                        string[] ids = profe.ComIdAsignada.Split(',');
                        if (ids != null && ids.Length > 0)
                            if (ids.Contains(idComuna.ToString()))
                                agrega = true;
                        //ahora agregamos las comunas a las que pertenece el Profesor
                        if (ids != null && ids.Length > 0)
                        {
                            foreach(string c in ids.ToList())
                            {
                                if (c != "")
                                {
                                    Entidad.Comuna comunaA = SinAlitas.Admin.Negocio.Territorio.ObtenerComunanPorId(int.Parse(c));
                                    if (comunaA != null && comunaA.Id > 0)
                                    {
                                        comunasAsignadas.Append(comunaA.Nombre);
                                        comunasAsignadas.Append(", ");

                                    }
                                }
                            }
                        }

                    }

                    if (agrega)
                    {
                        StringBuilder sbInfo = new StringBuilder();
                        StringBuilder sbSemanas = new StringBuilder();
                        Entidad.EnvoltorioProfesorCupo entidad = new Entidad.EnvoltorioProfesorCupo();
                        entidad.Profesor = profe;
                        //buscamos los cupos ya tomados
                        List<Entidad.Cupo> cuposTomados = Negocio.Cupo.ListarCuposProductoCodigo(pcoId);
                        if (cuposTomados != null && cuposTomados.Count > 0)
                        {
                            entidad.PrimerCupo = cuposTomados.FirstOrDefault().FechaHoraInicio;
                            entidad.DeshabilitarControlFecha = true;
                        }
                        else
                        {
                            entidad.PrimerCupo = DateTime.Now;
                        }
                        entidad.IdControl = "ASPxCallbackPanel1_rptCupos_dtFecha_" + contadorF.ToString() + "_I";
                        //buscamos los cupos disponibles
                        entidad.CuposProfesor = Negocio.Cupo.ListarCuposParaClientes(profe.NodId, profe.Id);
                        //tratemos de ordenar en semanas los cupos consultados.
                        //seria algo asi
                        //semana 1 del 12-12-2017 al 19-12-2017 40 cupos, semana 2 del 12-12-2017 al 19-12-2017 40 cupos
                        if (entidad.CuposProfesor != null && entidad.CuposProfesor.Count > 0)
                        {
                            DateTime primeraFecha = entidad.CuposProfesor.Min(p => p.FechaHoraInicio);
                            List<Entidad.Semanas> semanas =  SinAlitas.Admin.Entidad.Utiles.RetornaSemanas(primeraFecha);
                            foreach (SinAlitas.Admin.Entidad.Semanas semana in semanas)
                            {
                                semana.DiasDisponibles = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= semana.FechaInicioSemana && p.FechaHoraTermino <= semana.FechaTerminoSemana && p.ClieId == 0).Count();

                            }
                            
                            entidad.SemanasArr = semanas;

                        }
                        else
                        {
                            sbSemanas.Append("No tiene cupos disponibles");
                        }


                        entidad.CuposMostrar = new List<Entidad.CupoMostrarProfesor>();
                        if (entidad.CuposProfesor != null && entidad.CuposProfesor.Count > 0)
                        {
                            //el profesor debe tener al menos 7 días disponibles
                            if (entidad.CuposProfesor.Count < 1)
                            {
                                break;
                            }
                            var fechas = entidad.CuposProfesor.GroupBy(x => x.FechaHoraInicio.ToString("dd-MM-yyyy"));
                            int contador = 1;
                            int idList = 0;
                            foreach (var group in fechas)
                            {

                                DateTime fechaG = Convert.ToDateTime(group.Key.ToString());
                                int fechaEntera = Entidad.Utiles.RetornaFechaEntera(fechaG);
                                CultureInfo ci = new CultureInfo("Es-CL");
                                string dia = ci.DateTimeFormat.GetDayName(fechaG.DayOfWeek);
                                string mes = fechaG.Day.ToString();
                                string diaMes = dia + " " + mes;
                                string mesito = ci.DateTimeFormat.GetMonthName(fechaG.Month);
                                sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3} bloques</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;' onclick='Abrir({0},{1})'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3}</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //Console.WriteLine(group.Key + " " + group.Count());
                                Entidad.CupoMostrarProfesor cpo = new Entidad.CupoMostrarProfesor();
                                cpo.FechaLetras = Entidad.Utiles.EntregaFechaLetras(fechaG);
                                DateTime fechaConsultaInicio = Convert.ToDateTime(fechaG.ToShortDateString() + " 06:00");
                                DateTime fechaConsultaTermino = Convert.ToDateTime(fechaG.ToShortDateString() + " 23:00");
                                List<Entidad.Cupo> cpoProfe = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                cpo.Cupos = new List<Entidad.DetalleCupo>();
                                if (cpoProfe != null && cpoProfe.Count > 0)
                                {
                                    foreach (Entidad.Cupo cupitos in cpoProfe)
                                    {
                                        Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                                        detalle.IdSegmento = contador;
                                        detalle.Cupo = cupitos;
                                        detalle.TextoMostrar = "De " + cupitos.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cupitos.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs.";
                                        cpo.Cupos.Add(detalle);
                                        //date1.ToString("t",CultureInfo.CreateSpecificCulture("en-us"))
                                    }
                                }
                                cpo.IdSegmento = contador;
                                cpo.NombreListBox = "rptCupos_rpPrincipal_" + idList.ToString() + "_listBoxCupos_" + idList.ToString();
                                if (contador > 1)
                                    cpo.Colapsado = true;
                                contador++;
                                idList++;
                                //cpo.Cupos = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                entidad.CuposMostrar.Add(cpo);
                            }
                            
                        }
                        entidad.ProximosDias = sbInfo.ToString();
                        //obtener las estrellas del profesor

                        entidad.EstrellasProfesor = EntregaEstrellas(profe.Id);
                        entidad.CantidadVotos = 0;
                        //agregamos las comunas
                        if (comunasAsignadas.ToString() != "Todas")
                        {
                            //le quitamos los ultimos caracteres
                            string reparada = comunasAsignadas.ToString().Remove(comunasAsignadas.ToString().Length - 2, 2);
                            entidad.Comunas = reparada;
                        }
                        else
                            entidad.Comunas = comunasAsignadas.ToString();
                        //agregamos las semanas
                        entidad.Semanas = sbSemanas.ToString();
                        listaRetornar.Add(entidad);
                        contadorF++;

                    }

                }
            }



            return listaRetornar;
        }

        public static List<Entidad.EnvoltorioProfesorCupo> ListaProfesoresYCupos()
        {
            //por defecto consulta desde 23 horas más hasta 30 días en adelante

            List<Entidad.EnvoltorioProfesorCupo> listaRetornar = new List<Entidad.EnvoltorioProfesorCupo>();
            //comenzamos buscando a los profesores de la comuna
            List<Entidad.Profesor> profesores = Negocio.Profesor.ObtenerProfesores();
            if (profesores != null && profesores.Count > 0)
            {
                foreach (Entidad.Profesor profe in profesores)
                {
                    bool agrega = true;
                    
                    if (agrega)
                    {
                        StringBuilder sbInfo = new StringBuilder();
                        Entidad.EnvoltorioProfesorCupo entidad = new Entidad.EnvoltorioProfesorCupo();
                        entidad.Profesor = profe;
                        //buscamos los cupos disponibles
                        entidad.CuposProfesor = Negocio.Cupo.ListarCuposParaClientes(profe.NodId, profe.Id);
                        entidad.CuposMostrar = new List<Entidad.CupoMostrarProfesor>();
                        if (entidad.CuposProfesor != null && entidad.CuposProfesor.Count > 0)
                        {
                            //el profesor debe tener al menos 7 días disponibles
                            if (entidad.CuposProfesor.Count < 1)
                            {
                                break;
                            }
                            var fechas = entidad.CuposProfesor.GroupBy(x => x.FechaHoraInicio.ToString("dd-MM-yyyy"));
                            int contador = 1;
                            int idList = 0;
                            foreach (var group in fechas)
                            {

                                DateTime fechaG = Convert.ToDateTime(group.Key.ToString());
                                int fechaEntera = Entidad.Utiles.RetornaFechaEntera(fechaG);
                                CultureInfo ci = new CultureInfo("Es-CL");
                                string dia = ci.DateTimeFormat.GetDayName(fechaG.DayOfWeek);
                                string mes = fechaG.Day.ToString();
                                string diaMes = dia + " " + mes;
                                string mesito = ci.DateTimeFormat.GetMonthName(fechaG.Month);
                                sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3} bloques</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;' onclick='Abrir({0},{1})'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3}</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //Console.WriteLine(group.Key + " " + group.Count());
                                Entidad.CupoMostrarProfesor cpo = new Entidad.CupoMostrarProfesor();
                                cpo.FechaLetras = Entidad.Utiles.EntregaFechaLetras(fechaG);
                                DateTime fechaConsultaInicio = Convert.ToDateTime(fechaG.ToShortDateString() + " 06:00");
                                DateTime fechaConsultaTermino = Convert.ToDateTime(fechaG.ToShortDateString() + " 23:00");
                                List<Entidad.Cupo> cpoProfe = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                cpo.Cupos = new List<Entidad.DetalleCupo>();
                                if (cpoProfe != null && cpoProfe.Count > 0)
                                {
                                    foreach (Entidad.Cupo cupitos in cpoProfe)
                                    {
                                        Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                                        detalle.IdSegmento = contador;
                                        detalle.Cupo = cupitos;
                                        detalle.TextoMostrar = "De " + cupitos.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cupitos.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs.";
                                        cpo.Cupos.Add(detalle);
                                        //date1.ToString("t",CultureInfo.CreateSpecificCulture("en-us"))
                                    }
                                }
                                cpo.IdSegmento = contador;
                                cpo.NombreListBox = "rptCupos_rpPrincipal_" + idList.ToString() + "_listBoxCupos_" + idList.ToString();
                                if (contador > 1)
                                    cpo.Colapsado = true;
                                contador++;
                                idList++;
                                //cpo.Cupos = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                entidad.CuposMostrar.Add(cpo);
                            }
                        }
                        entidad.ProximosDias = sbInfo.ToString();
                        //obtener las estrellas del profesor

                        entidad.EstrellasProfesor = EntregaEstrellas(profe.Id);
                        entidad.CantidadVotos = 0;
                        listaRetornar.Add(entidad);


                    }

                }
            }



            return listaRetornar;
        }
        public static int EntregaEstrellas(int profId)
        {
            int retorno = 0;

            List<SinAlitas.Admin.Entidad.CalificacionesProfesor> calificaciones =
                        SinAlitas.Admin.Negocio.CalificacionesProfesor.ObtenerCalificacionesPorProfId(profId);

            if (calificaciones != null && calificaciones.Count > 0)
            {
                int evaluadas = calificaciones.Count;
                int cincoEstrellas = calificaciones.Count(p => p.Calificacion == 5);
                int totalValores = calificaciones.Sum(p => p.Calificacion);
                int promedio = 0;
                if (evaluadas > 0)
                    promedio = totalValores / evaluadas;

                retorno = promedio;
            }


            return retorno;
        }
        public static List<Entidad.EnvoltorioProfesorCupo> ListaProfesoresYCupos(int idComuna, int cantidadClases, DateTime fechaInicioSemana)
        {
            //por defecto consulta desde 23 horas más hasta 30 días en adelante

            List<Entidad.EnvoltorioProfesorCupo> listaRetornar = new List<Entidad.EnvoltorioProfesorCupo>();
            //comenzamos buscando a los profesores de la comuna
            List<Entidad.Profesor> profesores = Negocio.Profesor.ObtenerProfesores();
            if (profesores != null && profesores.Count > 0)
            {
                foreach (Entidad.Profesor profe in profesores)
                {
                    bool agrega = false;
                    if (profe.ComIdAsignada == "*")
                    {
                        //califica de inmediato
                        agrega = true;
                    }
                    else
                    {
                        string[] ids = profe.ComIdAsignada.Split(',');
                        if (ids != null && ids.Length > 0)
                            if (ids.Contains(idComuna.ToString()))
                                agrega = true;

                    }

                    if (agrega)
                    {
                        StringBuilder sbInfo = new StringBuilder();
                        Entidad.EnvoltorioProfesorCupo entidad = new Entidad.EnvoltorioProfesorCupo();
                        entidad.Profesor = profe;
                        //buscamos los cupos disponibles
                        entidad.CuposProfesor = Negocio.Cupo.ListarCuposParaClientes(profe.NodId, profe.Id, cantidadClases, fechaInicioSemana);
                        entidad.CuposMostrar = new List<Entidad.CupoMostrarProfesor>();
                        if (entidad.CuposProfesor != null && entidad.CuposProfesor.Count > 0)
                        {
                            //el profesor debe tener al menos 7 días disponibles
                            if (entidad.CuposProfesor.Count < 1)
                            {
                                break;
                            }
                            var fechas = entidad.CuposProfesor.GroupBy(x => x.FechaHoraInicio.ToString("dd-MM-yyyy"));
                            int contador = 1;
                            int idList = 0;
                            foreach (var group in fechas)
                            {

                                DateTime fechaG = Convert.ToDateTime(group.Key.ToString());
                                int fechaEntera = Entidad.Utiles.RetornaFechaEntera(fechaG);
                                CultureInfo ci = new CultureInfo("Es-CL");
                                string dia = ci.DateTimeFormat.GetDayName(fechaG.DayOfWeek);
                                string mes = fechaG.Day.ToString();
                                string diaMes = dia + " " + mes;
                                string mesito = ci.DateTimeFormat.GetMonthName(fechaG.Month);
                                sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3} bloques</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;' onclick='Abrir({0},{1})'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3}</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //Console.WriteLine(group.Key + " " + group.Count());
                                Entidad.CupoMostrarProfesor cpo = new Entidad.CupoMostrarProfesor();
                                cpo.FechaLetras = Entidad.Utiles.EntregaFechaLetras(fechaG);
                                DateTime fechaConsultaInicio = Convert.ToDateTime(fechaG.ToShortDateString() + " 06:00");
                                DateTime fechaConsultaTermino = Convert.ToDateTime(fechaG.ToShortDateString() + " 23:00");
                                int fechaEnteraInicio = Entidad.Utiles.RetornaFechaEntera(fechaConsultaInicio);
                                List<Entidad.Cupo> cpoProfe = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                cpo.Cupos = new List<Entidad.DetalleCupo>();
                                if (cpoProfe != null && cpoProfe.Count > 0)
                                {
                                    foreach (Entidad.Cupo cupitos in cpoProfe)
                                    {
                                        Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                                        detalle.IdSegmento = contador;
                                        detalle.Cupo = cupitos;
                                        detalle.TextoMostrar = "De " + cupitos.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cupitos.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs.";
                                        cpo.Cupos.Add(detalle);
                                        //date1.ToString("t",CultureInfo.CreateSpecificCulture("en-us"))
                                    }
                                }
                                cpo.IdSegmento = contador;
                                cpo.NombreListBox = "rptCupos_rpPrincipal_" + idList.ToString() + "_listBoxCupos_" + idList.ToString();
                                cpo.FechaEnteraInicio = fechaEnteraInicio;
                                if (contador > 1)
                                    cpo.Colapsado = true;
                                contador++;
                                idList++;
                                //cpo.Cupos = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                entidad.CuposMostrar.Add(cpo);
                            }
                        }
                        if (entidad.CuposMostrar != null && entidad.CuposMostrar.Count > 0)
                        {
                            entidad.CuposMostrar = entidad.CuposMostrar.OrderBy(p => p.FechaEnteraInicio).ToList();
                        }
                        entidad.ProximosDias = sbInfo.ToString();
                        entidad.EstrellasProfesor = 5;
                        entidad.CantidadVotos = 0;
                        listaRetornar.Add(entidad);


                    }

                }
            }



            return listaRetornar;
        }

        public static List<Entidad.EnvoltorioProfesorCupo> ListaProfesoresYCupos(int idComuna, int cantidadClases, DateTime fechaInicioSemana, int profId)
        {
            //por defecto consulta desde 23 horas más hasta 30 días en adelante

            List<Entidad.EnvoltorioProfesorCupo> listaRetornar = new List<Entidad.EnvoltorioProfesorCupo>();
            //comenzamos buscando a los profesores de la comuna
            List<Entidad.Profesor> profesores = Negocio.Profesor.ObtenerProfesores();
            if (profesores != null && profesores.Count > 0)
            {
                profesores = profesores.FindAll(p => p.Id == profId);
                foreach (Entidad.Profesor profe in profesores)
                {
                    bool agrega = false;
                    if (profe.ComIdAsignada == "*")
                    {
                        //califica de inmediato
                        agrega = true;
                    }
                    else
                    {
                        string[] ids = profe.ComIdAsignada.Split(',');
                        if (ids != null && ids.Length > 0)
                            if (ids.Contains(idComuna.ToString()))
                                agrega = true;

                    }

                    if (agrega)
                    {
                        StringBuilder sbInfo = new StringBuilder();
                        Entidad.EnvoltorioProfesorCupo entidad = new Entidad.EnvoltorioProfesorCupo();
                        entidad.Profesor = profe;
                        //buscamos los cupos disponibles
                        entidad.CuposProfesor = Negocio.Cupo.ListarCuposParaClientes(profe.NodId, profe.Id, cantidadClases, fechaInicioSemana);
                        entidad.CuposMostrar = new List<Entidad.CupoMostrarProfesor>();
                        if (entidad.CuposProfesor != null && entidad.CuposProfesor.Count > 0)
                        {
                            //el profesor debe tener al menos 7 días disponibles
                            if (entidad.CuposProfesor.Count < 1)
                            {
                                break;
                            }
                            var fechas = entidad.CuposProfesor.GroupBy(x => x.FechaHoraInicio.ToString("dd-MM-yyyy"));
                            int contador = 1;
                            int idList = 0;
                            foreach (var group in fechas)
                            {

                                DateTime fechaG = Convert.ToDateTime(group.Key.ToString());
                                int fechaEntera = Entidad.Utiles.RetornaFechaEntera(fechaG);
                                CultureInfo ci = new CultureInfo("Es-CL");
                                string dia = ci.DateTimeFormat.GetDayName(fechaG.DayOfWeek);
                                string mes = fechaG.Day.ToString();
                                string diaMes = dia + " " + mes;
                                string mesito = ci.DateTimeFormat.GetMonthName(fechaG.Month);
                                sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3} bloques</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //sbInfo.AppendFormat("<div class='col-xs-3 well text-center no-padding' style='margin-bottom:0;padding:5px;' onclick='Abrir({0},{1})'><div class='col-xs-12 no-padding text-center font-muypequeno'>{2}</div><div class='col-xs-12 no-padding text-center'><span class='badge'>{3}</span></div><div class='col-xs-12 no-padding text-center font-muypequeno'>{4}</div></div>", fechaEntera.ToString(), entidad.Profesor.Id.ToString(), diaMes, group.Count().ToString(), mesito);
                                //Console.WriteLine(group.Key + " " + group.Count());
                                Entidad.CupoMostrarProfesor cpo = new Entidad.CupoMostrarProfesor();
                                cpo.FechaLetras = Entidad.Utiles.EntregaFechaLetras(fechaG);
                                DateTime fechaConsultaInicio = Convert.ToDateTime(fechaG.ToShortDateString() + " 06:00");
                                DateTime fechaConsultaTermino = Convert.ToDateTime(fechaG.ToShortDateString() + " 23:00");
                                int fechaEnteraInicio = Entidad.Utiles.RetornaFechaEntera(fechaConsultaInicio);
                                List<Entidad.Cupo> cpoProfe = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                cpo.Cupos = new List<Entidad.DetalleCupo>();
                                if (cpoProfe != null && cpoProfe.Count > 0)
                                {
                                    foreach (Entidad.Cupo cupitos in cpoProfe)
                                    {
                                        Entidad.DetalleCupo detalle = new Entidad.DetalleCupo();
                                        detalle.IdSegmento = contador;
                                        detalle.Cupo = cupitos;
                                        detalle.TextoMostrar = "De " + cupitos.FechaHoraInicio.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " a " + cupitos.FechaHoraTermino.ToString("t", CultureInfo.CreateSpecificCulture("es-CL")) + " hrs.";
                                        cpo.Cupos.Add(detalle);
                                        //date1.ToString("t",CultureInfo.CreateSpecificCulture("en-us"))
                                    }
                                }
                                cpo.IdSegmento = contador;
                                cpo.NombreListBox = "rptCupos_rpPrincipal_" + idList.ToString() + "_listBoxCupos_" + idList.ToString();
                                cpo.FechaEnteraInicio = fechaEnteraInicio;
                                if (contador > 1)
                                    cpo.Colapsado = true;
                                contador++;
                                idList++;
                                //cpo.Cupos = entidad.CuposProfesor.FindAll(p => p.FechaHoraInicio >= fechaConsultaInicio && p.FechaHoraTermino <= fechaConsultaTermino);
                                entidad.CuposMostrar.Add(cpo);
                            }
                        }
                        if (entidad.CuposMostrar != null && entidad.CuposMostrar.Count > 0)
                        {
                            entidad.CuposMostrar = entidad.CuposMostrar.OrderBy(p => p.FechaEnteraInicio).ToList();
                            foreach (Entidad.CupoMostrarProfesor cpoL in entidad.CuposMostrar)
                            {
                                if (cpoL.Cupos != null && cpoL.Cupos.Count > 0)
                                    cpoL.Cupos = cpoL.Cupos.OrderBy(x => x.Cupo.FechaHoraInicio).ToList();
                            }
                        }
                        entidad.ProximosDias = sbInfo.ToString();
                        entidad.EstrellasProfesor = 5;
                        entidad.CantidadVotos = 0;
                        listaRetornar.Add(entidad);


                    }

                }
            }



            return listaRetornar;
        }

        public static Entidad.EnvoltorioProfesorCupo ObtenerProfesorYCupos(int idProfe, int idComuna, int cantidadClases, DateTime fechaInicioSemana)
        {
            //List<Entidad.EnvoltorioProfesorCupo> lista = ListaProfesoresYCupos(idComuna, cantidadClases, fechaInicioSemana);
            List<Entidad.EnvoltorioProfesorCupo> lista = ListaProfesoresYCupos(idComuna, cantidadClases, fechaInicioSemana, idProfe);

            Entidad.EnvoltorioProfesorCupo prof = lista.Find(p => p.Profesor.Id == idProfe);

            return prof;

        }
        public static List<EnvoltorioCupoMostrar> ListaCuposProfeFecha(int idProfe, int fechaEntera)
        {
            string fechaInicio = Entidad.Utiles.RetornaFecha(fechaEntera).ToShortDateString() + " 09:00";
            string fechaTermino = Entidad.Utiles.RetornaFecha(fechaEntera).ToShortDateString() + " 23:00";

            List<EnvoltorioCupoMostrar> cupos = new List<EnvoltorioCupoMostrar>();

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
            filtro2.Valor = "0";
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PROF_ID";
            filtro3.Valor = idProfe.ToString();
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
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= Convert.ToDateTime(fechaInicio) && p.FechaHoraTermino <= Convert.ToDateTime(fechaTermino));

            if (lista2 != null && lista.Count > 0)
            {
                foreach (Entidad.Cupo cpo in lista2)
                {
                    EnvoltorioCupoMostrar env = new EnvoltorioCupoMostrar();
                    env.Id = cpo.Id;
                    env.Horas = cpo.FechaHoraInicio.ToShortTimeString() + " - " + cpo.FechaHoraTermino.ToShortTimeString();
                    cupos.Add(env);

                }
            }
            return cupos;
        }
    }
    public class EnvoltorioCupoMostrar
    {
        public int Id { get; set; }
        public string Horas { get; set; }
        public bool EsSeleccionado { get; set; }
    }
}

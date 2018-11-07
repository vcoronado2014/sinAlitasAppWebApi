using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class CrudProfesores
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static List<SinAlitas.Admin.Entidad.Profesor> ObtenerProfesores()
        {
            try
            {
                Factory fac = new Factory();

                List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Profesor>(setCnsWebLun);
                List<SinAlitas.Admin.Entidad.Profesor> lista2 = new List<Entidad.Profesor>();
                if (lista != null)
                {

                    lista2 = lista.Cast<Entidad.Profesor>().ToList();

                }
                if (lista2 != null && lista2.Count > 0)
                {
                    foreach (Entidad.Profesor prof in lista2)
                    {
                        if (prof.Fotografia == null || prof.Fotografia == string.Empty)
                            prof.Fotografia = "img/no_disponible.png";
                    }
                }
                return lista2;
            }
            catch (Exception ex)
            {
                Utiles.Log(ex);
                return new List<Entidad.Profesor>();
            }
        }
        public static string ConstruirTabla(List<Entidad.Profesor> lista)
        {

            StringBuilder sb = new StringBuilder();

            if (lista != null && lista.Count > 0)
            {
                sb.Append("<thead class='hidden-xs'><tr><th>Nombre</th><th>Email</th><th>Teléfono</th><th>Sexo</th><th></th><th></th></tr></thead>");
                sb.Append("<tfoot class='hidden-xs'><tr><th>Nombre</th><th>Email</th><th>Teléfono</th><th>Sexo</th><th></th><th></th></tr></tfoot>");
                sb.Append("<tbody class='hidden-xs'>");
                foreach (Entidad.Profesor prof in lista)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", prof.Nombres + " " + prof.PrimerApellido + " " + prof.SegundoApellido);
                    sb.AppendFormat("<td>{0}</td>", prof.Email);
                    sb.AppendFormat("<td>{0}</td>", prof.TelefonosContacto.Replace("|", "").ToString());
                    sb.AppendFormat("<td>{0}</td>", prof.Sexo);
                    
                    if (prof.Activo == 1 && prof.Eliminado == 0)
                    {
                        //data-toggle='tooltip' data-placement='top' data-original-title='Eliminar Profesor'
                        //data-toggle='tooltip' data-placement='top' data-original-title='Editar Profesor'
                        //data-toggle='tooltip' data-placement='top' data-original-title='Activar Profesor'
                        sb.AppendFormat("<td><i data-toggle='tooltip' data-placement='top' data-original-title='Editar Profesor' class='fa fa-2x fa-edit' onClick='EditarProfesor({0},{1})'></i></td>", prof.Id.ToString(), "0");
                        sb.AppendFormat("<td><i data-toggle='tooltip' data-placement='top' data-original-title='Eliminar Profesor'  class='fa fa-2x fa-user-times' style='color: red;' onClick='EliminarProfesor({0},{1})'></i></td>", prof.Id.ToString(), "1");
                    }
                    else
                    {
                        sb.AppendFormat("<td><i data-toggle='tooltip' data-placement='top' data-original-title='Editar Profesor'  class='fa fa-2x fa-edit' onClick='EditarProfesor({0},{1})'></i></td>", prof.Id.ToString(), "1");
                        sb.AppendFormat("<td><i data-toggle='tooltip' data-placement='top' data-original-title='Activar Profesor' class='fa fa-2x fa-user-plus' onClick='ActivarProfesor({0},{1})'></i></td>", prof.Id.ToString(), "0");
                    }

                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
            }

            return sb.ToString();
        }

        public static string ConstruirItem(List<Entidad.Profesor> lista)
        {

            StringBuilder sb = new StringBuilder();

            if (lista != null && lista.Count > 0)
            {
                sb.Append("<div class='col-xs-12 no-padding visible-xs'>");
                foreach (Entidad.Profesor prof in lista)
                {
                    sb.AppendFormat("<div class='col-xs-12 no-padding border-bottom-burlywood'><div class='col-xs-12 no-padding'><div class='col-xs-2'><i class='fa fa-user'></i></div><div class='col-xs-10'><strong>{0}</strong></div>", prof.Nombres + " " + prof.PrimerApellido + " " + prof.SegundoApellido);
                    sb.AppendFormat("<div class='col-xs-12 no-padding'><div class='col-xs-2'><i class='fa fa-send'></i></div><div class='col-xs-10'>{0}</div></div></div>", prof.Email);
                    sb.AppendFormat("<div class='col-xs-12 no-padding'><div class='col-xs-5'><div class='col-xs-2 no-padding'><i class='fa fa-share'></i></div><div class='col-xs-10 no-padding'>{0}</div></div><div class='col-xs-7'><div class='col-xs-2'><i class='fa fa-phone'></i></div><div class='col-xs-9 no-padding'>{1}</div></div></div>", prof.Sexo, prof.TelefonosContacto.Replace("|", "").ToString());
                    if (prof.Activo == 1 && prof.Eliminado == 0)
                    {

                        sb.AppendFormat("<div class='col-xs-12'><i data-toggle='tooltip' data-placement='top' data-original-title='Eliminar Profesor' class='fa fa-2x fa-user-times pull-right cursor-pointer' style='color: red;' onClick='EliminarProfesor({0},{1})'></i></div>", prof.Id.ToString(), "1");
                    }
                    else
                    {
                        sb.AppendFormat("<div class='col-xs-12'><i  data-toggle='tooltip' data-placement='top' data-original-title='Activar Profesor' class='fa fa-2x fa-user-plus pull-right cursor-pointer' onClick='ActivarProfesor({0},{1})'></i></div>", prof.Id.ToString(), "0");
                    }
                    sb.Append("</div>");
                }
                sb.Append("</div>");
            }

            return sb.ToString();
        }

        public static string ObtenerComunasProfesor(int idProfesor)
        {
            StringBuilder sb = new StringBuilder();
            Entidad.Profesor profe = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            List<Entidad.Comuna> comunaSantiago = Negocio.Territorio.ListarComunasPorRegion(13);
            List<Entidad.Comuna> comunaVina = Negocio.Territorio.ListarComunasPorRegion(5);
            List<Entidad.Comuna> comuna = new List<Entidad.Comuna>();
            comuna.AddRange(comunaSantiago);
            comuna.AddRange(comunaVina);

            //comunas del profe
            List<Entidad.Comuna> comunasAsignadas = new List<Entidad.Comuna>();
            List<Entidad.Comuna> comunasNOAsignadas = new List<Entidad.Comuna>();

            bool esTodas = false;
            if (profe != null && profe.Id > 0)
            {
                if (profe.ComIdAsignada != null && profe.ComIdAsignada.Length > 0)
                {
                    esTodas = profe.ComIdAsignada.Contains("*");
                    if (esTodas)
                    {
                        comunasAsignadas = comuna;
                    }
                    else
                    {
                        string[] arrComunas = profe.ComIdAsignada.Split(',');
                        if (arrComunas != null && arrComunas.Length > 0)
                        {
                            foreach(string s in arrComunas)
                            {
                                if (s != string.Empty)
                                {
                                    Entidad.Comuna agr = comuna.Find(p => p.Id == int.Parse(s));
                                    if (agr != null)
                                    {
                                        if (!comunasAsignadas.Exists(p=>p.Id == int.Parse(s)))
                                            comunasAsignadas.Add(agr);
                                    }
                                }
                            }
                        }
                        if (comunasAsignadas != null && comunasAsignadas.Count > 0)
                        {
                            foreach(Entidad.Comuna com in comuna)
                            {
                                if (!comunasAsignadas.Exists(p=>p.Id == com.Id))
                                {
                                    comunasNOAsignadas.Add(com);
                                }
                            }
                        }
                    }
                }
            }

            sb.Append("<thead><tr><th>Nombre</th><th></th></tr></thead>");
            sb.Append("<tfoot><tr><th>Nombre</th><th></th></tr></tfoot>");
            sb.Append("<tbody>");

            if (comunasAsignadas != null && comunasAsignadas.Count > 0)
            {
                foreach(Entidad.Comuna cm in comunasAsignadas)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", cm.Nombre);
                    sb.AppendFormat("<td><i class='fa fa-2x fa-user-times' style='color: red;' onClick='alert({0})'></i></td>", cm.Id.ToString());
                    sb.Append("</tr>");

                }
            }

            if (comunasNOAsignadas != null && comunasNOAsignadas.Count > 0)
            {
                foreach (Entidad.Comuna cm in comunasNOAsignadas)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", cm.Nombre);
                    sb.AppendFormat("<td><i class='fa fa-2x fa-user-plus' onClick='alert({0})'></i></td>", cm.Id.ToString());
                    sb.Append("</tr>");

                }
            }


            sb.Append("</tbody>");
            return sb.ToString();
        }

        public static string ObtenerComunasProfesor(string idComunas)
        {
            StringBuilder sb = new StringBuilder();
            //Entidad.Profesor profe = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            List<Entidad.Comuna> comunaSantiago = Negocio.Territorio.ListarComunasPorRegion(13);
            List<Entidad.Comuna> comunaVina = Negocio.Territorio.ListarComunasPorRegion(5);
            List<Entidad.Comuna> comuna = new List<Entidad.Comuna>();
            comuna.AddRange(comunaSantiago);
            comuna.AddRange(comunaVina);

            //comunas del profe
            List<Entidad.Comuna> comunasAsignadas = new List<Entidad.Comuna>();
            List<Entidad.Comuna> comunasNOAsignadas = new List<Entidad.Comuna>();

            bool esTodas = false;
            if (idComunas != null && idComunas.Length > 0)
            {
                if (idComunas != null && idComunas.Length > 0)
                {
                    esTodas = idComunas.Contains("*");
                    if (esTodas)
                    {
                        comunasAsignadas = comuna;
                    }
                    else
                    {
                        string[] arrComunas = idComunas.Split(',');
                        if (arrComunas != null && arrComunas.Length > 0)
                        {
                            foreach (string s in arrComunas)
                            {
                                if (s != string.Empty)
                                {
                                    Entidad.Comuna agr = comuna.Find(p => p.Id == int.Parse(s));
                                    if (agr != null)
                                    {
                                        if (!comunasAsignadas.Exists(p => p.Id == int.Parse(s)))
                                            comunasAsignadas.Add(agr);
                                    }
                                }
                            }
                        }
                        if (comunasAsignadas != null && comunasAsignadas.Count > 0)
                        {
                            foreach (Entidad.Comuna com in comuna)
                            {
                                if (!comunasAsignadas.Exists(p => p.Id == com.Id))
                                {
                                    comunasNOAsignadas.Add(com);
                                }
                            }
                        }
                    }
                }
            }

            sb.Append("<thead><tr><th>Nombre</th><th></th></tr></thead>");
            sb.Append("<tfoot><tr><th>Nombre</th><th></th></tr></tfoot>");
            sb.Append("<tbody>");

            if (comunasAsignadas != null && comunasAsignadas.Count > 0)
            {
                foreach (Entidad.Comuna cm in comunasAsignadas)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", cm.Nombre);
                    sb.AppendFormat("<td><i class='fa fa-2x fa-user-times' style='color: red;' onClick='alert({0})'></i></td>", cm.Id.ToString());
                    sb.Append("</tr>");

                }
            }

            if (comunasNOAsignadas != null && comunasNOAsignadas.Count > 0)
            {
                foreach (Entidad.Comuna cm in comunasNOAsignadas)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", cm.Nombre);
                    sb.AppendFormat("<td><i class='fa fa-2x fa-user-plus' onClick='alert({0})'></i></td>", cm.Id.ToString());
                    sb.Append("</tr>");

                }
            }


            sb.Append("</tbody>");
            return sb.ToString();
        }

        public static string ObtenerComunasProfesorAsociadas(string idComunas)
        {
            StringBuilder sb = new StringBuilder();
            //Entidad.Profesor profe = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            List<Entidad.Comuna> comunaSantiago = Negocio.Territorio.ListarComunasPorRegion(13);
            List<Entidad.Comuna> comunaVina = Negocio.Territorio.ListarComunasPorRegion(5);
            List<Entidad.Comuna> comuna = new List<Entidad.Comuna>();
            comuna.AddRange(comunaSantiago);
            comuna.AddRange(comunaVina);

            //comunas del profe
            List<Entidad.Comuna> comunasAsignadas = new List<Entidad.Comuna>();

            bool esTodas = false;
            if (idComunas != null && idComunas.Length > 0)
            {
                if (idComunas != null && idComunas.Length > 0)
                {
                    esTodas = idComunas.Contains("*");
                    if (esTodas)
                    {
                        comunasAsignadas = comuna;
                    }
                    else
                    {
                        string[] arrComunas = idComunas.Split(',');
                        if (arrComunas != null && arrComunas.Length > 0)
                        {
                            foreach (string s in arrComunas)
                            {
                                if (s != string.Empty)
                                {
                                    Entidad.Comuna agr = comuna.Find(p => p.Id == int.Parse(s));
                                    if (agr != null)
                                    {
                                        if (!comunasAsignadas.Exists(p => p.Id == int.Parse(s)))
                                            comunasAsignadas.Add(agr);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            sb.Append("<thead><tr><th>Nombre</th><th></th></tr></thead>");
            sb.Append("<tfoot><tr><th>Nombre</th><th></th></tr></tfoot>");
            sb.Append("<tbody>");

            if (comunasAsignadas != null && comunasAsignadas.Count > 0)
            {
                foreach (Entidad.Comuna cm in comunasAsignadas)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", cm.Nombre);
                    sb.AppendFormat("<td><i class='fa fa-2x fa-user-times' style='color: red;' onClick='alert({0})'></i></td>", cm.Id.ToString());
                    sb.Append("</tr>");

                }
            }


            sb.Append("</tbody>");
            return sb.ToString();
        }

        public static List<Entidad.Comuna> ObtenerComunasProfesorAsociadasLista(string idComunas)
        {
            StringBuilder sb = new StringBuilder();
            //Entidad.Profesor profe = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            List<Entidad.Comuna> comunaSantiago = Negocio.Territorio.ListarComunasPorRegionSin(13);
            List<Entidad.Comuna> comunaVina = Negocio.Territorio.ListarComunasPorRegionSin(5);
            List<Entidad.Comuna> comuna = new List<Entidad.Comuna>();
            comuna.AddRange(comunaSantiago);
            comuna.AddRange(comunaVina);

            //comunas del profe
            List<Entidad.Comuna> comunasAsignadas = new List<Entidad.Comuna>();

            bool esTodas = false;
            if (idComunas != null && idComunas.Length > 0)
            {
                if (idComunas != null && idComunas.Length > 0)
                {
                    esTodas = idComunas.Contains("*");
                    if (esTodas)
                    {
                        comunasAsignadas = comuna;
                    }
                    else
                    {
                        string[] arrComunas = idComunas.Split(',');
                        if (arrComunas != null && arrComunas.Length > 0)
                        {
                            foreach (string s in arrComunas)
                            {
                                if (s != string.Empty)
                                {
                                    Entidad.Comuna agr = comuna.Find(p => p.Id == int.Parse(s));
                                    if (agr != null)
                                    {
                                        if (!comunasAsignadas.Exists(p => p.Id == int.Parse(s)))
                                            comunasAsignadas.Add(agr);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return comunasAsignadas;
        }

        public static string ObtenerComunasProfesorNOAsignadas(string idComunas)
        {
            StringBuilder sb = new StringBuilder();
            //Entidad.Profesor profe = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            List<Entidad.Comuna> comunaSantiago = Negocio.Territorio.ListarComunasPorRegion(13);
            List<Entidad.Comuna> comunaVina = Negocio.Territorio.ListarComunasPorRegion(5);
            List<Entidad.Comuna> comuna = new List<Entidad.Comuna>();
            comuna.AddRange(comunaSantiago);
            comuna.AddRange(comunaVina);

            //comunas del profe
            List<Entidad.Comuna> comunasAsignadas = new List<Entidad.Comuna>();
            List<Entidad.Comuna> comunasNOAsignadas = new List<Entidad.Comuna>();

            bool esTodas = false;
            if (idComunas != null && idComunas.Length > 0)
            {
                if (idComunas != null && idComunas.Length > 0)
                {
                    esTodas = idComunas.Contains("*");
                    if (esTodas)
                    {
                        comunasAsignadas = comuna;
                    }
                    else
                    {
                        string[] arrComunas = idComunas.Split(',');
                        if (arrComunas != null && arrComunas.Length > 0)
                        {
                            foreach (string s in arrComunas)
                            {
                                if (s != string.Empty)
                                {
                                    Entidad.Comuna agr = comuna.Find(p => p.Id == int.Parse(s));
                                    if (agr != null)
                                    {
                                        if (!comunasAsignadas.Exists(p => p.Id == int.Parse(s)))
                                            comunasAsignadas.Add(agr);
                                    }
                                }
                            }
                        }
                        if (comunasAsignadas != null && comunasAsignadas.Count > 0)
                        {
                            foreach (Entidad.Comuna com in comuna)
                            {
                                if (!comunasAsignadas.Exists(p => p.Id == com.Id))
                                {
                                    comunasNOAsignadas.Add(com);
                                }
                            }
                        }
                    }
                }
            }

            sb.Append("<thead><tr><th>Nombre</th><th></th></tr></thead>");
            sb.Append("<tfoot><tr><th>Nombre</th><th></th></tr></tfoot>");
            sb.Append("<tbody>");


            if (comunasNOAsignadas != null && comunasNOAsignadas.Count > 0)
            {
                foreach (Entidad.Comuna cm in comunasNOAsignadas)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", cm.Nombre);
                    sb.AppendFormat("<td><i class='fa fa-2x fa-user-plus' onClick='alert({0})'></i></td>", cm.Id.ToString());
                    sb.Append("</tr>");

                }
            }


            sb.Append("</tbody>");
            return sb.ToString();
        }

        public static List<Entidad.Comuna> ObtenerComunasProfesorNOAsignadasLista(string idComunas)
        {
            StringBuilder sb = new StringBuilder();
            //Entidad.Profesor profe = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            List<Entidad.Comuna> comunaSantiago = Negocio.Territorio.ListarComunasPorRegionSin(13);
            List<Entidad.Comuna> comunaVina = Negocio.Territorio.ListarComunasPorRegionSin(5);
            List<Entidad.Comuna> comuna = new List<Entidad.Comuna>();
            comuna.AddRange(comunaSantiago);
            comuna.AddRange(comunaVina);

            //comunas del profe
            List<Entidad.Comuna> comunasAsignadas = new List<Entidad.Comuna>();
            List<Entidad.Comuna> comunasNOAsignadas = new List<Entidad.Comuna>();

            bool esTodas = false;
            if (idComunas != null && idComunas.Length > 0)
            {
                if (idComunas != null && idComunas.Length > 0)
                {
                    esTodas = idComunas.Contains("*");
                    if (esTodas)
                    {
                        comunasAsignadas = comuna;
                        comunasNOAsignadas = comuna;
                    }
                    else
                    {
                        string[] arrComunas = idComunas.Split(',');
                        if (arrComunas != null && arrComunas.Length > 0)
                        {
                            foreach (string s in arrComunas)
                            {
                                if (s != string.Empty)
                                {
                                    Entidad.Comuna agr = comuna.Find(p => p.Id == int.Parse(s));
                                    if (agr != null)
                                    {
                                        if (!comunasAsignadas.Exists(p => p.Id == int.Parse(s)))
                                            comunasAsignadas.Add(agr);
                                    }
                                }
                            }
                        }
                        if (comunasAsignadas != null && comunasAsignadas.Count > 0)
                        {
                            foreach (Entidad.Comuna com in comuna)
                            {
                                if (!comunasAsignadas.Exists(p => p.Id == com.Id))
                                {
                                    comunasNOAsignadas.Add(com);
                                }
                            }
                        }
                    }
                }
            }

            return comunasNOAsignadas;
        }

        public static bool ModificarComunasProfesor(bool agrega, int idProfesor, int idComuna)
        {
            bool retorno = false;
            StringBuilder nuevoArrF = new StringBuilder();

            Entidad.Profesor profesor = Negocio.Profesor.ObtenerProfesorPorIdSP(idProfesor);
            if (profesor != null && profesor.Id > 0)
            {
                if (profesor.ComIdAsignada == "*")
                {
                    profesor.ComIdAsignada = idComuna.ToString();
                }
                else
                {
                    if (!agrega)
                    {
                        StringBuilder nuevoArr = new StringBuilder();
                        string[] arrActual = profesor.ComIdAsignada.Split(',');
                        if (arrActual != null && arrActual.Length > 0)
                        {
                            foreach (string s in arrActual)
                            {
                                if (s != "")
                                {
                                    if (s != idComuna.ToString())
                                    {
                                        nuevoArr.Append(s);
                                        nuevoArr.Append(",");
                                    }
                                }
                            }
                        }
                        if (nuevoArr.ToString().Length > 0)
                        {
                            if (nuevoArr.ToString().Substring(nuevoArr.ToString().Length-1, 1) ==",")
                            {
                                nuevoArrF.Append(nuevoArr.ToString().Remove(nuevoArr.ToString().Length - 1, 1).ToString());
                            }
                        }
                        profesor.ComIdAsignada = nuevoArrF.ToString();

                    }
                    else
                    {
                        profesor.ComIdAsignada = profesor.ComIdAsignada + "," + idComuna.ToString();
                    }
                }
            }

            VCFramework.Negocio.Factory.Factory fac = new Factory();
            if (fac.Update<Entidad.Profesor>(profesor, setCnsWebLun) >= 0)
                retorno = true;

            return retorno;
        }
        public static int GuardarNuevoProfesor(Entidad.Profesor profesor)
        {
            Factory fac = new Factory();
            int retorno = fac.Insertar<Entidad.Profesor>(profesor, setCnsWebLun);

            return retorno;
        }
        public static string GuardarProfesor(Entidad.Profesor profesor)
        {
            string retorno = "";
            try
            {

                Factory fac = new Factory();

                if (profesor.Id > 0)
                {
                    if (fac.Update<Entidad.Profesor>(profesor, setCnsWebLun) >= 0)
                    {
                        retorno = "Modificado con éxito.";
                    }
                    else
                        retorno = "Error al guardar profesor";
                }
                else
                {
                    profesor.Activo = 1;
                    profesor.Eliminado = 0;
                    profesor.Banco = "";
                    profesor.ComId = 13;
                    profesor.Codigo = "";
                    profesor.Direccion = "";
                    profesor.FechaNacimiento = Convert.ToDateTime("01-01-1990");
                    profesor.Fotografia = "";
                    profesor.NodId = 1;
                    profesor.PaiId = 1;
                    profesor.RegId = 13;
                    profesor.RolId = 3;
                    profesor.TipoCuenta = 0;
                    profesor.Password = profesor.Rut;
                    profesor.NombreUsuario = profesor.Rut;
                    if (fac.Insertar<Entidad.Profesor>(profesor, setCnsWebLun) >= 0)
                    {
                        retorno = "Guardado con éxito.";
                    }
                    else
                        retorno = "Error al guardar profesor";
                }
            }
            catch(Exception ex)
            {
                Utiles.Log(ex);
                retorno = ex.Message;
            }

            return retorno;
        }

        public static SinAlitas.Admin.Entidad.Profesor ObtenerProfesorPorRut(string rut)
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
            filtro2.Campo = "RUT";
            filtro2.Valor = rut.ToUpper().ToString();
            filtro2.TipoDato = TipoDatoGeneral.Varchar;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);


            SinAlitas.Admin.Entidad.Profesor retorno = new Entidad.Profesor();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Profesor>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Profesor> lista2 = new List<Entidad.Profesor>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Profesor>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static List<SinAlitas.Admin.Entidad.Cupo> ObtenerCuposPendientes(int profId)
        {
            DateTime hoy = DateTime.Now;

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "PROF_ID";
            filtro.Valor = profId.ToString();
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "ELIMINADO";
            filtro2.Valor = "0";
            filtro2.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);

            SinAlitas.Admin.Entidad.Cupo retorno = new Entidad.Cupo();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cupo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cupo> lista2 = new List<Entidad.Cupo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cupo>().ToList();
            }


            if (lista2 != null && lista2.Count > 0)
            {
                //evaluar fecha y clie_id > 0
                lista2 = lista2.FindAll(p => p.FechaHoraInicio >= hoy && p.ClieId > 0 && p.EstadoCupo != 3);
            }

            return lista2;
        }

        public static List<Entidad.ReportePagosEnvoltorio> ObtenerReportePagos(DateTime fechaInicio, DateTime fechaTermino, int cantidadClases)
        {
            
            //si la cantidad de clases es 0 son todas
            List<Entidad.ReportePagosEnvoltorio> lista = new List<Entidad.ReportePagosEnvoltorio>();

            List<Entidad.Profesor> profesores = ObtenerProfesores();

            if (profesores != null && profesores.Count > 0)
            {

                foreach (Entidad.Profesor profe in profesores)
                {
                    bool agregar = false;
                    Entidad.ReportePagosEnvoltorio entidad = new Entidad.ReportePagosEnvoltorio();
                    entidad.Profesor = profe;
                    entidad.DatosPack = new List<Entidad.DatosPago>();
                    List<Entidad.DatosPago> datospago = ObtenerDatosPago(profe.Id);
                    datospago = datospago.FindAll(p => p.FechaPrimeraClase >= fechaInicio && p.FechaPrimeraClase <= fechaTermino);
                    if (cantidadClases > 0)
                    {
                        //todas
                        datospago = datospago.FindAll(p => p.ClasesCerradas == cantidadClases);

                    }
                    if (datospago.Count > 0)
                        agregar = true;
                    entidad.DatosPack = datospago;
                    if (agregar)
                        lista.Add(entidad);
                }
            }

            return lista;
        }

        public static List<Entidad.DatosPago> ObtenerDatosPago(int idProfesor)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            List<Entidad.DatosPago> lista = new List<Entidad.DatosPago>();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            //cmd = new SqlCommand("select * from RELUN_REGISTRO_LUN where ECOL_ID = @ENCO_ID and ELIMINADO = 1", conn);
            cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(conexionStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OBTENER_DATOS_PAGO";
            
            cmd.Parameters.AddWithValue("@PROF_ID", idProfesor);
            cmd.Connection.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int CODIGO_CLIENTE = rdr.GetOrdinal("CODIGO_CLIENTE");
                int NOMBRE_CLIENTE = rdr.GetOrdinal("NOMBRE_CLIENTE");
                int PRIMERA_FECEHA = rdr.GetOrdinal("PRIMERA_FECHA");
                int TOTAL = rdr.GetOrdinal("TOTAL");
                int CERRADAS = rdr.GetOrdinal("CERRADAS");
                int ESTA_PAGADO = rdr.GetOrdinal("ESTA_PAGADO");

                try
                {
                    while (rdr.Read())
                    {
                        Entidad.DatosPago lun = new Entidad.DatosPago();
                        lun.CodigoPack = rdr.GetString(CODIGO_CLIENTE);
                        lun.NombreCliente = rdr.IsDBNull(NOMBRE_CLIENTE) ? "" : rdr.GetString(NOMBRE_CLIENTE);
                        lun.FechaPrimeraClase = rdr.IsDBNull(PRIMERA_FECEHA) ? DateTime.MinValue : rdr.GetDateTime(PRIMERA_FECEHA);
                        lun.ClasesCerradas = rdr.IsDBNull(CERRADAS) ? 0 : rdr.GetInt32(CERRADAS);
                        lun.TotalClases = rdr.IsDBNull(TOTAL) ? 0 : rdr.GetInt32(TOTAL);
                        lun.ClasesPendientes = lun.TotalClases - lun.ClasesCerradas;
                        int PAGADO = rdr.IsDBNull(ESTA_PAGADO) ? 0 : rdr.GetInt32(ESTA_PAGADO);
                        if (PAGADO == 1)
                        {
                            lun.EstaPagado = "Si";
                            lun.EstiloPagado = "col-xs-3 huge text-center fg-white bg-gray";
                        }
                        else
                        {
                            lun.EstaPagado = "No";
                            lun.EstiloPagado = "col-xs-3 huge text-center fg-red bg-gray";
                        }

                        lista.Add(lun);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        public static List<Entidad.DatosPago> ObtenerDatosPagoCompleto(DateTime fechaInicio, DateTime fechaTermino, int cantidadClases)
        {
            if (fechaInicio == DateTime.MinValue)
                fechaInicio = DateTime.Now.AddDays(-25);
            if (fechaTermino == DateTime.MinValue)
                fechaTermino = DateTime.Now;
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            List<Entidad.DatosPago> lista = new List<Entidad.DatosPago>();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            //cmd = new SqlCommand("select * from RELUN_REGISTRO_LUN where ECOL_ID = @ENCO_ID and ELIMINADO = 1", conn);
            cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(conexionStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OBTENER_DATOS_PAGO_COMPLETO";
            cmd.Parameters.AddWithValue("@fecha_inio", fechaInicio);
            cmd.Parameters.AddWithValue("@fecha_termino", fechaTermino);
            cmd.Parameters.AddWithValue("@cantidad_clases", cantidadClases);

            cmd.Connection.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int CODIGO_CLIENTE = rdr.GetOrdinal("CODIGO_CLIENTE");
                int PROFESOR = rdr.GetOrdinal("PROFESOR");
                int NOMBRE_CLIENTE = rdr.GetOrdinal("NOMBRE_CLIENTE");
                int PRIMERA_FECEHA = rdr.GetOrdinal("PRIMERA_FECHA");
                int TOTAL = rdr.GetOrdinal("TOTAL");
                int CERRADAS = rdr.GetOrdinal("CERRADAS");
                int ESTA_PAGADO = rdr.GetOrdinal("ESTA_PAGADO");

                try
                {
                    while (rdr.Read())
                    {
                        Entidad.DatosPago lun = new Entidad.DatosPago();
                        lun.CodigoPack = rdr.GetString(CODIGO_CLIENTE);
                        lun.NombreProfesor = rdr.GetString(PROFESOR);
                        lun.NombreCliente = rdr.IsDBNull(NOMBRE_CLIENTE) ? "" : rdr.GetString(NOMBRE_CLIENTE);
                        lun.FechaPrimeraClase = rdr.IsDBNull(PRIMERA_FECEHA) ? DateTime.MinValue : rdr.GetDateTime(PRIMERA_FECEHA);
                        lun.ClasesCerradas = rdr.IsDBNull(CERRADAS) ? 0 : rdr.GetInt32(CERRADAS);
                        lun.TotalClases = rdr.IsDBNull(TOTAL) ? 0 : rdr.GetInt32(TOTAL);
                        lun.ClasesPendientes = lun.TotalClases - lun.ClasesCerradas;
                        int PAGADO = rdr.IsDBNull(ESTA_PAGADO) ? 0 : rdr.GetInt32(ESTA_PAGADO);
                        if (PAGADO == 1)
                        {
                            lun.EstaPagado = "Si";
                            lun.EstiloPagado = "col-xs-3 huge text-center fg-white bg-gray";
                        }
                        else
                        {
                            lun.EstaPagado = "No";
                            lun.EstiloPagado = "col-xs-3 huge text-center fg-red bg-gray";
                        }

                        lista.Add(lun);
                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            if (lista != null)
                lista = lista.OrderByDescending(p => p.NombreProfesor).ToList();

            return lista;
        }

    }
}

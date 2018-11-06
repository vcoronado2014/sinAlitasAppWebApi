using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VCFramework.NegocioMySql
{
    public class Utiles
    {
        public const string HTML_DOCTYPE = "text/html";
        public const string JSON_DOCTYPE = "application/json";
        public const string XML_DOCTYPE = "application/xml";

        public static String DiferenciaFechas(DateTime newdt, DateTime olddt)
        {
            Int32 anios;
            Int32 meses;
            Int32 dias;
            Int32 horas;
            Int32 minutos;
            Int32 segundos;
            String str = "";

            anios = (newdt.Year - olddt.Year);
            meses = (newdt.Month - olddt.Month);
            dias = (newdt.Day - olddt.Day);
            horas = (newdt.Hour - olddt.Hour);
            minutos = (newdt.Minute - olddt.Minute);
            segundos = (newdt.Second - olddt.Second);

            if (meses < 0)
            {
                anios -= 1;
                meses += 12;
            }
            if (dias < 0)
            {
                meses -= 1;
                dias += DateTime.DaysInMonth(newdt.Year, newdt.Month);
            }

            if (anios < 0)
            {
                return "Fecha Invalida";
            }
            if (anios > 0)
                str = str + anios.ToString() + " años ";
            if (meses > 0)
                str = str + meses.ToString() + " meses ";
            if (dias > 0)
                str = str + dias.ToString() + " dias ";
            if (horas > 0)
                str = str + horas.ToString() + " horas ";
            if (minutos > 0)
                str = str + minutos.ToString() + " minutos ";
            if (anios == 0 && meses == 0 && dias == 0 && horas == 0 && minutos == 0)
                str = segundos.ToString() + " segundos ";


            return "hace " + str;
        }
        public static int EntregaEntero(string valorDosDigitos)
        {
            int retorno = 0;

            if (valorDosDigitos.Length == 2)
            {
                string valorUno = valorDosDigitos.Substring(0, 1);
                string valorDos = valorDosDigitos.Substring(1, 1);
                if (valorUno == "0")
                {
                    retorno = int.Parse(valorDos);
                }
                else
                {
                    retorno = int.Parse(valorDosDigitos);
                }
            }

            return retorno;
        }
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        public static string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        public static bool ValidaEmail(string email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public static string ConnStr()
        {
            string cns = "Driver={MySQL ODBC 5.1 Driver};Server=MYSQL5011.Smarterasp.net;Database=db_9dac90_cole;User=9dac90_cole;Password=antonia2006;Option=3;";
            if (System.Configuration.ConfigurationManager.ConnectionStrings["BDColegioSql"].ConnectionString != null)
                cns = System.Configuration.ConfigurationManager.ConnectionStrings["BDColegioSql"].ConnectionString;
            return cns;
        }

        public const string CNS = "BDColegioSql";

        //nuevos metodos para enviar mailing
        //public static System.Net.Mail.MailMessage ConstruyeMensajeProyecto(int instId, string nombreInstitucion, string nombreProyecto, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Proyecto", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreProyecto).Replace("{NombreInstitucion}", nombreInstitucion);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaProyecto == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Proyecto en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaProyecto == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Proyecto en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaProyecto == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Proyecto en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeTricel(int instId, string nombreInstitucion, string nombreTricel, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Tricel", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreTricel).Replace("{NombreInstitucion}", nombreInstitucion);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaTricel == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Tricel en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaTricel == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Tricel en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaTricel == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Tricel en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeDocumento(int instId, string nombreInstitucion, string nombreDocumento, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Documento", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreDocumento).Replace("{NombreInstitucion}", nombreInstitucion);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaDocumento == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Documento en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaDocumento == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Documento en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeRendicion(int instId, string nombreInstitucion, string tipoMovimiento, string monto, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail
        //        double montoD = Convert.ToDouble(monto);
        //        string montoMostrar = montoD.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("es-CL"));

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Rendicion", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", montoMostrar).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{TipoMovimiento}", tipoMovimiento);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaRendicion == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Rendición en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaRendicion == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Rendicion en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaRendicion == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Rendicion en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeEvento(int instId, string nombreInstitucion, string nombreTricel, string ubicacion, string fechaInicioTermino, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Evento", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreTricel).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Ubicacion}", ubicacion).Replace("{FechaInicioTermino}", fechaInicioTermino);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaCalendario == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Evento en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaCalendario == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Evento en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaCalendario == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Evento en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeUsuario(int instId, string nombreInstitucion, string nombreUsuario, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Usuario", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreUsuario).Replace("{NombreInstitucion}", nombreInstitucion);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaUsuario == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Usuario en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaUsuario == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Usuario en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaUsuario == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Usuario en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeMuro(int instId, string nombreInstitucion, string novedad, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Muro", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", novedad).Replace("{NombreInstitucion}", nombreInstitucion);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaMuro == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Novedad en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaMuro == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Novedad en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaMuro == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Novedad en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeRol(int instId, string nombreInstitucion, string rol, List<string> correos, bool esNuevo, bool esModificado, bool esEliminado)
        //{

        //    List<VCFramework.Entidad.Mailing> mailing = VCFramework.NegocioMySql.Mailing.ObtenerMailingPorInstId(instId);
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();

        //    if (mailing != null && mailing.Count == 1)
        //    {
        //        VCFramework.Entidad.Mailing mail = mailing[0];
        //        //variables del mail

        //        sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //        sms.IsBodyHtml = true;
        //        if (correos != null && correos.Count > 0)
        //        {
        //            foreach (string s in correos)
        //            {
        //                sms.To.Add(s);
        //            }
        //        }
        //        string htmlMensaje = ObtenerMensajeXML("Rol", esNuevo, esModificado, esEliminado);

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("<html>");
        //        htmlMensaje = htmlMensaje.Replace("{NombreItem}", rol).Replace("{NombreInstitucion}", nombreInstitucion);
        //        sb.Append(htmlMensaje);
        //        sb.Append("</html>");
        //        sms.Body = sb.ToString();

        //        //verificamos si puede o no puede realizar la operación
        //        if (esNuevo && mail.CreaRol == 1)
        //        {
        //            //puede crear
        //            sms.Subject = "Creación de Rol en asambleas.cl";

        //        }
        //        else if (esModificado && mail.ModificaRol == 1)
        //        {
        //            //puede modificar
        //            sms.Subject = "Modificación de Rol en asambleas.cl";

        //        }
        //        else if (esEliminado && mail.EliminaRol == 1)
        //        {
        //            //puede eliminar
        //            sms.Subject = "Eliminación de Rol en asambleas.cl";

        //        }
        //        else
        //        {
        //            sms = null;
        //        }
        //        if (htmlMensaje == null || htmlMensaje.Length == 0)
        //            sms = null;
        //    }
        //    else
        //    {
        //        sms = null;
        //    }

        //    return sms;

        //}

        public static string NombreBaseDatos()
        {
            string retorno = "'db_9dac90_cole'";

            if (System.Configuration.ConfigurationManager.AppSettings["NOMBRE_BD"] != null)
            {
                retorno = "'" + System.Configuration.ConfigurationManager.AppSettings["NOMBRE_BD"].ToString() + "'";
            }

            return retorno;
        }
        public static string SMTP()
        {
            string retorno = "smtp.gmail.com";
            if (System.Configuration.ConfigurationManager.AppSettings["SMTP"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["SMTP"].ToString();
            }

            return retorno;
        }
        public static string PUERTO_SMTP()
        {
            string retorno = "587";
            if (System.Configuration.ConfigurationManager.AppSettings["PUERTO_SMTP"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["PUERTO_SMTP"].ToString();
            }

            return retorno;
        }
        public static string NOMBRE_SERVIDOR_CORREO()
        {
            string retorno = "vcoronado.alarcon@gmail.com";
            if (System.Configuration.ConfigurationManager.AppSettings["NOMBRE_SERVIDOR_CORREO"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["NOMBRE_SERVIDOR_CORREO"].ToString();
            }

            return retorno;
        }
        public static string CLAVE_SERVIDOR_CORREO()
        {
            string retorno = "antonia2005";
            if (System.Configuration.ConfigurationManager.AppSettings["CLAVE_SERVIDOR_CORREO"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["CLAVE_SERVIDOR_CORREO"].ToString();
            }

            return retorno;
        }
        public static string HABILITA_COPIA_ADMIN1()
        {
            string retorno = "0";
            if (System.Configuration.ConfigurationManager.AppSettings["HABILITA_COPIA_ADMIN1"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["HABILITA_COPIA_ADMIN1"].ToString();
            }

            return retorno;
        }
        //public static string ENVIA_CORREO_EVENTO(int instId)
        //{
        //    string retorno = "0";
        //    //primero validamos la base de datos 
        //    Entidad.ConfiguracionInstitucion config = NegocioMySQL.ConfiguracionInstitucion.ObtenerConfiguracionPorInstId(instId);
        //    if (config != null)
        //    {
        //        retorno = config.EnviaDocumentos.ToString();
        //    }
        //    else
        //    {
        //        if (System.Configuration.ConfigurationManager.AppSettings["ENVIA_CORREO_EVENTO"] != null)
        //        {
        //            retorno = System.Configuration.ConfigurationManager.AppSettings["ENVIA_CORREO_EVENTO"].ToString();
        //        }
        //    }
        //    return retorno;
        //}
        public static string HABILITA_COPIA_ADMIN2()
        {
            string retorno = "0";
            if (System.Configuration.ConfigurationManager.AppSettings["HABILITA_COPIA_ADMIN2"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["HABILITA_COPIA_ADMIN2"].ToString();
            }

            return retorno;
        }
        public static string COPIA_ADMIN_1()
        {
            string retorno = "turk182@gmail.com";
            if (System.Configuration.ConfigurationManager.AppSettings["COPIA_ADMIN_1"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["COPIA_ADMIN_1"].ToString();
            }

            return retorno;
        }
        public static string COPIA_ADMIN_2()
        {
            string retorno = "vcoronado.alarcon@gmail.com";
            if (System.Configuration.ConfigurationManager.AppSettings["COPIA_ADMIN_2"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["COPIA_ADMIN_2"].ToString();
            }

            return retorno;
        }
        public static string ENABLE_SSL()
        {
            string retorno = "0";
            if (System.Configuration.ConfigurationManager.AppSettings["ENABLE_SSL"] != null)
            {
                retorno = System.Configuration.ConfigurationManager.AppSettings["ENABLE_SSL"].ToString();
            }

            return retorno;
        }
        //public static System.Net.Mail.MailMessage ConstruyeMensajeContacto(string nombre, string telefono, string email, string motivo)
        //{

        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
        //    sms.Subject = "Formulario Contacto CPAS";
        //    sms.To.Add(Utiles.NOMBRE_SERVIDOR_CORREO());
        //    string habilitaCopiaAdmin1 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin1 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_1());
        //    }
        //    string habilitaCopiaAdmin2 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin2 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_2());
        //    }
        //    sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //    sms.IsBodyHtml = true;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html>");

        //    string htmlMensaje = ObtenerMensajeXML("Contacto", true);
        //    if (htmlMensaje != null)
        //    {
        //        htmlMensaje = htmlMensaje.Replace("{Nombre}", nombre).Replace("{Telefono}", telefono).Replace("{Email}", email).Replace("{Motivo}", motivo);
        //        sb.Append(htmlMensaje);
        //    }
        //    else
        //    {
        //        sb.AppendFormat("{0}, Teléfono Contacto {1}, correo electónico {2}:<br />", nombre, telefono, email);
        //        sb.AppendFormat("Desea contactarse mediante el formulario de contacto, donde ha ingresado el siguiente motivo: {0}<br />", motivo);
        //        sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ******");

        //    }
        //    sb.Append("</html>");
        //    sms.Body = sb.ToString();
        //    return sms;
        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensaje(string email, string contenido)
        //{

        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
        //    sms.Subject = "Creación automática Usuario";
        //    sms.To.Add(email);

        //    sms.From = new System.Net.Mail.MailAddress("contacto@asambleas.cl", "asambleas");
        //    sms.IsBodyHtml = true;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html>");
        //    sb.Append(contenido);

        //    sb.Append("</html>");
        //    sms.Body = sb.ToString();
        //    return sms;
        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeCrearProyecto(string nombreInstitucion, string nombreProyecto, List<string> correos, bool esNuevo)
        //{
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
        //    sms.Subject = "Creación de Proyecto en asambleas.cl";
        //    string habilitaCopiaAdmin1 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin1 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_1());
        //    }
        //    string habilitaCopiaAdmin2 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin2 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_2());
        //    }
        //    //recorremos la lista de usuarios de la institución
        //    if (correos != null && correos.Count > 0)
        //    {
        //        foreach (string s in correos)
        //        {
        //            sms.To.Add(s);
        //        }
        //    }


        //    sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //    sms.IsBodyHtml = true;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html>");
        //    if (esNuevo)
        //    {
        //        string htmlMensaje = ObtenerMensajeXML("Proyecto", true);
        //        if (htmlMensaje != null)
        //        {
        //            htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreProyecto).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
        //            sb.Append(htmlMensaje);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("Se ha creado el Proyecto <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreProyecto, nombreInstitucion);
        //            sb.Append("Estimado Usuario, ha sido creado un Proyecto para su establecimiento, ingrese a www.asambleas.cl para poder verlo.");
        //            sb.Append("<br />");
        //            sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de asambleas.cl ****** <br />");
        //        }
        //    }
        //    else
        //    {
        //        string htmlMensaje = ObtenerMensajeXML("Proyecto", false);
        //        if (htmlMensaje != null)
        //        {
        //            htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreProyecto).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
        //            sb.Append(htmlMensaje);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("Ha sido modificado el Proyecto <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreProyecto, nombreInstitucion);
        //            sb.Append("Estimado Usuario, ha sido Modificado un Proyecto para su establecimiento, ingrese a www.asambleas.cl para poder verlo.");
        //            sb.Append("<br />");
        //            sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de asambleas ****** <br />");
        //        }
        //    }

        //    sb.Append("</html>");
        //    sms.Body = sb.ToString();
        //    return sms;
        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeCrearTricel(string nombreInstitucion, string nombreProyecto, List<string> correos, bool esNuevo)
        //{
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
        //    sms.Subject = "Creación de Tricel en asambleas.cl";
        //    string habilitaCopiaAdmin1 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin1 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_1());
        //    }
        //    string habilitaCopiaAdmin2 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin2 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_2());
        //    }
        //    //recorremos la lista de usuarios de la institución
        //    if (correos != null && correos.Count > 0)
        //    {
        //        foreach (string s in correos)
        //        {
        //            sms.To.Add(s);
        //        }
        //    }


        //    sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "asambleas");
        //    sms.IsBodyHtml = true;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html>");
        //    if (esNuevo)
        //    {
        //        string htmlMensaje = ObtenerMensajeXML("Tricel", true);
        //        if (htmlMensaje != null)
        //        {
        //            htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreProyecto).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
        //            sb.Append(htmlMensaje);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("Se ha creado el Proyecto <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreProyecto, nombreInstitucion);
        //            sb.Append("Estimado Usuario, ha sido creado un Tricel para su establecimiento, ingrese a www.asambleas.cl para poder verlo.");
        //            sb.Append("<br />");
        //            sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de asambleas.cl ****** <br />");
        //        }
        //    }
        //    else
        //    {
        //        string htmlMensaje = ObtenerMensajeXML("Tricel", false);
        //        if (htmlMensaje != null)
        //        {
        //            htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreProyecto).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
        //            sb.Append(htmlMensaje);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("Ha sido modificado el Tricel <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreProyecto, nombreInstitucion);
        //            sb.Append("Estimado Usuario, ha sido Modificado un Tricel para su establecimiento, ingrese a www.asambleas.cl para poder verlo.");
        //            sb.Append("<br />");
        //            sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de asambleas ****** <br />");
        //        }
        //    }

        //    sb.Append("</html>");
        //    sms.Body = sb.ToString();
        //    return sms;
        //}

        //public static System.Net.Mail.MailMessage ConstruyeMensajeAgregarDocumento(string nombreInstitucion, string nombreDocumento, List<string> correos, bool esNuevo)
        //{
        //    System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
        //    sms.Subject = "Documento en CPAS";
        //    string habilitaCopiaAdmin1 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin1 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_1());
        //    }
        //    string habilitaCopiaAdmin2 = Utiles.HABILITA_COPIA_ADMIN1();
        //    if (habilitaCopiaAdmin2 == "1")
        //    {
        //        sms.To.Add(COPIA_ADMIN_2());
        //    }
        //    //recorremos la lista de usuarios de la institución
        //    if (correos != null && correos.Count > 0)
        //    {
        //        foreach (string s in correos)
        //        {
        //            sms.To.Add(s);
        //        }
        //    }


        //    sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "CPAS");
        //    sms.IsBodyHtml = true;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<html>");
        //    if (esNuevo)
        //    {
        //        string htmlMensaje = ObtenerMensajeXML("Documento", true);
        //        if (htmlMensaje != null)
        //        {
        //            htmlMensaje = htmlMensaje.Replace("{NombreItem}", EntregaNombreArchivo(nombreDocumento)).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
        //            sb.Append(htmlMensaje);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("Se ha subido el documento <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreDocumento, nombreInstitucion);
        //            sb.Append("Estimado Usuario CPAS, ha sido subido un documento para su establecimiento, ingrese a www.cpas.cl para poder verlo.");
        //        }
        //    }
        //    else
        //    {
        //        string htmlMensaje = ObtenerMensajeXML("Documento", false);
        //        if (htmlMensaje != null)
        //        {
        //            htmlMensaje = htmlMensaje.Replace("{NombreItem}", EntregaNombreArchivo(nombreDocumento)).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
        //            sb.Append(htmlMensaje);
        //        }
        //        else
        //        {
        //            sb.AppendFormat("Ha sido modificado el documento <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreDocumento, nombreInstitucion);
        //            sb.Append("Estimado Usuario CPAS, ha sido Modificado un documento para su establecimiento, ingrese a www.cpas.cl para poder verlo.");
        //            sb.Append("<br />");
        //            sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ****** <br />");
        //        }
        //    }

        //    sb.Append("</html>");
        //    sms.Body = sb.ToString();
        //    return sms;
        //}

        public static System.Net.Mail.MailMessage ConstruyeMensajeCrearRendicion(string nombreInstitucion, string nombreDocumento, List<string> correos, bool esNuevo)
        {
            System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
            sms.Subject = "Rendición en CPAS";
            string habilitaCopiaAdmin1 = Utiles.HABILITA_COPIA_ADMIN1();
            if (habilitaCopiaAdmin1 == "1")
            {
                sms.To.Add(COPIA_ADMIN_1());
            }
            string habilitaCopiaAdmin2 = Utiles.HABILITA_COPIA_ADMIN1();
            if (habilitaCopiaAdmin2 == "1")
            {
                sms.To.Add(COPIA_ADMIN_2());
            }
            //recorremos la lista de usuarios de la institución
            if (correos != null && correos.Count > 0)
            {
                foreach (string s in correos)
                {
                    sms.To.Add(s);
                }
            }


            sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "CPAS");
            sms.IsBodyHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            if (esNuevo)
            {
                string htmlMensaje = ObtenerMensajeXML("Rendicion", true);
                if (htmlMensaje != null)
                {
                    htmlMensaje = htmlMensaje.Replace("{NombreItem}", EntregaNombreArchivo(nombreDocumento)).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
                    sb.Append(htmlMensaje);
                }
                else
                {
                    sb.AppendFormat("Se ha agregado una rendición  <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreDocumento, nombreInstitucion);
                    sb.Append("Estimado Usuario CPAS, ha sido agrgada una rendición para su establecimiento, ingrese a www.cpas.cl para poder verlo.");
                    sb.Append("<br />");
                    sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ****** <br />");
                }
            }
            else
            {
                string htmlMensaje = ObtenerMensajeXML("Rendicion", false);
                if (htmlMensaje != null)
                {
                    htmlMensaje = htmlMensaje.Replace("{NombreItem}", EntregaNombreArchivo(nombreDocumento)).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{Url}", ObtenerUrl());
                    sb.Append(htmlMensaje);
                }
                else
                {
                    sb.AppendFormat("Ha sido modificado una rendición <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreDocumento, nombreInstitucion);
                    sb.Append("Estimado Usuario CPAS, ha sido Modificada una rendición para su establecimiento, ingrese a www.cpas.cl para poder verlo.");
                    sb.Append("<br />");
                    sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ****** <br />");
                }
            }

            sb.Append("</html>");
            sms.Body = sb.ToString();
            return sms;
        }
        public static System.Net.Mail.MailMessage ConstruyeMensajeRecuperarClave(string nombre, string clave, string email)
        {

            System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
            sms.Subject = "Recuperación Clave asambleas";
            sms.To.Add(email);

            sms.From = new System.Net.Mail.MailAddress("contacto@asambleas.cl", "Asambleas");
            sms.IsBodyHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");

            string htmlMensaje = ObtenerMensajeXML("RecuperarClave", true);
            if (htmlMensaje != null)
            {
                htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombre).Replace("{Clave}", clave).Replace("{Url}", ObtenerUrl());
                sb.Append(htmlMensaje);
            }
            else
            {
                sb.AppendFormat("Estimado Usuario {0}:<br />", nombre);
                sb.AppendFormat("Su password fué recuperada con exito: {0}<br />", clave);
                sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de asambleas ******");
            }

            sb.Append("</html>");
            sms.Body = sb.ToString();
            return sms;
        }
        public static System.Net.Mail.MailMessage ConstruyeMensajeCambiarClave(string nombre, string clave, string email)
        {

            System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
            sms.Subject = "Cambio Clave Asambleas";
            sms.To.Add(email);

            sms.From = new System.Net.Mail.MailAddress("contacto@asambleas.cl", "Asambleas");
            sms.IsBodyHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");


            string htmlMensaje = ObtenerMensajeXML("RecuperarClave", false);
            if (htmlMensaje != null)
            {
                htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombre).Replace("{Clave}", clave).Replace("{Url}", ObtenerUrl());
                sb.Append(htmlMensaje);
            }
            else
            {
                sb.AppendFormat("Estimado Usuario {0}:<br />", nombre);
                sb.AppendFormat("Su password fué cambiada con exito: {0}<br />", clave);
                sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de Asambleas ******");
            }



            sb.Append("</html>");
            sms.Body = sb.ToString();
            return sms;
        }
        public static System.Net.Mail.MailMessage ConstruyeMensajeCreacionMasiva(string nombre, string clave, string email)
        {

            System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
            sms.Subject = "Creación Usuario CPAS";
            sms.To.Add(email);

            sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "CPAS");
            sms.IsBodyHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            string htmlMensaje = ObtenerMensajeXML("CrearUsuario", true);
            if (htmlMensaje != null)
            {
                htmlMensaje = htmlMensaje.Replace("{NombreUsuario}", nombre).Replace("{Clave}", clave).Replace("{Url}", ObtenerUrl());
                sb.Append(htmlMensaje);
            }
            else
            {

                sb.AppendFormat("Estimado Usuario {0} se ha creado una cuenta de acceso a CPAS,<br />", nombre);
                sb.AppendFormat("Su nueva password fué es: {0}<br />", clave);
                sb.Append("***** Puede acceder al Sistema y cambiarla cuando lo estime conveniente ******");
                sb.Append("<br />");
                sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ******");
            }

            sb.Append("</html>");
            sms.Body = sb.ToString();
            return sms;
        }

        //public static string ENVIA_DOCUMENTOS(int instId)
        //{
        //    string retorno = "0";
        //    //primero validamos la base de datos 
        //    Entidad.ConfiguracionInstitucion config = NegocioMySQL.ConfiguracionInstitucion.ObtenerConfiguracionPorInstId(instId);
        //    if (config != null)
        //    {
        //        retorno = config.EnviaDocumentos.ToString();
        //    }
        //    else
        //    {
        //        if (System.Configuration.ConfigurationManager.AppSettings["ENVIA_DOCUMENTOS"] != null)
        //        {
        //            retorno = System.Configuration.ConfigurationManager.AppSettings["ENVIA_DOCUMENTOS"].ToString();
        //        }
        //    }

        //    return retorno;
        //}
        //public static string ENVIA_PROYECTOS(int instId)
        //{
        //    string retorno = "0";
        //    //primero validamos la base de datos 
        //    Entidad.ConfiguracionInstitucion config = NegocioMySQL.ConfiguracionInstitucion.ObtenerConfiguracionPorInstId(instId);
        //    if (config != null)
        //    {
        //        retorno = config.EnviaProyectos.ToString();
        //    }
        //    else
        //    {
        //        if (System.Configuration.ConfigurationManager.AppSettings["ENVIA_PROYECTOS"] != null)
        //        {
        //            retorno = System.Configuration.ConfigurationManager.AppSettings["ENVIA_PROYECTOS"].ToString();
        //        }
        //    }

        //    return retorno;
        //}
        //public static string ENVIA_RENDICIONES(int instId)
        //{
        //    string retorno = "0";
        //    //primero validamos la base de datos 
        //    Entidad.ConfiguracionInstitucion config = NegocioMySQL.ConfiguracionInstitucion.ObtenerConfiguracionPorInstId(instId);
        //    if (config != null)
        //    {
        //        retorno = config.EnviaRendiciones.ToString();
        //    }
        //    else
        //    {
        //        if (System.Configuration.ConfigurationManager.AppSettings["ENVIA_RENDICIONES"] != null)
        //        {
        //            retorno = System.Configuration.ConfigurationManager.AppSettings["ENVIA_RENDICIONES"].ToString();
        //        }
        //    }

        //    return retorno;
        //}
        //public static string ENVIA_CORREO_CREACION_MASIVA()
        //{
        //    string retorno = "0";
        //    if (System.Configuration.ConfigurationManager.AppSettings["ENVIA_CORREO_CREACION_MASIVA"] != null)
        //    {
        //        retorno = System.Configuration.ConfigurationManager.AppSettings["ENVIA_CORREO_CREACION_MASIVA"].ToString();
        //    }

        //    return retorno;
        //}
        public static string RetornaFechaFormateadaServidor(string fechaServidor)
        {
            string retorno = "";
            //lo primero es descomponer la fecha
            string[] fechas = fechaServidor.Split('/');
            if (fechas != null && fechas.Length == 3)
            {
                string dia = "";
                if (fechas[1].Length == 1)
                {
                    dia = "0" + fechas[1].ToString();
                }
                else
                {
                    dia = fechas[1].ToString();
                }
                string mes = "";
                if (fechas[0].Length == 1)
                {
                    mes = "0" + fechas[0].ToString();
                }
                else
                {
                    dia = fechas[0].ToString();
                }
                string anno = fechas[2].ToString();

                retorno = dia + "-" + mes + "-" + anno;
            }
            else
                retorno = DateTime.Now.ToShortDateString();


            return retorno;
        }

        public static string RetornaFechaEntera()
        {
            DateTime fechaServidor = DateTime.Now;
            string retorno = "";
            string anno = fechaServidor.Year.ToString();
            string mes = "";
            string dia = "";
            if (fechaServidor.Month < 10)
                mes = "0" + fechaServidor.Month.ToString();
            else
                mes = fechaServidor.Month.ToString();

            if (fechaServidor.Day < 10)
                dia = "0" + fechaServidor.Day.ToString();
            else
                dia = fechaServidor.Day.ToString();

            retorno = anno + mes + dia;

            return retorno;
        }
        public static string RetornaHoraEntera()
        {
            DateTime fechaServidor = DateTime.Now;
            string retorno = "";
            string hora = "";
            string minutos = "";
            if (fechaServidor.Hour < 10)
                hora = "0" + fechaServidor.Hour.ToString();
            else
                hora = fechaServidor.Hour.ToString();

            if (fechaServidor.Minute < 10)
                minutos = "0" + fechaServidor.Minute.ToString();
            else
                minutos = fechaServidor.Minute.ToString();

            retorno = hora + minutos;

            return retorno;
        }
        public static int RetornaFechaEntera(DateTime fechaProcesar)
        {
            DateTime fechaServidor = fechaProcesar;
            int retorno = 0;
            string anno = fechaServidor.Year.ToString();
            string mes = "";
            string dia = "";
            if (fechaServidor.Month < 10)
                mes = "0" + fechaServidor.Month.ToString();
            else
                mes = fechaServidor.Month.ToString();

            if (fechaServidor.Day < 10)
                dia = "0" + fechaServidor.Day.ToString();
            else
                dia = fechaServidor.Day.ToString();

            retorno = int.Parse(anno + mes + dia);

            return retorno;
        }
        public static string RetornaFechaDocumento(string fechaProcesar)
        {
            //12/03/2017 10:07 p.m.
            //5/17/2017 7:06 PM

            string retorno = RetornaFechaFormateadaServidor("09/09/2017");
            try
            {
                //primera separacion
                string[] parte1 = fechaProcesar.Split(' ');
                if (parte1.Length > 1)
                {
                    //segunda separación
                    string[] parte2 = parte1[0].Split('/');
                    if (parte2.Length == 3)
                    {
                        if (parte2[1].Length == 1)
                            parte2[1] = "0" + parte2[1];
                        if (parte2[0].Length == 1)
                            parte2[0] = "0" + parte2[0];

                        retorno = parte2[1] + "-" + parte2[0] + "-" + parte2[2];

                    }
                }

            }
            catch (Exception ex)
            {
                Log(ex);
            }


            return retorno;
        }

        /// <summary>
        /// Entrega Fecha entera a partir del formato 21-01-2017
        /// </summary>
        /// <param name="fechaProcesar">Fecha string</param>
        /// <returns></returns>
        public static int RetornaFechaEnteraStr(string fechaProcesar)
        {
            string[] fechitas = fechaProcesar.Split('-');
            int retorno = 0;
            try
            {
                if (fechitas != null && fechitas.Length > 2)
                {
                    string anio = fechitas[2].Split(' ')[0];
                    retorno = int.Parse(anio + fechitas[1] + fechitas[0]);
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            return retorno;
        }

        public static string ConstruyeFecha(DateTime fecha)
        {
            string retorno = "";
            string dia = "";
            string mes = "";
            string anno = "";
            string hora = "";
            string minutos = "";
            string segundos = "";


            if (fecha.Day < 10)
                dia = "0" + fecha.Day.ToString();
            else
                dia = fecha.Day.ToString();

            if (fecha.Month < 10)
                mes = "0" + fecha.Month.ToString();
            else
                mes = fecha.Month.ToString();

            if (fecha.Hour < 10)
                hora = "0" + fecha.Hour.ToString();
            else
                hora = fecha.Hour.ToString();

            if (fecha.Minute < 10)
                minutos = "0" + fecha.Minute.ToString();
            else
                minutos = fecha.Minute.ToString();

            if (fecha.Second < 10)
                segundos = "0" + fecha.Second.ToString();
            else
                segundos = fecha.Second.ToString();

            anno = fecha.Year.ToString();

            retorno = anno + "-" + mes + "-" + dia + " " + hora + ":" + segundos;
            return retorno;
        }
        public static System.Net.Mail.MailMessage ConstruyeMensajeCrearEvento(string nombreInstitucion, string nombreEvento, string fechaInicioYTermino, string ubicacion, List<string> correos, bool esNuevo)
        {
            System.Net.Mail.MailMessage sms = new System.Net.Mail.MailMessage();
            sms.Subject = "Evento en CPAS";
            string habilitaCopiaAdmin1 = Utiles.HABILITA_COPIA_ADMIN1();
            if (habilitaCopiaAdmin1 == "1")
            {
                sms.To.Add(COPIA_ADMIN_1());
            }
            string habilitaCopiaAdmin2 = Utiles.HABILITA_COPIA_ADMIN1();
            if (habilitaCopiaAdmin2 == "1")
            {
                sms.To.Add(COPIA_ADMIN_2());
            }
            //recorremos la lista de usuarios de la institución
            if (correos != null && correos.Count > 0)
            {
                foreach (string s in correos)
                {
                    sms.To.Add(s);
                }
            }


            sms.From = new System.Net.Mail.MailAddress("contacto@cpas.cl", "CPAS");
            sms.IsBodyHtml = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            if (esNuevo)
            {
                string htmlMensaje = ObtenerMensajeXML("Evento", true);
                if (htmlMensaje != null)
                {
                    htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreEvento).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{FechaInicioTermino}", fechaInicioYTermino).Replace("{Url}", ObtenerUrl());
                    sb.Append(htmlMensaje);
                }
                else
                {
                    sb.AppendFormat("Se ha agregado el evento  <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreEvento, nombreInstitucion);
                    sb.AppendFormat("Estimado Usuario CPAS, ha sido agregado un evento para su establecimiento, este tendrá lugar en {0} entre {1}, ingrese a www.cpas.cl para poder verlo.", ubicacion, fechaInicioYTermino);
                    sb.Append("<br />");
                    sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ****** <br />");
                    sb.Append("</html>");
                }
            }
            else
            {
                string htmlMensaje = ObtenerMensajeXML("Evento", false);
                if (htmlMensaje != null)
                {
                    htmlMensaje = htmlMensaje.Replace("{NombreItem}", nombreEvento).Replace("{NombreInstitucion}", nombreInstitucion).Replace("{FechaInicioTermino}", fechaInicioYTermino).Replace("{Url}", ObtenerUrl());
                    sb.Append(htmlMensaje);
                }
                else
                {
                    sb.AppendFormat("Ha sido modificado el evento  <strong>{0}</strong>, para el Establecimiento {1}:<br />", nombreEvento, nombreInstitucion);
                    sb.AppendFormat("Estimado Usuario CPAS, ha sido agregado un evento para su establecimiento, este tendrá lugar en {0} entre {1}, ingrese a www.cpas.cl para poder verlo.", ubicacion, fechaInicioYTermino);
                    sb.Append("<br />");
                    sb.Append("***** Mensaje enviado desde el sistema automático de envio de correos de CPAS ****** <br />");
                    sb.Append("</html>");
                }
            }

            sms.Body = sb.ToString();
            return sms;
        }
        public static void Log(string mensaje)
        {
            string carpetaArchivo = @"Logs\log.txt";
            string rutaFinal = AppDomain.CurrentDomain.BaseDirectory + carpetaArchivo;

            object Locker = new object();
            XmlDocument _doc = new XmlDocument();

            try
            {
                if (!File.Exists(rutaFinal))
                {
                    File.Create(rutaFinal);
                }

                _doc.Load(rutaFinal);

                lock (Locker)
                {
                    //var id = (XmlElement)_doc.DocumentElement.LastChild;
                    //id.GetElementsByTagName("Id");
                    int cantidad = _doc.ChildNodes.Count;
                    int indice = 1;
                    if (cantidad > 0)
                    {
                        //obtener el ultimo elemento id
                        if ((XmlElement)_doc.DocumentElement.LastChild != null)
                        {
                            var ultimo = (XmlElement)_doc.DocumentElement.LastChild;
                            indice = int.Parse(ultimo.LastChild.InnerText);
                            indice = indice + 1;
                        }
                    }

                    var el = (XmlElement)_doc.DocumentElement.AppendChild(_doc.CreateElement("error"));
                    //el.SetAttribute("Fecha", ConstruyeFecha(DateTime.Now));

                    el.AppendChild(_doc.CreateElement("Fecha")).InnerText = ConstruyeFecha(DateTime.Now);
                    el.AppendChild(_doc.CreateElement("Detalle")).InnerText = mensaje;
                    el.AppendChild(_doc.CreateElement("Id")).InnerText = indice.ToString();
                    _doc.Save(rutaFinal);
                    VCFramework.Entidad.LogsSistema log = new Entidad.LogsSistema();
                    log.FechaRegistro = DateTime.Now;
                    log.Mensaje = mensaje;
                    log.TipoMensaje = 1; //corresponde a texto no a error
                    VCFramework.NegocioMySql.LogsSistema.Insertar(log);
                }

            }
            catch (Exception ex)
            {

            }

        }
        public static string EntregaNombreArchivo(string nombreArchivo)
        {
            StringBuilder sb = new StringBuilder();
            string[] texto = nombreArchivo.ToString().Split(' ');
            string nuevoNombre = string.Empty;
            if (texto.Length > 0)
            {
                for (int i = 0; i < texto.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(texto[i]);
                        sb.Append(" ");
                    }
                }
            }
            return sb.ToString();
        }
        public static string ObtenerMensajeXML(string nombre, bool esNuevo, bool esModificado, bool esEliminado)
        {
            string retorno = "";
            string carpetaArchivo = @"Mensajes.xml";
            string rutaFinal = AppDomain.CurrentDomain.BaseDirectory + carpetaArchivo;
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaFinal);

            try
            {
                XmlNodeList mensaje = doc.GetElementsByTagName("Mensaje");
                XmlNodeList lista = ((XmlElement)mensaje[0]).GetElementsByTagName("item");
                if (lista != null && lista.Count > 0)
                {
                    foreach (XmlElement nodo in lista)
                    {
                        if (nodo != null)
                        {
                            if (nodo.Attributes[0] != null)
                            {
                                if (nodo.Attributes[0].InnerText.ToString().ToUpper() == nombre.ToUpper())
                                {
                                    string otraBusqueda = "nuevo";
                                    if (esNuevo)
                                        otraBusqueda = "nuevo";
                                    if (esModificado)
                                        otraBusqueda = "modificado";
                                    if (esEliminado)
                                        otraBusqueda = "eliminado";

                                    if (nodo.ChildNodes != null && nodo.ChildNodes.Count > 0)
                                    {
                                        foreach (XmlElement nodito in nodo.ChildNodes)
                                        {
                                            if (nodito.Name.ToUpper() == otraBusqueda.ToUpper())
                                            {
                                                retorno = nodito.InnerXml;
                                                break;
                                            }
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
                Log(ex);
            }

            return retorno;
        }
        public static string ObtenerMensajeXML(string nombre, bool esNuevo)
        {
            string retorno = "";
            string carpetaArchivo = @"Mensajes.xml";
            string rutaFinal = AppDomain.CurrentDomain.BaseDirectory + carpetaArchivo;
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaFinal);

            try
            {
                XmlNodeList mensaje = doc.GetElementsByTagName("Mensaje");
                XmlNodeList lista = ((XmlElement)mensaje[0]).GetElementsByTagName("item");
                if (lista != null && lista.Count > 0)
                {
                    foreach (XmlElement nodo in lista)
                    {
                        if (nodo != null)
                        {
                            if (nodo.Attributes[0] != null)
                            {
                                if (nodo.Attributes[0].InnerText.ToString().ToUpper() == nombre.ToUpper())
                                {
                                    string otraBusqueda = "nuevo";
                                    if (!esNuevo)
                                        otraBusqueda = "modificado";

                                    if (nodo.ChildNodes != null && nodo.ChildNodes.Count > 0)
                                    {
                                        foreach (XmlElement nodito in nodo.ChildNodes)
                                        {
                                            if (nodito.Name.ToUpper() == otraBusqueda.ToUpper())
                                            {
                                                retorno = nodito.InnerXml;
                                                break;
                                            }
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
                Log(ex);
            }

            return retorno;
        }

        public static string ObtenerUrl()
        {
            string retorno = "http://www.cpas.cl";
            try
            {
                retorno = System.Web.HttpContext.Current.Request.Url.Host;
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            return retorno;
        }
        public static void Log(Exception mensaje)
        {
            string carpetaArchivo = @"Logs\log.txt";
            string rutaFinal = AppDomain.CurrentDomain.BaseDirectory + carpetaArchivo;

            object Locker = new object();
            XmlDocument _doc = new XmlDocument();

            try
            {
                if (!File.Exists(rutaFinal))
                {
                    File.Create(rutaFinal);
                }

                _doc.Load(rutaFinal);

                lock (Locker)
                {
                    //var id = (XmlElement)_doc.DocumentElement.LastChild;
                    //id.GetElementsByTagName("Id");
                    int cantidad = _doc.ChildNodes.Count;
                    int indice = 1;
                    if (cantidad > 0)
                    {
                        //obtener el ultimo elemento id
                        if ((XmlElement)_doc.DocumentElement.LastChild != null)
                        {
                            var ultimo = (XmlElement)_doc.DocumentElement.LastChild;
                            indice = int.Parse(ultimo.LastChild.InnerText);
                            indice = indice + 1;
                        }
                    }

                    var el = (XmlElement)_doc.DocumentElement.AppendChild(_doc.CreateElement("error"));
                    //el.SetAttribute("Fecha", ConstruyeFecha(DateTime.Now));

                    el.AppendChild(_doc.CreateElement("Fecha")).InnerText = ConstruyeFecha(DateTime.Now);
                    el.AppendChild(_doc.CreateElement("Detalle")).InnerText = mensaje.Message;
                    el.AppendChild(_doc.CreateElement("Id")).InnerText = indice.ToString();
                    _doc.Save(rutaFinal);
                    VCFramework.Entidad.LogsSistema log = new Entidad.LogsSistema();
                    log.FechaRegistro = DateTime.Now;
                    log.Mensaje = mensaje.InnerException.Message;
                    log.TipoMensaje = 2; //corresponde a error
                    VCFramework.NegocioMySql.LogsSistema.Insertar(log);
                }

            }
            catch (Exception ex)
            {

            }


        }
        /// <summary>
        /// Validar Rut en el formato 12.333.66-K
        /// </summary>
        /// <param name="rut">Rut Formateado</param>
        /// <returns></returns>
        public static bool validarRut(string rut)
        {

            bool validacion = false;
            try
            {
                rut = rut.ToUpper();
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));

                char dv = char.Parse(rut.Substring(rut.Length - 1, 1));

                int m = 0, s = 1;
                for (; rutAux != 0; rutAux /= 10)
                {
                    s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
                }
                if (dv == (char)(s != 0 ? s + 47 : 75))
                {
                    validacion = true;
                }
            }
            catch (Exception)
            {
            }
            return validacion;
        }
    }
}

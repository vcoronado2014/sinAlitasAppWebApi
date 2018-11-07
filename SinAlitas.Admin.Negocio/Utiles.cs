using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace SinAlitas.Admin.Negocio
{
    public class Utiles
    {
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
        public static void Log(Exception mensaje)
        {
            string carpetaArchivo = @"Logs\log.txt";
            //string rutaFinal = AppDomain.CurrentDomain.BaseDirectory + carpetaArchivo;
            string rutaFinal = HttpContext.Current.Server.MapPath("~/logs/log.txt");

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
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public static void Log(string mensaje)
        {
            string carpetaArchivo = @"Logs\log.txt";
            //string rutaFinal = AppDomain.CurrentDomain.BaseDirectory + carpetaArchivo;
            string rutaFinal = HttpContext.Current.Server.MapPath("~/logs/log.txt");

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
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}

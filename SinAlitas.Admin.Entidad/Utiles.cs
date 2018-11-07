using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
//using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SinAlitas.Admin.Entidad
{
    public class Utiles
    {
        public static string RetornaEstadoCupo(int estado)
        {
            string retorno = "";
            switch(estado)
            {
                case 0:
                    retorno = "Creado";
                    break;
                case 1:
                    retorno = "Disponible";
                    break;
                case 2:
                    retorno = "Agendado";
                    break;
                case 3:
                    retorno = "Terminado";
                    break;
            }
            return retorno;
        }
        public static List<Entidad.Semanas> RetornaSemanas(DateTime fechaInicio)
        {
            List<Entidad.Semanas> devolver = new List<Semanas>();
            //vamos a entregar máximo 4 semanas
            //la fecha de inicio no se agregará 23 horas
            //fechaInicio = fechaInicio.AddHours(23);
            for (int i = 0; i < 8; i++)
            {
                int suma = i + 1;
                //DateTime fechaIniSemnana = fechaInicio;
                DateTime fechaIniSemnana = DateTime.MinValue;
                if (i == 0)
                    fechaIniSemnana = fechaInicio.AddHours(23);
                else
                    fechaIniSemnana = Convert.ToDateTime(fechaInicio.ToShortDateString() + " 06:00");
                DateTime fechaTerSemana = fechaIniSemnana.AddDays(6);
                string semanaTexto = "Semana " + suma.ToString() + " desde el " + fechaIniSemnana.ToShortDateString() + " al " + fechaTerSemana.ToShortDateString();
                Entidad.Semanas sem = new Semanas();
                sem.FechaInicioSemana = fechaIniSemnana;
                sem.FechaTerminoSemana = fechaTerSemana;
                sem.SemanaString = semanaTexto;
                sem.FechaInicioEntera = Utiles.RetornaFechaEntera(sem.FechaInicioSemana);
                devolver.Add(sem);
                if (i == 0)
                    fechaInicio = fechaTerSemana.AddDays(1);
                else
                {

                    fechaInicio = Convert.ToDateTime(fechaTerSemana.ToShortDateString() + " 06:00").AddDays(1);
                }
            }


            return devolver;
        }
        public static List<Meses> RetornaMesesProfesor()
        {
            List<Meses> lista = new List<Meses>();

            DateTime ahora = DateTime.Now;
            int mesInicial = ahora.AddMonths(-1).Month;
            int mesFinal = ahora.AddMonths(1).Month;
            int contador = -1;
            for (int i = mesInicial; i <= mesFinal; i++)
            {
                DateTime fechaMostrar = DateTime.Now.AddMonths(contador);
                //DateTime fecha = Convert.ToDateTime("01-" + RetornaMesDia(i) + "-" + ahora.Year);
                Meses mes = new Meses();
                mes.IdMes = fechaMostrar.Month;
                mes.Nombre = fechaMostrar.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")) + ", " + fechaMostrar.Year.ToString();
                lista.Add(mes);
                contador++;
            }

            return lista;
        }
        public static List<Meses> RetornaMesesAnnoProfesor()
        {
            List<Meses> lista = new List<Meses>();

            DateTime ahora = DateTime.Now;
            DateTime inicio = Convert.ToDateTime("01-" + RetornaMesDia(ahora.Month) +"-"+ ahora.Year.ToString()).AddMonths(-1);
            DateTime termino = Convert.ToDateTime("01-" + RetornaMesDia(ahora.Month) +"-"+ ahora.Year.ToString()).AddMonths(3);

            int contador = -1;
            int difMeses = Math.Abs((termino.Month - inicio.Month) + 12 * (termino.Year - inicio.Year));

            if (difMeses > 0)
            {
                for(int i=-1; i < difMeses; i++)
                {
                    int valor = 0;
                    //if (i == 0)
                    //    valor = i - 1;
                    //else
                        valor = i;

                    DateTime mostrar = Convert.ToDateTime("01-" + RetornaMesDia(ahora.Month) + "-" + ahora.Year.ToString()).AddMonths(valor);
                    Meses mes = new Meses();
                    mes.IdMes = mostrar.Month;
                    mes.Anno = mostrar.Year;
                    mes.Nombre = mostrar.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")) + ", " + mostrar.Year.ToString();
                    lista.Add(mes);

                }
            }



            //for (int i = mesInicial; i <= mesFinal; i++)
            //{
            //    DateTime fechaMostrar = DateTime.Now.AddMonths(contador);
            //    //DateTime fecha = Convert.ToDateTime("01-" + RetornaMesDia(i) + "-" + ahora.Year);
            //    Meses mes = new Meses();
            //    mes.IdMes = fechaMostrar.Month;
            //    mes.Nombre = fechaMostrar.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")) + ", " + fechaMostrar.Year.ToString();
            //    lista.Add(mes);
            //    contador++;
            //}

            return lista;
        }

        static Regex ValidEmailRegex = CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        public static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }

        public static string RetornaMesDia(int mes)
        {
            string retorno = "01";
            if (mes < 10)
                retorno = "0" + mes.ToString();
            else
                retorno = mes.ToString();

            return retorno;
        }
        public static DateTime RetornaFecha(int fechaEntera)
        {
            DateTime retorno = DateTime.MinValue;

            string year = fechaEntera.ToString().Substring(0, 4);
            string mes = fechaEntera.ToString().Substring(4, 2);
            string dia = fechaEntera.ToString().Substring(6, 2);

            retorno = Convert.ToDateTime(dia + "-" + mes + "-" + year);

            return retorno;
        }
        public static int RetornaFechaEntera(DateTime fecha)
        {
            int retorno = 0;
            string dia = RetornaMesDia(fecha.Day);
            string mes = RetornaMesDia(fecha.Month);
            string anno = fecha.Year.ToString();

            retorno = int.Parse(anno + mes + dia);

            return retorno;

        }
        public static string EntregaFechaLetras(DateTime fecha)
        {
            string retorno = "";

            string diaSemanaLetras = fecha.ToString("dddd", new CultureInfo("es-CL"));
            string dia = fecha.Day.ToString();
            string mesLetras = fecha.ToString("MMMM", new CultureInfo("es-CL"));
            string anno = fecha.Year.ToString();

            retorno = diaSemanaLetras + " " + dia + " de " + mesLetras + " del " + anno;

            return retorno;
        }
        public static bool HABILITA_RESTRICCION_FECHA_PROFESOR()
        {
            bool retorno = false;
            if (System.Configuration.ConfigurationManager.AppSettings["HABILITA_RESTRICCION_FECHA_PROFESOR"] != null)
            {
                string habilita = System.Configuration.ConfigurationManager.AppSettings["HABILITA_RESTRICCION_FECHA_PROFESOR"].ToString();
                if (habilita == "1")
                    retorno = true;
            }

            return retorno;
        }
        public static int CANTIDAD_DIAS_RESTRICCION_AGENDA_INICIO()
        {
            int retorno = 15;
            if (System.Configuration.ConfigurationManager.AppSettings["CANTIDAD_DIAS_RESTRICCION_AGENDA_INICIO"] != null)
            {
                string habilita = System.Configuration.ConfigurationManager.AppSettings["CANTIDAD_DIAS_RESTRICCION_AGENDA_INICIO"].ToString();
                retorno = int.Parse(habilita);
            }

            return retorno;
        }
        public static int CANTIDAD_DIAS_CONSULTA_TAREAS()
        {
            int retorno = 30;
            if (System.Configuration.ConfigurationManager.AppSettings["CANTIDAD_DIAS_CONSULTA_TAREAS"] != null)
            {
                string habilita = System.Configuration.ConfigurationManager.AppSettings["CANTIDAD_DIAS_CONSULTA_TAREAS"].ToString();
                retorno = int.Parse(habilita);
            }

            return retorno;
        }
        public static int CANTIDAD_DIAS_RESTRICCION_AGENDA_TERMINO()
        {
            int retorno = 5;
            if (System.Configuration.ConfigurationManager.AppSettings["CANTIDAD_DIAS_RESTRICCION_AGENDA_TERMINO"] != null)
            {
                string habilita = System.Configuration.ConfigurationManager.AppSettings["CANTIDAD_DIAS_RESTRICCION_AGENDA_TERMINO"].ToString();
                retorno = int.Parse(habilita);
            }

            return retorno;
        }
        public static bool PuedeAgendar(int mes)
        {
            if (HABILITA_RESTRICCION_FECHA_PROFESOR() == true)
                return true;
            else
            {
                DateTime fechaHoraActual = DateTime.Now;
                DateTime fechaInicio = Convert.ToDateTime("01-" + RetornaMesDia(mes) + "-" + fechaHoraActual.Year).AddDays(CANTIDAD_DIAS_RESTRICCION_AGENDA_INICIO());
                int diasDelMes = DateTime.DaysInMonth(fechaInicio.Year, mes);
                int difDia = diasDelMes - CANTIDAD_DIAS_RESTRICCION_AGENDA_TERMINO();
                DateTime fechaTermino = Convert.ToDateTime(RetornaMesDia(difDia) + "-" + RetornaMesDia(mes) + "-" + fechaHoraActual.Year);
                //entonces si la fecha actual está en este periodo de tiempo puede agendar
                if (fechaHoraActual >= fechaInicio && fechaHoraActual <= fechaTermino)
                    return true;
                else
                    return false;
            }
        }

    }

    public class Meses
    {
        public int IdMes { get; set; }
        public string Nombre { get; set; }

        public int Anno { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.Entidad
{
    public class Utiles
    {
        public static string RetornaMesDia(int mes)
        {
            string retorno = "01";
            if (mes < 10)
                retorno = "0" + mes.ToString();
            else
                retorno = mes.ToString();

            return retorno;
        }
        public static List<Meses> RetornaMesesAnnoProfesor()
        {
            List<Meses> lista = new List<Meses>();

            DateTime ahora = DateTime.Now;
            DateTime inicio = Convert.ToDateTime("01-" + RetornaMesDia(ahora.Month) + "-" + ahora.Year.ToString()).AddMonths(-1);
            DateTime termino = Convert.ToDateTime("01-" + RetornaMesDia(ahora.Month) + "-" + ahora.Year.ToString()).AddMonths(3);

            int contador = -1;
            int difMeses = Math.Abs((termino.Month - inicio.Month) + 12 * (termino.Year - inicio.Year));

            if (difMeses > 0)
            {
                for (int i = -1; i < difMeses; i++)
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


            return lista;
        }
    }
}

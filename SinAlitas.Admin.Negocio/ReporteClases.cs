using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Data;

namespace SinAlitas.Admin.Negocio
{
    public class ReporteClases
    {
        public static List<Entidad.ReporteClases> ObtenerReporteClases()
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            List<Entidad.ReporteClases> lista = new List<Entidad.ReporteClases>();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(conexionStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OBTENER_PACKS_PROFESORES";
            cmd.Connection.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int FECHA_CREACION = rdr.GetOrdinal("FECHA_CREACION");
                int PRIMERA_CLASE = rdr.GetOrdinal("PRIMERA_CLASE");
                int NOMBRE_PROFESOR = rdr.GetOrdinal("NOMBRE_PROFESOR");
                int NOMBRE = rdr.GetOrdinal("NOMBRE");
                int CODIGO_CLIENTE = rdr.GetOrdinal("CODIGO_CLIENTE");
                int PENDIENTES = rdr.GetOrdinal("PENDIENTES");
                int CERRADAS = rdr.GetOrdinal("CERRADAS");
                int PROGRAMADAS = rdr.GetOrdinal("PROGRAMADAS");
                int ESTA_PAGADO = rdr.GetOrdinal("ESTA_PAGADO");

                try
                {
                    while (rdr.Read())
                    {
                        Entidad.ReporteClases lun = new Entidad.ReporteClases();
                        lun.FechaCreacion = rdr.IsDBNull(FECHA_CREACION) ? DateTime.MinValue : rdr.GetDateTime(FECHA_CREACION);
                        lun.PrimeraClase = rdr.IsDBNull(PRIMERA_CLASE) ? DateTime.MinValue : rdr.GetDateTime(PRIMERA_CLASE);
                        lun.NombreProfesor = rdr.IsDBNull(NOMBRE_PROFESOR) ? "Sin Profesor" : rdr.GetString(NOMBRE_PROFESOR);
                        lun.Nombre = rdr.IsDBNull(NOMBRE) ? "Sin Cliente" : rdr.GetString(NOMBRE);
                        lun.CodigoCliente = rdr.IsDBNull(CODIGO_CLIENTE) ? "" : rdr.GetString(CODIGO_CLIENTE);
                        lun.Pendientes = rdr.GetInt32(PENDIENTES);
                        lun.Cerradas = rdr.GetInt32(CERRADAS);
                        lun.Programadas = rdr.GetInt32(PROGRAMADAS);
                        lun.EstaPagado = rdr.IsDBNull(ESTA_PAGADO) ? "" : rdr.GetString(ESTA_PAGADO);
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
    }
}

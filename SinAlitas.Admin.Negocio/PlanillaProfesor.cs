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
    public class PlanillaProfesor
    {
        public static List<Entidad.ResultadosProfesor> ObtenerMatriz(int fechaEntera)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            List<Entidad.ResultadosProfesor> lista = new List<Entidad.ResultadosProfesor>();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            //cmd = new SqlCommand("select * from RELUN_REGISTRO_LUN where ECOL_ID = @ENCO_ID and ELIMINADO = 1", conn);
            cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(conexionStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OBTENER_MATRIZ_PROFESORES";
            if (conexionStr.Contains("MRT743D508O"))
                fechaEntera = int.Parse(fechaEntera.ToString().Substring(0, 4) + fechaEntera.ToString().Substring(6, 2) + fechaEntera.ToString().Substring(4, 2));

            cmd.Parameters.AddWithValue("@fechaint", fechaEntera);
            cmd.Connection.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int NOMBRES = rdr.GetOrdinal("NOMBRES");
                int CODIGO_CLIENTE = rdr.GetOrdinal("CODIGO_CLIENTE");
                int FECHA_HORA_INICIO = rdr.GetOrdinal("FECHA_HORA_INICIO");
                int FECHA_HORA_TERMINO = rdr.GetOrdinal("FECHA_HORA_TERMINO");

                try
                {
                    while (rdr.Read())
                    {
                        Entidad.ResultadosProfesor lun = new Entidad.ResultadosProfesor();
                        lun.Id = rdr.GetInt32(ID);
                        lun.Nombres = rdr.IsDBNull(NOMBRES) ? "" : rdr.GetString(NOMBRES);
                        lun.CodigoCliente = rdr.IsDBNull(CODIGO_CLIENTE) ? "" : rdr.GetString(CODIGO_CLIENTE);
                        lun.FechaHoraInicio = rdr.IsDBNull(FECHA_HORA_INICIO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_HORA_INICIO);
                        lun.FechaHoraTermino = rdr.IsDBNull(FECHA_HORA_TERMINO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_HORA_TERMINO);

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

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
    public class FichaAlumno
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static int Insetar(SinAlitas.Admin.Entidad.FichaAlumno alumno)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            
            int id = fac.Insertar<SinAlitas.Admin.Entidad.FichaAlumno>(alumno, setCnsWebLun);
            return id;
        }
        public static SinAlitas.Admin.Entidad.FichaAlumno ObtenerFichaPorId(int id)
        {
            SinAlitas.Admin.Entidad.FichaAlumno entidad = new Entidad.FichaAlumno();
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ID";
            filtro.Valor = id.ToString();
            filtro.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);


            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.FichaAlumno>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.FichaAlumno> lista2 = new List<Entidad.FichaAlumno>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.FichaAlumno>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                entidad = lista2[0];
            return entidad;
        }
        public static Entidad.EnvoltorioAlumno ObtenerAlumnoPorId(int id)
        {
            
            Entidad.FichaAlumno ficha = ObtenerFichaPorId(id);

            Entidad.EnvoltorioAlumno al = new Entidad.EnvoltorioAlumno();

            if (ficha.Id > 0)
            {
                al.DondeAcudir = ficha.DondeAcudir;
                al.Edad = ficha.Edad;
                al.Id = ficha.Id;
                al.NombreCompleto = ficha.NombreCompleto;
                al.NumeroEmergencia = ficha.NumeroEmergencia;
                al.Sexo = ficha.Sexo;
                if (ficha.TieneAsma == 1)
                    al.DetalleAsma = "Tiene asma";
                else
                    al.DetalleAsma = "No tiene Asma";
                if (ficha.TieneProblemasCardiacos == 1)
                    al.DetalleProblemasCardiacos = "Tiene Problemas cardiacos, " + ficha.CualesProblemasCardiacos;
                else
                    al.DetalleProblemasCardiacos = "No tiene problemas cardiacos.";
                if (ficha.TieneProblemasMotores == 1)
                    al.DetalleProblemasMotores = "Tiene dificultades motoras, " + ficha.CualesProblemasMotores;
                else
                    al.DetalleProblemasMotores = "No tiene dificultades motoras.";
            }
            return al;

        }

        public static List<Entidad.FichaAlumno> ObtenerFichasPorCliente(int idCliente)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            List<Entidad.FichaAlumno> lista = new List<Entidad.FichaAlumno>();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            //cmd = new SqlCommand("select * from RELUN_REGISTRO_LUN where ECOL_ID = @ENCO_ID and ELIMINADO = 1", conn);
            cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(conexionStr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "OBTENER_FICHAS_POR_CLIENTE";

            cmd.Parameters.AddWithValue("@clie_id", idCliente);
            cmd.Connection.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int NOMBRE_COMPLETO = rdr.GetOrdinal("NOMBRE_COMPLETO");
                int EDAD = rdr.GetOrdinal("EDAD");
                int SEXO = rdr.GetOrdinal("SEXO");
                int TIENE_PROBLEMAS_MOTORES = rdr.GetOrdinal("TIENE_PROBLEMAS_MOTORES");
                int CUALES_PROBLEMAS_MOTORES = rdr.GetOrdinal("CUALES_PROBLEMAS_MOTORES");
                int TIENE_PROBLEMAS_CARDIACOS = rdr.GetOrdinal("TIENE_PROBLEMAS_CARDIACOS");
                int CUALES_PROBLEMAS_CARDIACOS = rdr.GetOrdinal("CUALES_PROBLEMAS_CARDIACOS");
                int TIENE_ASMA = rdr.GetOrdinal("TIENE_ASMA");
                int OTRA_ENFERMEDAD = rdr.GetOrdinal("OTRA_ENFERMEDAD");
                int NUMERO_EMERGENCIA = rdr.GetOrdinal("NUMERO_EMERGENCIA");
                int DONDE_ACUDIR = rdr.GetOrdinal("DONDE_ACUDIR");
                int OBSERVACION = rdr.GetOrdinal("OBSERVACION");


                try
                {
                    while (rdr.Read())
                    {
                        Entidad.FichaAlumno entidad = new Entidad.FichaAlumno();
                        entidad.Id = rdr.GetInt32(ID);
                        entidad.NombreCompleto = rdr.IsDBNull(NOMBRE_COMPLETO) ? "" : rdr.GetString(NOMBRE_COMPLETO);
                        entidad.Edad = rdr.IsDBNull(EDAD) ? 0 : rdr.GetInt32(EDAD);
                        entidad.Sexo = rdr.IsDBNull(SEXO) ? "" : rdr.GetString(SEXO);
                        entidad.TieneProblemasMotores = rdr.IsDBNull(TIENE_PROBLEMAS_MOTORES) ? 0 : rdr.GetInt32(TIENE_PROBLEMAS_MOTORES);
                        entidad.CualesProblemasMotores = rdr.IsDBNull(CUALES_PROBLEMAS_MOTORES) ? "" : rdr.GetString(CUALES_PROBLEMAS_MOTORES);
                        entidad.TieneAsma = rdr.IsDBNull(TIENE_ASMA) ? 0 : rdr.GetInt32(TIENE_ASMA);
                        entidad.OtraEnfermedad = rdr.IsDBNull(OTRA_ENFERMEDAD) ? "" : rdr.GetString(OTRA_ENFERMEDAD);
                        entidad.NumeroEmergencia = rdr.IsDBNull(NUMERO_EMERGENCIA) ? "" : rdr.GetString(NUMERO_EMERGENCIA);
                        entidad.DondeAcudir = rdr.IsDBNull(DONDE_ACUDIR) ? "" : rdr.GetString(DONDE_ACUDIR);
                        entidad.Observacion = rdr.IsDBNull(OBSERVACION) ? "" : rdr.GetString(OBSERVACION);

                        lista.Add(entidad);
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
            List<Entidad.FichaAlumno> listaNueva = new List<Entidad.FichaAlumno>();
            if (lista != null && lista.Count > 0)
            {
                //devolver los nombres no repetidos
                foreach(Entidad.FichaAlumno fic in lista)
                {
                    if (!listaNueva.Exists(p=>p.NombreCompleto == fic.NombreCompleto))
                    {
                        listaNueva.Add(fic);
                    }
                }
            }

            return listaNueva;
        }
    }
}

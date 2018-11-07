using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class Profesor
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static SinAlitas.Admin.Entidad.Profesor ObtenerProfesorPord(int id)
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
            filtro2.Campo = "ID";
            filtro2.Valor = id.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;
            

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

        public static SinAlitas.Admin.Entidad.Profesor ObtenerProfesorPorIdSP(int id)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            SinAlitas.Admin.Entidad.Profesor entidad = new SinAlitas.Admin.Entidad.Profesor();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from PROF_PROFESOR where ID = @id and ELIMINADO = 0", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RUT = rdr.GetOrdinal("RUT");
                int NOMBRES = rdr.GetOrdinal("NOMBRES");
                int PRIMER_APELLIDO = rdr.GetOrdinal("PRIMER_APELLIDO");
                int SEGUNDO_APELLIDO = rdr.GetOrdinal("SEGUNDO_APELLIDO");
                int FOTOGRAFIA = rdr.GetOrdinal("FOTOGRAFIA");
                int CODIGO = rdr.GetOrdinal("CODIGO");
                int PAI_ID = rdr.GetOrdinal("PAI_ID");
                int REG_ID = rdr.GetOrdinal("REG_ID");
                int COM_ID = rdr.GetOrdinal("COM_ID");
                int DIRECCION = rdr.GetOrdinal("DIRECCION");
                int SEXO = rdr.GetOrdinal("SEXO");
                int FECHA_NACIMIENTO = rdr.GetOrdinal("FECHA_NACIMIENTO");
                int NOMBRE_USUARIO = rdr.GetOrdinal("NOMBRE_USUARIO");
                int PASSWORD = rdr.GetOrdinal("PASSWORD");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int TELEFONOS_CONTACTO = rdr.GetOrdinal("TELEFONOS_CONTACTO");
                int BANCO = rdr.GetOrdinal("BANCO");
                int TIPO_CUENTA = rdr.GetOrdinal("TIPO_CUENTA");
                int NUMERO_CUENTA = rdr.GetOrdinal("NUMERO_CUENTA");
                int EMAIL = rdr.GetOrdinal("EMAIL");
                int NOD_ID = rdr.GetOrdinal("NOD_ID");
                int COM_ID_ASIGNADA = rdr.GetOrdinal("COM_ID_ASIGNADA");
                int ROL_ID = rdr.GetOrdinal("ROL_ID");
                try
                {
                    while (rdr.Read())
                    {
                        entidad.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        entidad.Banco = rdr.IsDBNull(BANCO) ? "" : rdr.GetString(BANCO);
                        entidad.Codigo = rdr.IsDBNull(CODIGO) ? "" : rdr.GetString(CODIGO);
                        entidad.ComId = rdr.IsDBNull(COM_ID) ? 0 : rdr.GetInt32(COM_ID);
                        entidad.ComIdAsignada = rdr.IsDBNull(COM_ID_ASIGNADA) ? "" : rdr.GetString(COM_ID_ASIGNADA);
                        entidad.Direccion = rdr.IsDBNull(DIRECCION) ? "" : rdr.GetString(DIRECCION);
                        entidad.Eliminado = 0;
                        entidad.Email = rdr.IsDBNull(EMAIL) ? "" : rdr.GetString(EMAIL);
                        entidad.FechaNacimiento = rdr.IsDBNull(FECHA_NACIMIENTO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_NACIMIENTO);
                        entidad.Fotografia = rdr.IsDBNull(FOTOGRAFIA) ? "" : rdr.GetString(FOTOGRAFIA);
                        entidad.Id = rdr.IsDBNull(ID) ? 0 : rdr.GetInt32(ID);
                        entidad.NodId = rdr.IsDBNull(NOD_ID) ? 0 : rdr.GetInt32(NOD_ID);
                        entidad.Nombres = rdr.IsDBNull(NOMBRES) ? "" : rdr.GetString(NOMBRES);
                        entidad.NombreUsuario = rdr.IsDBNull(NOMBRE_USUARIO) ? "" : rdr.GetString(NOMBRE_USUARIO);
                        entidad.NumeroCuenta = rdr.IsDBNull(NUMERO_CUENTA) ? "" : rdr.GetString(NUMERO_CUENTA);
                        entidad.PaiId = rdr.IsDBNull(PAI_ID) ? 0 : rdr.GetInt32(PAI_ID);
                        entidad.Password = rdr.IsDBNull(PASSWORD) ? "" : rdr.GetString(PASSWORD);
                        entidad.PrimerApellido = rdr.IsDBNull(PRIMER_APELLIDO) ? "" : rdr.GetString(PRIMER_APELLIDO);
                        entidad.RegId = rdr.IsDBNull(REG_ID) ? 0 : rdr.GetInt32(REG_ID);
                        entidad.RolId = rdr.IsDBNull(ROL_ID) ? 0 : rdr.GetInt32(ROL_ID);
                        entidad.Rut = rdr.IsDBNull(RUT) ? "" : rdr.GetString(RUT);
                        entidad.SegundoApellido = rdr.IsDBNull(SEGUNDO_APELLIDO) ? "" : rdr.GetString(SEGUNDO_APELLIDO);
                        entidad.Sexo = rdr.IsDBNull(SEXO) ? "" : rdr.GetString(SEXO);
                        entidad.TelefonosContacto = rdr.IsDBNull(TELEFONOS_CONTACTO) ? "" : rdr.GetString(TELEFONOS_CONTACTO);
                        entidad.TipoCuenta = rdr.IsDBNull(TIPO_CUENTA) ? 0 : rdr.GetInt32(TIPO_CUENTA);

                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                conn.Close();
            }
            return entidad;
        }

        public static SinAlitas.Admin.Entidad.Profesor ObtenerProfesorPorIdSPEliminado(int id)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            SinAlitas.Admin.Entidad.Profesor entidad = new SinAlitas.Admin.Entidad.Profesor();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from PROF_PROFESOR where ID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RUT = rdr.GetOrdinal("RUT");
                int NOMBRES = rdr.GetOrdinal("NOMBRES");
                int PRIMER_APELLIDO = rdr.GetOrdinal("PRIMER_APELLIDO");
                int SEGUNDO_APELLIDO = rdr.GetOrdinal("SEGUNDO_APELLIDO");
                int FOTOGRAFIA = rdr.GetOrdinal("FOTOGRAFIA");
                int CODIGO = rdr.GetOrdinal("CODIGO");
                int PAI_ID = rdr.GetOrdinal("PAI_ID");
                int REG_ID = rdr.GetOrdinal("REG_ID");
                int COM_ID = rdr.GetOrdinal("COM_ID");
                int DIRECCION = rdr.GetOrdinal("DIRECCION");
                int SEXO = rdr.GetOrdinal("SEXO");
                int FECHA_NACIMIENTO = rdr.GetOrdinal("FECHA_NACIMIENTO");
                int NOMBRE_USUARIO = rdr.GetOrdinal("NOMBRE_USUARIO");
                int PASSWORD = rdr.GetOrdinal("PASSWORD");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int TELEFONOS_CONTACTO = rdr.GetOrdinal("TELEFONOS_CONTACTO");
                int BANCO = rdr.GetOrdinal("BANCO");
                int TIPO_CUENTA = rdr.GetOrdinal("TIPO_CUENTA");
                int NUMERO_CUENTA = rdr.GetOrdinal("NUMERO_CUENTA");
                int EMAIL = rdr.GetOrdinal("EMAIL");
                int NOD_ID = rdr.GetOrdinal("NOD_ID");
                int COM_ID_ASIGNADA = rdr.GetOrdinal("COM_ID_ASIGNADA");
                int ROL_ID = rdr.GetOrdinal("ROL_ID");
                try
                {
                    while (rdr.Read())
                    {
                        entidad.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        entidad.Banco = rdr.IsDBNull(BANCO) ? "" : rdr.GetString(BANCO);
                        entidad.Codigo = rdr.IsDBNull(CODIGO) ? "" : rdr.GetString(CODIGO);
                        entidad.ComId = rdr.IsDBNull(COM_ID) ? 0 : rdr.GetInt32(COM_ID);
                        entidad.ComIdAsignada = rdr.IsDBNull(COM_ID_ASIGNADA) ? "" : rdr.GetString(COM_ID_ASIGNADA);
                        entidad.Direccion = rdr.IsDBNull(DIRECCION) ? "" : rdr.GetString(DIRECCION);
                        entidad.Eliminado = 0;
                        entidad.Email = rdr.IsDBNull(EMAIL) ? "" : rdr.GetString(EMAIL);
                        entidad.FechaNacimiento = rdr.IsDBNull(FECHA_NACIMIENTO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_NACIMIENTO);
                        entidad.Fotografia = rdr.IsDBNull(FOTOGRAFIA) ? "" : rdr.GetString(FOTOGRAFIA);
                        entidad.Id = rdr.IsDBNull(ID) ? 0 : rdr.GetInt32(ID);
                        entidad.NodId = rdr.IsDBNull(NOD_ID) ? 0 : rdr.GetInt32(NOD_ID);
                        entidad.Nombres = rdr.IsDBNull(NOMBRES) ? "" : rdr.GetString(NOMBRES);
                        entidad.NombreUsuario = rdr.IsDBNull(NOMBRE_USUARIO) ? "" : rdr.GetString(NOMBRE_USUARIO);
                        entidad.NumeroCuenta = rdr.IsDBNull(NUMERO_CUENTA) ? "" : rdr.GetString(NUMERO_CUENTA);
                        entidad.PaiId = rdr.IsDBNull(PAI_ID) ? 0 : rdr.GetInt32(PAI_ID);
                        entidad.Password = rdr.IsDBNull(PASSWORD) ? "" : rdr.GetString(PASSWORD);
                        entidad.PrimerApellido = rdr.IsDBNull(PRIMER_APELLIDO) ? "" : rdr.GetString(PRIMER_APELLIDO);
                        entidad.RegId = rdr.IsDBNull(REG_ID) ? 0 : rdr.GetInt32(REG_ID);
                        entidad.RolId = rdr.IsDBNull(ROL_ID) ? 0 : rdr.GetInt32(ROL_ID);
                        entidad.Rut = rdr.IsDBNull(RUT) ? "" : rdr.GetString(RUT);
                        entidad.SegundoApellido = rdr.IsDBNull(SEGUNDO_APELLIDO) ? "" : rdr.GetString(SEGUNDO_APELLIDO);
                        entidad.Sexo = rdr.IsDBNull(SEXO) ? "" : rdr.GetString(SEXO);
                        entidad.TelefonosContacto = rdr.IsDBNull(TELEFONOS_CONTACTO) ? "" : rdr.GetString(TELEFONOS_CONTACTO);
                        entidad.TipoCuenta = rdr.IsDBNull(TIPO_CUENTA) ? 0 : rdr.GetInt32(TIPO_CUENTA);

                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                conn.Close();
            }
            return entidad;
        }
        public static SinAlitas.Admin.Entidad.Profesor ObtenerProfesorPorUsuarioPasswordSP(string usuario, string password)
        {
            //por cambio de implementación se cuentan todos inclusive los eliminados
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-CL");
            SinAlitas.Admin.Entidad.Profesor entidad = new SinAlitas.Admin.Entidad.Profesor();
            string conexionStr = ConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(conexionStr);
            SqlCommand cmd = null;
            cmd = new SqlCommand("select * from PROF_PROFESOR where NOMBRE_USUARIO = @USAUARIO and PASSWORD = @PASSWORD and ELIMINADO = 0", conn);
            cmd.Parameters.AddWithValue("@USAUARIO", usuario);
            cmd.Parameters.AddWithValue("@PASSWORD", password);
            conn.Open();


            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                int ID = rdr.GetOrdinal("ID");
                int RUT = rdr.GetOrdinal("RUT");
                int NOMBRES = rdr.GetOrdinal("NOMBRES");
                int PRIMER_APELLIDO = rdr.GetOrdinal("PRIMER_APELLIDO");
                int SEGUNDO_APELLIDO = rdr.GetOrdinal("SEGUNDO_APELLIDO");
                int FOTOGRAFIA = rdr.GetOrdinal("FOTOGRAFIA");
                int CODIGO = rdr.GetOrdinal("CODIGO");
                int PAI_ID= rdr.GetOrdinal("PAI_ID");
                int REG_ID = rdr.GetOrdinal("REG_ID");
                int COM_ID = rdr.GetOrdinal("COM_ID");
                int DIRECCION = rdr.GetOrdinal("DIRECCION");
                int SEXO = rdr.GetOrdinal("SEXO");
                int FECHA_NACIMIENTO = rdr.GetOrdinal("FECHA_NACIMIENTO");
                int NOMBRE_USUARIO = rdr.GetOrdinal("NOMBRE_USUARIO");
                int PASSWORD = rdr.GetOrdinal("PASSWORD");
                int ACTIVO = rdr.GetOrdinal("ACTIVO");
                int ELIMINADO = rdr.GetOrdinal("ELIMINADO");
                int TELEFONOS_CONTACTO = rdr.GetOrdinal("TELEFONOS_CONTACTO");
                int BANCO = rdr.GetOrdinal("BANCO");
                int TIPO_CUENTA = rdr.GetOrdinal("TIPO_CUENTA");
                int NUMERO_CUENTA = rdr.GetOrdinal("NUMERO_CUENTA");
                int EMAIL = rdr.GetOrdinal("EMAIL");
                int NOD_ID = rdr.GetOrdinal("NOD_ID");
                int COM_ID_ASIGNADA = rdr.GetOrdinal("COM_ID_ASIGNADA");
                int ROL_ID = rdr.GetOrdinal("ROL_ID");
                try
                {
                    while (rdr.Read())
                    {
                        entidad.Activo = rdr.IsDBNull(ACTIVO) ? 0 : rdr.GetInt32(ACTIVO);
                        entidad.Banco = rdr.IsDBNull(BANCO) ? "" : rdr.GetString(BANCO);
                        entidad.Codigo = rdr.IsDBNull(CODIGO) ? "" : rdr.GetString(CODIGO);
                        entidad.ComId = rdr.IsDBNull(COM_ID) ? 0 : rdr.GetInt32(COM_ID);
                        entidad.ComIdAsignada = rdr.IsDBNull(COM_ID_ASIGNADA) ? "" : rdr.GetString(COM_ID_ASIGNADA);
                        entidad.Direccion = rdr.IsDBNull(DIRECCION) ? "" : rdr.GetString(DIRECCION);
                        entidad.Eliminado = 0;
                        entidad.Email = rdr.IsDBNull(EMAIL) ? "" : rdr.GetString(EMAIL);
                        entidad.FechaNacimiento = rdr.IsDBNull(FECHA_NACIMIENTO) ? DateTime.MinValue : rdr.GetDateTime(FECHA_NACIMIENTO);
                        entidad.Fotografia = rdr.IsDBNull(FOTOGRAFIA) ? "" : rdr.GetString(FOTOGRAFIA);
                        entidad.Id = rdr.IsDBNull(ID) ? 0 : rdr.GetInt32(ID);
                        entidad.NodId = rdr.IsDBNull(NOD_ID) ? 0 : rdr.GetInt32(NOD_ID);
                        entidad.Nombres = rdr.IsDBNull(NOMBRES) ? "" : rdr.GetString(NOMBRES);
                        entidad.NombreUsuario = rdr.IsDBNull(NOMBRE_USUARIO) ? "" : rdr.GetString(NOMBRE_USUARIO);
                        entidad.NumeroCuenta = rdr.IsDBNull(NUMERO_CUENTA) ? "" : rdr.GetString(NUMERO_CUENTA);
                        entidad.PaiId = rdr.IsDBNull(PAI_ID) ? 0 : rdr.GetInt32(PAI_ID);
                        entidad.Password = rdr.IsDBNull(PASSWORD) ? "" : rdr.GetString(PASSWORD);
                        entidad.PrimerApellido = rdr.IsDBNull(PRIMER_APELLIDO) ? "" : rdr.GetString(PRIMER_APELLIDO);
                        entidad.RegId = rdr.IsDBNull(REG_ID) ? 0 : rdr.GetInt32(REG_ID);
                        entidad.RolId = rdr.IsDBNull(ROL_ID) ? 0 : rdr.GetInt32(ROL_ID);
                        entidad.Rut = rdr.IsDBNull(RUT) ? "" : rdr.GetString(RUT);
                        entidad.SegundoApellido = rdr.IsDBNull(SEGUNDO_APELLIDO) ? "" : rdr.GetString(SEGUNDO_APELLIDO);
                        entidad.Sexo = rdr.IsDBNull(SEXO) ? "" : rdr.GetString(SEXO);
                        entidad.TelefonosContacto = rdr.IsDBNull(TELEFONOS_CONTACTO) ? "" : rdr.GetString(TELEFONOS_CONTACTO);
                        entidad.TipoCuenta = rdr.IsDBNull(TIPO_CUENTA) ? 0 : rdr.GetInt32(TIPO_CUENTA);

                    }
                }
                finally
                {
                    rdr.Close();
                }
            }
            finally
            {
                conn.Close();
            }
            return entidad;
        }

        public static SinAlitas.Admin.Entidad.Profesor ObtenerProfesorPorUsuarioPassword(string usuario, string password)
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
            filtro2.Campo = "NOMBRE_USUARIO";
            filtro2.Valor = usuario;
            filtro2.TipoDato = TipoDatoGeneral.Varchar;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PASSWORD";
            filtro3.Valor = password;
            filtro3.TipoDato = TipoDatoGeneral.Varchar;
            

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

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

        public static Entidad.EnvoltorioProfesor ObtenerEnvoltorioProfesorPorUsuarioPassword(string usuario, string password)
        {
            Entidad.EnvoltorioProfesor profesor = new Entidad.EnvoltorioProfesor();

            //profesor.Profesor = ObtenerProfesorPorUsuarioPassword(usuario, password);
            profesor.Profesor = ObtenerProfesorPorUsuarioPasswordSP(usuario, password);

            if (profesor.Profesor != null && profesor.Profesor.Id > 0)
            {
                profesor.Pais = new Entidad.Pais();
                profesor.Pais = Negocio.Territorio.ObtenerPaisPorId(profesor.Profesor.PaiId);

                profesor.Region = new Entidad.Region();
                profesor.Region = Negocio.Territorio.ObtenerRegionPorId(profesor.Profesor.RegId);

                profesor.Comuna = new Entidad.Comuna();
                profesor.Comuna = Negocio.Territorio.ObtenerComunanPorId(profesor.Profesor.ComId);

                profesor.Nodo = new Entidad.Nodo();
                profesor.Nodo = Negocio.Territorio.ObtenerNodoPorId(profesor.Profesor.NodId);

                profesor.Rol = new Entidad.Rol();
                profesor.Rol = Negocio.Global.ObtenerRolPorId(profesor.Profesor.RolId);

            }

            return profesor;
        }

        public static Entidad.EnvoltorioProfesor ObtenerEnvoltorioProfesorPorId(int id)
        {
            Entidad.EnvoltorioProfesor profesor = new Entidad.EnvoltorioProfesor();

            //profesor.Profesor = ObtenerProfesorPord(id);
            profesor.Profesor = ObtenerProfesorPorIdSP(id);

            if (profesor.Profesor != null && profesor.Profesor.Id > 0)
            {
                profesor.Pais = new Entidad.Pais();
                profesor.Pais = Negocio.Territorio.ObtenerPaisPorId(profesor.Profesor.PaiId);

                profesor.Region = new Entidad.Region();
                profesor.Region = Negocio.Territorio.ObtenerRegionPorId(profesor.Profesor.RegId);

                profesor.Comuna = new Entidad.Comuna();
                profesor.Comuna = Negocio.Territorio.ObtenerComunanPorId(profesor.Profesor.ComId);

                profesor.Nodo = new Entidad.Nodo();
                profesor.Nodo = Negocio.Territorio.ObtenerNodoPorId(profesor.Profesor.NodId);

                profesor.Rol = new Entidad.Rol();
                profesor.Rol = Negocio.Global.ObtenerRolPorId(profesor.Profesor.RolId);

            }

            return profesor;
        }

        public static List<SinAlitas.Admin.Entidad.Profesor> ObtenerProfesores()
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ACTIVO";
            filtro.Valor = "1";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ROL_ID";
            filtro1.Valor = "3";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Profesor>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Profesor> lista2 = new List<Entidad.Profesor>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Profesor>().ToList();

            }
            if (lista2 != null && lista2.Count > 0)
            {
                foreach(Entidad.Profesor prof in lista2)
                {
                    if (prof.Fotografia == null || prof.Fotografia == string.Empty)
                        prof.Fotografia = "img/no_disponible.png";
                }
            }
            return lista2;
        }
    }
}

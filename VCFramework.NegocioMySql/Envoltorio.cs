using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.NegocioMySql
{
    public class Envoltorio
    {
        public static Entidad.Envoltorio ObtenerEnvoltorio(string codigoCliente)
        {
            Entidad.Envoltorio env = new Entidad.Envoltorio();
            try
            {
                env.ProductoCodigo = VCFramework.NegocioMySql.ProductoCodigo.ObtenerPorCodigoPack(codigoCliente);
                env.Producto = new Entidad.Producto();
                env.Cliente = new Entidad.Cliente();
                env.RegionCliente = new Entidad.Region();
                env.ComunaCliente = new Entidad.Comuna();
                env.TieneAceptaCondiciones = false;
                env.AceptaCondiciones = new Entidad.AceptaCondiciones();
                env.Cupos = new List<Entidad.Cupo>();
                env.Profesor = new Entidad.Profesor();
                int idProfesorAsignado = 0;

                if (env.ProductoCodigo != null && env.ProductoCodigo.Id > 0)
                {
                    env.Producto = VCFramework.NegocioMySql.Producto.ObtenerPorId(env.ProductoCodigo.ProId);
                    env.Cliente = VCFramework.NegocioMySql.Cliente.ObtenerPorId(env.ProductoCodigo.ClieId);
                    env.RegionCliente = VCFramework.NegocioMySql.Region.ObtenerPorId(env.Cliente.RegId);
                    env.ComunaCliente = VCFramework.NegocioMySql.Comuna.ObtenerPorId(env.Cliente.ComId);
                    env.AceptaCondiciones = VCFramework.NegocioMySql.AceptaCondiciones.ObtenerPorPcoId(env.ProductoCodigo.Id);
                    if (env.AceptaCondiciones != null && env.AceptaCondiciones.Id > 0)
                    {
                        env.TieneAceptaCondiciones = true;
                    }
                    env.Cupos = VCFramework.NegocioMySql.Cupo.ObtenerPorPcoId(env.ProductoCodigo.Id);
                    if (env.Cupos != null && env.Cupos.Count > 0)
                    {
                        idProfesorAsignado = env.Cupos[0].ProfId;
                    }
                    if (idProfesorAsignado > 0)
                        env.Profesor = VCFramework.NegocioMySql.Profesor.ObtenerPorId(idProfesorAsignado);
                    //ficha alumnos
                    env.FichaAlumnos = VCFramework.NegocioMySql.EnvoltorioFicha.ListarPorIdPack(env.ProductoCodigo.Id);
                }
            }
            catch(Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }


            return env;
        }

        public static Entidad.Envoltorio ObtenerEnvoltorio(int idPack)
        {
            Entidad.Envoltorio env = new Entidad.Envoltorio();
            try
            {
                env.ProductoCodigo = VCFramework.NegocioMySql.ProductoCodigo.ObtenerPorCodigoPack(idPack);
                env.Producto = new Entidad.Producto();
                env.Cliente = new Entidad.Cliente();
                env.RegionCliente = new Entidad.Region();
                env.ComunaCliente = new Entidad.Comuna();
                env.TieneAceptaCondiciones = false;
                env.AceptaCondiciones = new Entidad.AceptaCondiciones();
                env.Cupos = new List<Entidad.Cupo>();
                env.Profesor = new Entidad.Profesor();
                int idProfesorAsignado = 0;

                if (env.ProductoCodigo != null && env.ProductoCodigo.Id > 0)
                {
                    env.Producto = VCFramework.NegocioMySql.Producto.ObtenerPorId(env.ProductoCodigo.ProId);
                    env.Cliente = VCFramework.NegocioMySql.Cliente.ObtenerPorId(env.ProductoCodigo.ClieId);
                    env.RegionCliente = VCFramework.NegocioMySql.Region.ObtenerPorId(env.Cliente.RegId);
                    env.ComunaCliente = VCFramework.NegocioMySql.Comuna.ObtenerPorId(env.Cliente.ComId);
                    env.AceptaCondiciones = VCFramework.NegocioMySql.AceptaCondiciones.ObtenerPorPcoId(env.ProductoCodigo.Id);
                    if (env.AceptaCondiciones != null && env.AceptaCondiciones.Id > 0)
                    {
                        env.TieneAceptaCondiciones = true;
                    }
                    env.Cupos = VCFramework.NegocioMySql.Cupo.ObtenerPorPcoId(env.ProductoCodigo.Id);
                    if (env.Cupos != null && env.Cupos.Count > 0)
                    {
                        idProfesorAsignado = env.Cupos[0].ProfId;
                    }
                    if (idProfesorAsignado > 0)
                        env.Profesor = VCFramework.NegocioMySql.Profesor.ObtenerPorId(idProfesorAsignado);
                    //ficha alumnos
                    env.FichaAlumnos = VCFramework.NegocioMySql.EnvoltorioFicha.ListarPorIdPack(env.ProductoCodigo.Id);
                }
            }
            catch (Exception ex)
            {
                VCFramework.NegocioMySql.Utiles.Log(ex);
            }


            return env;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCFramework.NegocioMySql
{
    public class EnvoltorioFicha
    {
        public static List<Entidad.EnvoltorioFicha> ListarPorIdPack(int idPack)
        {
            List<Entidad.EnvoltorioFicha> lista = new List<Entidad.EnvoltorioFicha>();

            List<Entidad.FichaPack> fichaPacks = NegocioMySql.FichaPack.ObtenerPorIdpack(idPack);
            if (fichaPacks != null && fichaPacks.Count > 0)
            {
                foreach (Entidad.FichaPack fic in fichaPacks) {
                    Entidad.FichaAlumno alumno = NegocioMySql.FichaAlumno.ObtenerPorId(fic.IdAlumno);
                    if (alumno != null && alumno.Id > 0)
                    {
                        if (!lista.Exists(p => p.NombreCompleto == alumno.NombreCompleto))
                        {
                            Entidad.EnvoltorioFicha env = new Entidad.EnvoltorioFicha();
                            env.CualesProblemasCardiacos = alumno.CualesProblemasCardiacos;
                            env.CualesProblemasMotores = alumno.CualesProblemasMotores;
                            env.DondeAcudir = alumno.DondeAcudir;
                            env.Edad = alumno.Edad;
                            env.FechaCreacion = fic.FechaCreacion;
                            env.Id = alumno.Id;
                            env.IdFichaPack = fic.Id;
                            env.IdPack = idPack;
                            env.NombreCompleto = alumno.NombreCompleto;
                            env.NumeroEmergencia = alumno.NumeroEmergencia;
                            env.Observacion = alumno.Observacion;
                            env.OtraEnfermedad = alumno.OtraEnfermedad;
                            env.Sexo = alumno.Sexo;
                            env.TieneAsma = alumno.TieneAsma;
                            env.TieneProblemasCardiacos = alumno.TieneProblemasCardiacos;
                            env.TieneProblemasMotores = alumno.TieneProblemasMotores;
                            lista.Add(env);
                        }
                    }
                }
            }

            return lista;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VCFramework.Entidad;
using VCFramework.NegocioMySql;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;

namespace WebApiSinAlitas.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FichaAlumnoController : ApiController
    {
        [AcceptVerbs("OPTIONS")]
        public void Options()
        { }

        [System.Web.Http.AcceptVerbs("POST")]
        public HttpResponseMessage Post(dynamic DynamicClass)
        {

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            //validaciones antes de ejecutar la llamada.
            if (data.Id == null)
                throw new ArgumentNullException("Id");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string id = data.Id;


                List<VCFramework.Entidad.EnvoltorioFicha> fichas = VCFramework.NegocioMySql.EnvoltorioFicha.ListarPorIdPack(int.Parse(id));


                if (fichas != null && fichas.Count > 0)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(fichas);
                    httpResponse.Content = new StringContent(JSON);
                    httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(VCFramework.NegocioMySql.Utiles.JSON_DOCTYPE);
                }
                else
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                }

            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                throw ex;
            }
            return httpResponse;


        }

        [System.Web.Http.AcceptVerbs("PUT")]
        public HttpResponseMessage Put(dynamic DynamicClass)
        {

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            string idPack = data.IdPack;

            string idAlumno = string.IsNullOrEmpty(Convert.ToString(data.IdAlumno)) ? "0" : Convert.ToString(data.IdAlumno);
            string nombreCompleto = string.IsNullOrEmpty(Convert.ToString(data.NombreCompleto)) ? "" : Convert.ToString(data.NombreCompleto);
            string edad = string.IsNullOrEmpty(Convert.ToString(data.Edad)) ? "0" : Convert.ToString(data.Edad);
            string sexo = string.IsNullOrEmpty(Convert.ToString(data.Sexo)) ? "" : Convert.ToString(data.Sexo);
            string tieneProblemasMotores = string.IsNullOrEmpty(Convert.ToString(data.TieneProblemasMotores)) ? "0" : Convert.ToString(data.TieneProblemasMotores);
            string cualesProblemasMotores = string.IsNullOrEmpty(Convert.ToString(data.CualesProblemasMotores)) ? "" : Convert.ToString(data.CualesProblemasMotores);
            string tieneProblemasCardiacos = string.IsNullOrEmpty(Convert.ToString(data.TieneProblemasCardiacos)) ? "0" : Convert.ToString(data.TieneProblemasCardiacos);
            string cualesProblemasCardiacos = string.IsNullOrEmpty(Convert.ToString(data.CualesProblemasCardiacos)) ? "" : Convert.ToString(data.CualesProblemasCardiacos);
            string tieneAsma = string.IsNullOrEmpty(Convert.ToString(data.TieneAsma)) ? "0" : Convert.ToString(data.TieneAsma);
            string otraEnfermedad = string.IsNullOrEmpty(Convert.ToString(data.OtraEnfermedad)) ? "" : Convert.ToString(data.OtraEnfermedad);
            string numeroEmergencia = string.IsNullOrEmpty(Convert.ToString(data.NumeroEmergencia)) ? "" : Convert.ToString(data.NumeroEmergencia);
            string dondeAcudir = string.IsNullOrEmpty(Convert.ToString(data.DondeAcudir)) ? "" : Convert.ToString(data.DondeAcudir);
            string observacion = string.IsNullOrEmpty(Convert.ToString(data.Observacion)) ? "" : Convert.ToString(data.Observacion);

            int nuevoIdAlumno = 0;
            int nuevoIdFichaPack = 0;
            bool existeAlumno = false;


            if (idPack == null)
                throw new ArgumentNullException("PcoId");
            if (nombreCompleto == "" || edad == "" || dondeAcudir == "" || numeroEmergencia == "")
                throw new ArgumentNullException("Debe agregar datos");

            //en este caso vamos a retornar una lista de alumnos de la ficha
            List<VCFramework.Entidad.EnvoltorioFicha> fichas = new List<VCFramework.Entidad.EnvoltorioFicha>();
            VCFramework.Entidad.FichaAlumno fichaAlumno = new VCFramework.Entidad.FichaAlumno();
            
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //verificamos si existe el elemento
                if (idAlumno != "0")
                {
                    //si existe entonces hay que traerlo ya que esta editando
                    fichaAlumno = VCFramework.NegocioMySql.FichaAlumno.ObtenerPorId(int.Parse(idAlumno));
                    existeAlumno = true;

                }
                fichaAlumno.CualesProblemasCardiacos = cualesProblemasCardiacos;
                fichaAlumno.CualesProblemasMotores = cualesProblemasMotores;
                fichaAlumno.DondeAcudir = dondeAcudir;
                fichaAlumno.Edad = int.Parse(edad);
                fichaAlumno.NombreCompleto = nombreCompleto;
                fichaAlumno.NumeroEmergencia = numeroEmergencia;
                fichaAlumno.Observacion = observacion;
                fichaAlumno.OtraEnfermedad = otraEnfermedad;
                fichaAlumno.Sexo = sexo;
                fichaAlumno.TieneAsma = int.Parse(tieneAsma);
                fichaAlumno.TieneProblemasCardiacos = int.Parse(tieneProblemasCardiacos);
                fichaAlumno.TieneProblemasMotores = int.Parse(tieneProblemasMotores);
                
                //es nuevo o antiguo
                if (existeAlumno)
                {
                    VCFramework.NegocioMySql.FichaAlumno.Modificar(fichaAlumno);
                }
                else
                {
                    nuevoIdAlumno = VCFramework.NegocioMySql.FichaAlumno.Insertar(fichaAlumno);
                    VCFramework.Entidad.FichaPack ficPack = new VCFramework.Entidad.FichaPack
                    {
                        Activo = 1,
                        Eliminado = 0,
                        FechaCreacion = DateTime.Now,
                        IdAlumno = nuevoIdAlumno,
                        IdPack = int.Parse(idPack)
                    };
                    nuevoIdFichaPack = VCFramework.NegocioMySql.FichaPack.Insertar(ficPack);

                }

                fichas = VCFramework.NegocioMySql.EnvoltorioFicha.ListarPorIdPack(int.Parse(idPack));

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(fichas);
                httpResponse.Content = new StringContent(JSON);
                httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(VCFramework.NegocioMySql.Utiles.JSON_DOCTYPE);


            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                throw ex;
            }

            return httpResponse;
        }

    }
}
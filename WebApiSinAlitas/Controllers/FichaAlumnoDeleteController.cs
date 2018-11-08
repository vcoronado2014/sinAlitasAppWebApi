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
    public class FichaAlumnoDeleteController : ApiController
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
            if (data.IdPack == null)
                throw new ArgumentNullException("Id Pack");
            if (data.IdAlumno == null)
                throw new ArgumentNullException("Id alumno");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string idPack = data.IdPack;
                string idAlumno = data.IdAlumno;


                List<SinAlitas.Admin.Entidad.FichaPack> ficha = SinAlitas.Admin.Negocio.FichaPack.ObtenerFichasPack(int.Parse(idPack));


                if (ficha != null && ficha.Count   > 0)
                {
                    SinAlitas.Admin.Entidad.FichaPack fichaBorar = ficha.Find(p => p.IdAlumno == int.Parse(idAlumno));
                    if (fichaBorar != null && fichaBorar.Id > 0)
                    {
                        SinAlitas.Admin.Negocio.FichaPack.Eliminar(fichaBorar);
                    }


                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(ficha);
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
    }
}
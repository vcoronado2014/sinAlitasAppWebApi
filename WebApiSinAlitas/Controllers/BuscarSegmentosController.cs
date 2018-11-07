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
    public class BuscarSegmentosController : ApiController
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
            if (data.NodId == "")
                throw new ArgumentNullException("NodId");

            if (data.FechaEntera == "")
                throw new ArgumentNullException("FechaEntera");
            if (data.ProfId == "")
                throw new ArgumentNullException("ProfId");
            if (data.EsBloqueado == "")
                throw new ArgumentNullException("EsBloqueado");

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string nodId = data.NodId;
                string fechaEntera = data.FechaEntera;
                string profId = data.ProfId;
                string esBloqueado = data.EsBloqueado;
                List<SinAlitas.Admin.Entidad.SegmentoMostrar> segmentos = SinAlitas.Admin.Negocio.SegmentoHorario.BuscarSegmentoPorFechaNuevo(
                    int.Parse(fechaEntera),
                    int.Parse(nodId),
                    int.Parse(profId),
                    int.Parse(esBloqueado)
                    );


                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(segmentos);
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
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
    public class AceptaCondicionesController : ApiController
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
            if (data.PcoId == "")
                throw new ArgumentNullException("PcoId");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string pcoId = data.PcoId;


                VCFramework.Entidad.AceptaCondiciones producto = VCFramework.NegocioMySql.AceptaCondiciones.ObtenerPorPcoId(int.Parse(pcoId));


                if (producto != null && producto.Id > 0)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(producto);
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
            if (idPack == null)
                throw new ArgumentNullException("PcoId");


            //validaciones antes de ejecutar la llamada.
            VCFramework.Entidad.AceptaCondiciones acpeta = new VCFramework.Entidad.AceptaCondiciones();

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                acpeta.PcoId = int.Parse(idPack);
                acpeta.Activo = 1;
                acpeta.EsAceptado = 1;
                acpeta.EsCompletado = 0;
                acpeta.FechaAcepta = DateTime.Now;
                int nuevoId = VCFramework.NegocioMySql.AceptaCondiciones.Insertar(acpeta);
                acpeta.Id = nuevoId;

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(acpeta);
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
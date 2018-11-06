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
    public class ClientesController : ApiController
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
            if (data.Usuario == "")
                throw new ArgumentNullException("Usuario");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string usuario = data.Usuario;
                List<VCFramework.Entidad.Cliente> distinctPeople = new List<VCFramework.Entidad.Cliente>();

                List<VCFramework.Entidad.Cliente> clientes = VCFramework.NegocioMySql.Cliente.Listar();
                if (clientes != null && clientes.Count > 0)
                {
                    distinctPeople = clientes
                      .GroupBy(p => new { p.Nombres, p.PrimerApellido, p.SegundoApellido })
                      .Select(g => g.First())
                      .ToList();
                }
                distinctPeople = distinctPeople.OrderBy(p => p.Nombres).ToList();

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(distinctPeople);
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
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
    public class ClientePaackProductoController : ApiController
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
            if (data.PcoId == null)
                throw new ArgumentNullException("PcoId");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string pcoId = data.PcoId;

                SinAlitas.Admin.Entidad.EnvoltorioClientePackProducto envoltorio = SinAlitas.Admin.Negocio.EnvoltorioClientePackProducto.Obtener(int.Parse(pcoId));
                //VCFramework.Entidad.ProductoCodigo producto = VCFramework.NegocioMySql.ProductoCodigo.ObtenerPorCodigoPack(nombre);
                //List<SinAlitas.Admin.Entidad.ProductoCodigoTexto> producto = SinAlitas.Admin.Negocio.ProductoCodigo.ObtenerProductoCodigoGrilla(int.Parse(estado), codigo);


                if (envoltorio != null)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(envoltorio);
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
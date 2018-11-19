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
    public class ProductoCodigoController : ApiController
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
            if (data.Nombre == "")
                throw new ArgumentNullException("Nombre");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string nombre = data.Nombre;


                VCFramework.Entidad.ProductoCodigo producto = VCFramework.NegocioMySql.ProductoCodigo.ObtenerPorCodigoPack(nombre);


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


            string idCliente = string.IsNullOrEmpty(Convert.ToString(data.IdCliente)) ? "0" : Convert.ToString(data.IdCliente);
            string codigoCliente = string.IsNullOrEmpty(Convert.ToString(data.CodigoCliente)) ? "" : Convert.ToString(data.CodigoCliente);
            string proId = string.IsNullOrEmpty(Convert.ToString(data.ProId)) ? "0" : Convert.ToString(data.ProId);
            string cantidadClases = string.IsNullOrEmpty(Convert.ToString(data.CantidadClases)) ? "0" : Convert.ToString(data.CantidadClases);
            string descuento = string.IsNullOrEmpty(Convert.ToString(data.Descuento)) ? "0" : Convert.ToString(data.Descuento);
            string totalPack = string.IsNullOrEmpty(Convert.ToString(data.TotalPack)) ? "0" : Convert.ToString(data.TotalPack);
            string cantidadAlumnos = string.IsNullOrEmpty(Convert.ToString(data.CantidadAlumnos)) ? "0" : Convert.ToString(data.CantidadAlumnos);


            int nuevoId = 0;
            //bool existe = false;

            VCFramework.Entidad.ProductoCodigo pco = new VCFramework.Entidad.ProductoCodigo();

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //verificamos si existe el elemento
                pco.Activo = 1;
                pco.CantidadAlumnos = int.Parse(cantidadAlumnos);
                pco.CantidadClases = int.Parse(cantidadClases);
                pco.ClieId = int.Parse(idCliente);
                pco.CodigoCliente = codigoCliente;
                pco.Eliminado = 0;
                pco.Estado = 1;
                pco.EstaPagado = 0;
                pco.FechaCreacion = DateTime.Now;
                pco.PorcentajeDescuento = int.Parse(descuento);
                pco.ProId = int.Parse(proId);
                pco.TotalPack = int.Parse(totalPack);

                nuevoId = VCFramework.NegocioMySql.ProductoCodigo.Insertar(pco);
                pco.Id = nuevoId;


                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(pco);
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
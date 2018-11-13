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
    public class CupoController : ApiController
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


                List<VCFramework.Entidad.Cupo> producto = VCFramework.NegocioMySql.Cupo.ObtenerPorPcoId(int.Parse(pcoId));


                if (producto != null && producto.Count > 0)
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
        //este put lo ocuparemos para eliminarel cupo
        [System.Web.Http.AcceptVerbs("PUT")]
        public HttpResponseMessage Put(dynamic DynamicClass)
        {

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);


            string id = string.IsNullOrEmpty(Convert.ToString(data.Id)) ? "0" : Convert.ToString(data.Id);
            string motivo = string.IsNullOrEmpty(Convert.ToString(data.Motivo)) ? "" : Convert.ToString(data.Motivo);
            
            
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //verificamos si existe el elemento
                SinAlitas.Admin.Entidad.Cupo cupo = SinAlitas.Admin.Negocio.Cupo.ObtenerCupoPorId(int.Parse(id));
                if (cupo != null && cupo.Id > 0)
                {
                    DateTime fechaCupo = cupo.FechaHoraTermino;
                    if (fechaCupo.AddHours(24) > DateTime.Now)
                    {

                        SinAlitas.Admin.Entidad.CancelacionClase obs = new SinAlitas.Admin.Entidad.CancelacionClase();
                        obs.CpoId = cupo.Id;
                        obs.FechaCancelacion = DateTime.Now;
                        //aca el motivo de cancelacion
                        obs.Motivo = motivo;
                        //****************************
                        obs.PcoId = cupo.PcoId;
                        obs.ProfId = cupo.ProfId;


                        cupo.EstadoCupo = 0; //lo volvemnos a 0
                        cupo.PcoId = 0;
                        cupo.ClieId = 0;
                        cupo.Descripcion = "";

                        if (SinAlitas.Admin.Negocio.Cupo.Actualizar(cupo) > 0)
                        {
                            if (SinAlitas.Admin.Negocio.CancelacionClase.Insetar(obs) > 0)
                            {
                                #region insertar historial
                                SinAlitas.Admin.Entidad.HistorialMovimiento historial = new SinAlitas.Admin.Entidad.HistorialMovimiento();
                                historial.TipoMovimiento = "Profesor Cancela Clase|" + cupo.Id.ToString();
                                historial.UsuarioMovimiento = cupo.ProfId;
                                historial.UsuarioOrigen = cupo.ProfId;
                                SinAlitas.Admin.Negocio.HistorialMovimiento.Insetar(historial);
                                #endregion

                            }
                        }
                        else
                        {
                            //no puede cancelar un cupo con fecha manor a 24 horas
                            httpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                        }

                        //response
                        httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                        String JSON = JsonConvert.SerializeObject(cupo);
                        httpResponse.Content = new StringContent(JSON);
                        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(VCFramework.NegocioMySql.Utiles.JSON_DOCTYPE);
                    }
                    else
                    {
                        //no puede cancelar un cupo con fecha manor a 24 horas
                        httpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                    }
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
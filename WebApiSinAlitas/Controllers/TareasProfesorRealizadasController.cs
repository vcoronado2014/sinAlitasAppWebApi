﻿using System;
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
    public class TareasProfesorRealizadasController : ApiController
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
            if (data.FechaEntera == null)
                throw new ArgumentNullException("Fecha Entera");

            if (data.ProfId == null)
                throw new ArgumentNullException("Prof Id");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string profId = data.ProfId;
                string fechaEntera = data.FechaEntera;
                //lo pasamos por defecto
                int nodId = 1;
                List<SinAlitas.Admin.Entidad.TareasEnvoltorio> tareasEnv = SinAlitas.Admin.Negocio.TareasProfesor.ObtenerTareasProfesorEnvoltorioRealizadas(
                    int.Parse(fechaEntera), nodId, int.Parse(profId));


                if (tareasEnv != null && tareasEnv.Count > 0)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(tareasEnv);
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
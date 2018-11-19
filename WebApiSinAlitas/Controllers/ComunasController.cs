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
    public class ComunasController : ApiController
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
            if (data.IdProfesor == "")
                throw new ArgumentNullException("IdProfesor");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string id = data.IdProfesor;


                List<VCFramework.Entidad.Comuna> comunas = VCFramework.NegocioMySql.Comuna.Listar();
                VCFramework.Entidad.Profesor profesor = VCFramework.NegocioMySql.Profesor.ObtenerPorId(int.Parse(id));

                List<VCFramework.Entidad.Comuna> comunasDisponibles = new List<VCFramework.Entidad.Comuna>();
                List<VCFramework.Entidad.Comuna> comunasProfesor = new List<VCFramework.Entidad.Comuna>();

                VCFramework.Entidad.ComunaEnvoltorio devolver = new VCFramework.Entidad.ComunaEnvoltorio();

                //ahora procesamos
                List<int> comunasIdProfesor = new List<int>();
                string[] comunasIdsStr = profesor.ComIdAsignada.Split(',');
                if (comunasIdsStr != null && comunasIdsStr.Length > 0)
                {
                    if (comunasIdsStr[0] == "*")
                    {
                        comunasProfesor = comunas;
                    }
                    else
                    {
                        foreach (string s in comunasIdsStr)
                        {
                            if (s != "")
                            {
                                comunasIdProfesor.Add(int.Parse(s));
                            }
                        }
                    }
                }
                //ahora que tenemos la lista de comunas del profesor debemos quitarlas de la lista total de comunas
                if (comunas != null && comunas.Count > 0)
                {
                    //antes evaluamos si ya vienen comunas del profesor, en ese caso devolvemos solo estas
                    if (comunasProfesor.Count > 0)
                    {
                        comunasDisponibles = new List<VCFramework.Entidad.Comuna>();
                    }
                    else
                    {

                        foreach (VCFramework.Entidad.Comuna com in comunas)
                        {
                            if (comunasIdProfesor.Exists(p => p == com.Id))
                            {
                                //si existe en la lista hay que agregarla a las comunas del profesor
                                comunasProfesor.Add(com);
                            }
                            else
                            {
                                //si no existe hay que agregarlas a las comunas disponibles
                                comunasDisponibles.Add(com);
                            }
                        }
                    }
                }
                //creamos el nuevo objeto
                devolver.ComunasDisponibles = comunasDisponibles.OrderBy(p=>p.Nombre).ToList();
                devolver.ComunasProfesor = comunasProfesor.OrderBy(p => p.Nombre).ToList();

                if (devolver != null)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(devolver);
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


            string idProfesor = string.IsNullOrEmpty(Convert.ToString(data.IdProfesor)) ? "0" : Convert.ToString(data.IdProfesor);
            string comunasId = string.IsNullOrEmpty(Convert.ToString(data.Comunas)) ? "" : Convert.ToString(data.Comunas);

            VCFramework.Entidad.Profesor profe = VCFramework.NegocioMySql.Profesor.ObtenerPorId(int.Parse(idProfesor));

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //verificamos si existe el elemento
                if (profe != null)
                {
                    //quitamos el primer elemento de la cadena que siempre sera una coma
                    string nuevaCadena = comunasId.Remove(0, 1).ToString();
                    //ahora tenemos un arreglo concreto
                    profe.ComIdAsignada = nuevaCadena;
                    int guardado = VCFramework.NegocioMySql.Profesor.Modificar(profe);
                    if (guardado >= 0)
                    {
                        //todo ok, ahora devolvemos lo que debemos retornar
                        List<VCFramework.Entidad.Comuna> comunas = VCFramework.NegocioMySql.Comuna.Listar();
                        VCFramework.Entidad.Profesor profesor = VCFramework.NegocioMySql.Profesor.ObtenerPorId(int.Parse(idProfesor));

                        List<VCFramework.Entidad.Comuna> comunasDisponibles = new List<VCFramework.Entidad.Comuna>();
                        List<VCFramework.Entidad.Comuna> comunasProfesor = new List<VCFramework.Entidad.Comuna>();

                        VCFramework.Entidad.ComunaEnvoltorio devolver = new VCFramework.Entidad.ComunaEnvoltorio();

                        //ahora procesamos
                        List<int> comunasIdProfesor = new List<int>();
                        string[] comunasIdsStr = profesor.ComIdAsignada.Split(',');
                        if (comunasIdsStr != null && comunasIdsStr.Length > 0)
                        {
                            if (comunasIdsStr[0] == "*")
                            {
                                comunasProfesor = comunas;
                            }
                            else
                            {
                                foreach (string s in comunasIdsStr)
                                {
                                    if (s != "")
                                    {
                                        comunasIdProfesor.Add(int.Parse(s));
                                    }
                                }
                            }
                        }

                        //creamos el nuevo objeto
                        devolver.ComunasDisponibles = comunasDisponibles.OrderBy(p => p.Nombre).ToList();
                        devolver.ComunasProfesor = comunasProfesor.OrderBy(p => p.Nombre).ToList();

                        if (devolver != null)
                        {
                            httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                            String JSON = JsonConvert.SerializeObject(devolver);
                            httpResponse.Content = new StringContent(JSON);
                            httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(VCFramework.NegocioMySql.Utiles.JSON_DOCTYPE);
                        }
                        else
                        {
                            httpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                httpResponse = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                throw ex;
            }

            return httpResponse;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public HttpResponseMessage Get([FromUri]string Parametro)
        {
            //Parametro: { "TokenAcceso":"2BFFDCEAECCF9BE7698E1BA7593EB28C"}

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                string idComuna = Parametro;
                SinAlitas.Admin.Entidad.Comuna comuna = SinAlitas.Admin.Negocio.Territorio.ObtenerComunanPorId(int.Parse(idComuna));

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(comuna);
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
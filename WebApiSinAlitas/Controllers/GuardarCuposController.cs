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
    public class GuardarCuposController : ApiController
    {
        [AcceptVerbs("OPTIONS")]
        public void Options()
        { }

        [System.Web.Http.AcceptVerbs("PUT")]
        public HttpResponseMessage Put(dynamic DynamicClass)
        {

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            //validaciones antes de ejecutar la llamada.
            if (data.ClieId == "")
                throw new ArgumentNullException("ClieId");
            if (data.PcoId == "")
                throw new ArgumentNullException("Pco Id");
            if (data.ProfId == "")
                throw new ArgumentNullException("Prof Id");
            if (data.ArrCupos == "")
                throw new ArgumentNullException("Arr Cupos");
            if (data.NombreCliente == "")
                throw new ArgumentNullException("NombreCliente");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string clieId = data.ClieId;
                string pcoId = data.PcoId;
                string profId = data.ProfId;
                string arrCupos = data.ArrCupos;
                List<int> listaCuposGuardar = new List<int>();
                string nombreCliente = data.NombreCliente;

                string[] arr = arrCupos.Split(',');

                if (arr.Length > 0)
                {
                    foreach(string s in arr)
                    {
                        if (s != "")
                        {
                            listaCuposGuardar.Add(int.Parse(s));
                        }
                    }
                }
                //lista de cupos que usaremos para guardar
                List<SinAlitas.Admin.Entidad.Cupo> lista = new List<SinAlitas.Admin.Entidad.Cupo>();
                if (listaCuposGuardar.Count > 0)
                {
                    foreach(int e in listaCuposGuardar)
                    {
                        SinAlitas.Admin.Entidad.Cupo cpo = SinAlitas.Admin.Negocio.Cupo.ObtenerCupoPorId(e);
                        if (cpo != null && cpo.Id > 0)
                        {
                            lista.Add(cpo);
                        }
                    }
                }
                int cantidadProcesados = 0;
                //ahora procesamos la lista de cupos
                if (lista != null && lista.Count > 0)
                {
                    //antes vamos a procesar eliminados

                    foreach(SinAlitas.Admin.Entidad.Cupo cp in lista)
                    {
                        //procesamiento de los duplicados
                        List<SinAlitas.Admin.Entidad.Cupo> listaEliminar =
                                SinAlitas.Admin.Negocio.Cupo.ListarCuposEliminar(int.Parse(profId), cp.SghId, SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(cp.FechaHoraInicio), cp.Id);

                        if (listaEliminar != null && listaEliminar.Count > 0)
                        {
                            foreach (SinAlitas.Admin.Entidad.Cupo cpEl in listaEliminar)
                            {
                                SinAlitas.Admin.Negocio.Cupo.Eliminar(cpEl);
                            }
                        }
                        //ahora procesamos 
                        cp.Descripcion = "Clase tomada por " + nombreCliente;
                        cp.EstadoCupo = 2;
                        cp.PcoId = int.Parse(pcoId);
                        cp.ProfId = int.Parse(profId);
                        cp.ClieId = int.Parse(clieId);
                        //guardar los cupos y modificar los demas elementos
                        //entre ellos la revalorización del servicio
                        if (SinAlitas.Admin.Negocio.Cupo.Actualizar(cp) > 0)
                        {
                            cantidadProcesados++;
                        }
                    }
                }


                if (cantidadProcesados > 0)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(lista);
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
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

        [System.Web.Http.AcceptVerbs("PUT")]
        public HttpResponseMessage Put(dynamic DynamicClass)
        {

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);


            string nodId = string.IsNullOrEmpty(Convert.ToString(data.NodId)) ? "1" : Convert.ToString(data.NodId);
            string fechaEntera = string.IsNullOrEmpty(Convert.ToString(data.FechaEntera)) ? "" : Convert.ToString(data.FechaEntera);
            string profId = string.IsNullOrEmpty(Convert.ToString(data.ProfId)) ? "0" : Convert.ToString(data.ProfId);
            string arregloSgh = string.IsNullOrEmpty(Convert.ToString(data.Segmentos)) ? "" : Convert.ToString(data.Segmentos);
            string arregloNo = string.IsNullOrEmpty(Convert.ToString(data.SegmentosNo)) ? "" : Convert.ToString(data.SegmentosNo);


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            int contadorEntrada = 0;
            int contadorSalida = 0;

            try
            {
                //verificamos si existe el elemento
                DateTime fechaCupo = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(int.Parse(fechaEntera));
                List<SinAlitas.Admin.Entidad.SegmentoHorario> listSegmentos = SinAlitas.Admin.Negocio.SegmentoHorario.ListarSegmentos(int.Parse(nodId));

                string[] segmentosGuardar = arregloSgh.Split(',');
                string[] segmentosGuardarNo = arregloNo.Split(',');
                if (segmentosGuardar.Length > 0)
                {
                    contadorEntrada = segmentosGuardar.Length;
                    foreach(string s in segmentosGuardar)
                    {
                        if (s != "")
                        {
                            int idSeg = int.Parse(s);
                            SinAlitas.Admin.Entidad.SegmentoHorario segmento = listSegmentos.Find(p => p.Id == idSeg);
                            if (segmento != null)
                            {
                                DateTime fechaHoraInicio = Convert.ToDateTime(fechaCupo.ToShortDateString() + " " + segmento.FechaHoraInicial.ToLongTimeString());
                                DateTime fechaHoraTermino = Convert.ToDateTime(fechaCupo.ToShortDateString() + " " + segmento.FechaHoraFinal.ToLongTimeString());
                                SinAlitas.Admin.Entidad.Cupo cupoExsitente = SinAlitas.Admin.Negocio.Cupo.ObtenerCupo(int.Parse(nodId), int.Parse(profId), idSeg, fechaHoraInicio);
                                //si el cupo existe lo actualizamos
                                if (cupoExsitente != null && cupoExsitente.Id > 0)
                                {
                                    if (cupoExsitente.ClieId == 0)
                                    {
                                        cupoExsitente.Activo = 1;
                                        cupoExsitente.ClieId = 0;
                                        cupoExsitente.Descripcion = string.Empty;
                                        cupoExsitente.Eliminado = 0;
                                        cupoExsitente.EstadoCupo = 1;
                                        cupoExsitente.FechaHoraInicio = fechaHoraInicio;
                                        cupoExsitente.FechaHoraRegistro = DateTime.Now;
                                        cupoExsitente.FechaHoraTermino = fechaHoraTermino;
                                        cupoExsitente.NodId = int.Parse(nodId);
                                        cupoExsitente.ProfId = int.Parse(profId);
                                        cupoExsitente.SghId = idSeg;

                                        int id = SinAlitas.Admin.Negocio.Cupo.Actualizar(cupoExsitente);
                                        contadorSalida++;
                                    }


                                }
                                else
                                {
                                    SinAlitas.Admin.Entidad.Cupo cupo = new SinAlitas.Admin.Entidad.Cupo();
                                    cupo.Activo = 1;
                                    cupo.ClieId = 0;
                                    cupo.Descripcion = string.Empty;
                                    cupo.Eliminado = 0;
                                    cupo.EstadoCupo = 1;
                                    cupo.FechaHoraInicio = fechaHoraInicio;
                                    cupo.FechaHoraRegistro = DateTime.Now;
                                    cupo.FechaHoraTermino = fechaHoraTermino;
                                    cupo.NodId = int.Parse(nodId);
                                    cupo.ProfId = int.Parse(profId);
                                    cupo.SghId = idSeg;

                                    int id = SinAlitas.Admin.Negocio.Cupo.Insetar(cupo);
                                    contadorSalida++;
                                }
                            }
                        }

                        
                    }
                }
                if (segmentosGuardarNo.Length > 0)
                {
                    contadorEntrada = segmentosGuardarNo.Length;
                    foreach (string s in segmentosGuardarNo)
                    {
                        if (s != "")
                        {
                            int idSeg = int.Parse(s);
                            SinAlitas.Admin.Entidad.SegmentoHorario segmento = listSegmentos.Find(p => p.Id == idSeg);
                            if (segmento != null)
                            {
                                DateTime fechaHoraInicio = Convert.ToDateTime(fechaCupo.ToShortDateString() + " " + segmento.FechaHoraInicial.ToLongTimeString());
                                DateTime fechaHoraTermino = Convert.ToDateTime(fechaCupo.ToShortDateString() + " " + segmento.FechaHoraFinal.ToLongTimeString());
                                SinAlitas.Admin.Entidad.Cupo cupoExsitente = SinAlitas.Admin.Negocio.Cupo.ObtenerCupo(int.Parse(nodId), int.Parse(profId), idSeg, fechaHoraInicio);
                                //si el cupo existe lo eliminamos
                                if (cupoExsitente != null && cupoExsitente.Id > 0)
                                {
                                    if (cupoExsitente.ClieId == 0)
                                    {
                                        cupoExsitente.Activo = 1;
                                        cupoExsitente.ClieId = 0;
                                        cupoExsitente.Descripcion = string.Empty;
                                        cupoExsitente.Eliminado = 0;
                                        cupoExsitente.EstadoCupo = 1;
                                        cupoExsitente.FechaHoraInicio = fechaHoraInicio;
                                        cupoExsitente.FechaHoraRegistro = DateTime.Now;
                                        cupoExsitente.FechaHoraTermino = fechaHoraTermino;
                                        cupoExsitente.NodId = int.Parse(nodId);
                                        cupoExsitente.ProfId = int.Parse(profId);
                                        cupoExsitente.SghId = idSeg;

                                        int id = SinAlitas.Admin.Negocio.Cupo.Eliminar(cupoExsitente);
                                        contadorSalida++;
                                    }


                                }
                            }
                        }


                    }
                }
                List<SinAlitas.Admin.Entidad.SegmentoMostrar> devolver = new List<SinAlitas.Admin.Entidad.SegmentoMostrar>();

                //todo correcto
                string mesInicio = fechaCupo.Month.ToString();
                string anno = fechaCupo.Year.ToString();
                SinAlitas.Admin.Entidad.SegmentoMostrarFecha seg = SinAlitas.Admin.Negocio.SegmentoHorario.ContruirSegmentosFechaNuevo(int.Parse(nodId), int.Parse(mesInicio), int.Parse(profId), int.Parse(anno));


                devolver = seg.Dias;


                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(devolver);
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
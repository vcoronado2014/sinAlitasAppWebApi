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
    public class ProfesoresController : ApiController
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


                List<VCFramework.Entidad.Profesor> profesores = VCFramework.NegocioMySql.Profesor.Listar();
                profesores = profesores.OrderBy(p => p.Nombres).ToList();

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(profesores);
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


            string idProfesor = string.IsNullOrEmpty(Convert.ToString(data.IdProfesor)) ? "0" : Convert.ToString(data.IdProfesor);
            string Rut = string.IsNullOrEmpty(Convert.ToString(data.Rut)) ? "" : Convert.ToString(data.Rut);
            string nombres = string.IsNullOrEmpty(Convert.ToString(data.Nombres)) ? "" : Convert.ToString(data.Nombres);
            string primerApellido = string.IsNullOrEmpty(Convert.ToString(data.PrimerApellido)) ? "" : Convert.ToString(data.PrimerApellido);
            string segundoApellido = string.IsNullOrEmpty(Convert.ToString(data.SegundoApellido)) ? "" : Convert.ToString(data.SegundoApellido);
            string telefonos = string.IsNullOrEmpty(Convert.ToString(data.Telefonos)) ? "" : Convert.ToString(data.Telefonos);
            string sexo = string.IsNullOrEmpty(Convert.ToString(data.Sexo)) ? "" : Convert.ToString(data.Sexo);
            string correo = string.IsNullOrEmpty(Convert.ToString(data.Correo)) ? "" : Convert.ToString(data.Correo);
            string activo = string.IsNullOrEmpty(Convert.ToString(data.Activo)) ? "0" : Convert.ToString(data.Activo);
            string eliminado = string.IsNullOrEmpty(Convert.ToString(data.Eliminado)) ? "0" : Convert.ToString(data.Eliminado);

            int nuevoIdProfesor = 0;
            bool existeProfesor = false;

            VCFramework.Entidad.Profesor profe = new VCFramework.Entidad.Profesor();

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //verificamos si existe el elemento
                if (idProfesor != "0")
                {
                    //si existe entonces hay que traerlo ya que esta editando
                    profe = VCFramework.NegocioMySql.Profesor.ObtenerPorId(int.Parse(idProfesor));
                    existeProfesor = true;

                }
                profe.Activo = int.Parse(activo);
                //para eliminar al profe
                profe.Eliminado = int.Parse(eliminado);
                profe.Email = correo;
                profe.NodId = 1;
                profe.Nombres = nombres;
                profe.Rut = Rut;
                profe.NombreUsuario = Rut;
                profe.PrimerApellido = primerApellido;
                profe.SegundoApellido = segundoApellido;
                profe.Sexo = sexo;
                profe.TelefonosContacto = telefonos;
                profe.RolId = 3;


                //es nuevo o antiguo
                if (existeProfesor)
                {
                    if (profe.Eliminado == 1)
                    {
                        VCFramework.NegocioMySql.Profesor.Eliminar(profe);
                    }
                    else
                    {
                        VCFramework.NegocioMySql.Profesor.Modificar(profe);
                    }
                }
                else
                {
                    //AHORA LO BUSCAMOS POR RUN
                    profe = VCFramework.NegocioMySql.Profesor.Listar().Find(p=>p.Rut == profe.Rut);
                    if (profe.Id > 0)
                    {
                        //existe por run
                        //NO HAY QUE HACER NADA
                    }
                    else
                    {
                        //definitivamente no esta
                        profe.Codigo = "";
                        profe.ComId = 13;
                        profe.RegId = 13;
                        profe.Direccion = "cordon roma 0621";
                        profe.Fotografia = "";
                        profe.Password = profe.Rut;
                        profe.Eliminado = 0;
                        profe.Activo = 1;
                        profe.PaiId = 1;

                        nuevoIdProfesor = VCFramework.NegocioMySql.Profesor.Insertar(profe);
                    }
 

                }

                //fichas = VCFramework.NegocioMySql.EnvoltorioFicha.ListarPorIdPack(int.Parse(idPack));
                List<VCFramework.Entidad.Profesor> profesores = VCFramework.NegocioMySql.Profesor.Listar();
                profesores = profesores.OrderBy(p => p.Nombres).ToList();

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(profesores);
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
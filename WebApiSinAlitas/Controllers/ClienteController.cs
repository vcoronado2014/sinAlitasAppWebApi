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
    public class ClienteController : ApiController
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
            if (data.Id == "")
                throw new ArgumentNullException("Id");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string id = data.Id;


                VCFramework.Entidad.Cliente cliente = VCFramework.NegocioMySql.Cliente.ObtenerPorId(int.Parse(id));


                if (cliente != null && cliente.Id > 0)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(cliente);
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
            string regId = string.IsNullOrEmpty(Convert.ToString(data.RegId)) ? "0" : Convert.ToString(data.RegId);
            string comId = string.IsNullOrEmpty(Convert.ToString(data.ComId)) ? "0" : Convert.ToString(data.ComId);
            string nombres = string.IsNullOrEmpty(Convert.ToString(data.Nombres)) ? "" : Convert.ToString(data.Nombres);
            string primerApellido = string.IsNullOrEmpty(Convert.ToString(data.PrimerApellido)) ? "" : Convert.ToString(data.PrimerApellido);
            string segundoApellido = string.IsNullOrEmpty(Convert.ToString(data.SegundoApellido)) ? "" : Convert.ToString(data.SegundoApellido);
            string telefonos = string.IsNullOrEmpty(Convert.ToString(data.Telefonos)) ? "" : Convert.ToString(data.Telefonos);
            string sexo = string.IsNullOrEmpty(Convert.ToString(data.Sexo)) ? "" : Convert.ToString(data.Sexo);
            string correo = string.IsNullOrEmpty(Convert.ToString(data.Correo)) ? "" : Convert.ToString(data.Correo);
            string activo = string.IsNullOrEmpty(Convert.ToString(data.Activo)) ? "0" : Convert.ToString(data.Activo);
            string direccion = string.IsNullOrEmpty(Convert.ToString(data.Direccion)) ? "" : Convert.ToString(data.Direccion);

            int nuevoIdCliente = 0;
            bool existeCliente = false;

            VCFramework.Entidad.Cliente cliente = new VCFramework.Entidad.Cliente();

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //verificamos si existe el elemento
                if (idCliente != "0")
                {
                    //si existe entonces hay que traerlo ya que esta editando
                    cliente = VCFramework.NegocioMySql.Cliente.ObtenerPorId(int.Parse(idCliente));
                    existeCliente = true;

                }
                cliente.Activo = int.Parse(activo);
                if (cliente.Activo == 0)
                {
                    cliente.Eliminado = 1;

                }
                cliente.PaiId = 1;

                cliente.Email = correo;
                cliente.NodId = 1;

                cliente.Nombres = nombres;
                cliente.Rut = "";
                cliente.NombreUsuario = nombres;
                cliente.PrimerApellido = primerApellido;
                cliente.SegundoApellido = segundoApellido;
                cliente.Sexo = "No informado";
                cliente.TelefonosContacto = telefonos;
                cliente.RegId = int.Parse(regId);
                cliente.ComId = int.Parse(comId);
                cliente.Direccion = direccion;
                cliente.Password = "123456";


                //es nuevo o antiguo
                if (existeCliente)
                {

                    VCFramework.NegocioMySql.Cliente.Modificar(cliente);
                }
                else
                {
                    cliente.Codigo = "";
                    cliente.Eliminado = 0;
                    cliente.Activo = 1;

                    nuevoIdCliente = VCFramework.NegocioMySql.Cliente.Insertar(cliente);

                }

                //fichas = VCFramework.NegocioMySql.EnvoltorioFicha.ListarPorIdPack(int.Parse(idPack));
                List<VCFramework.Entidad.Cliente> clientes = VCFramework.NegocioMySql.Cliente.Listar();
                clientes = clientes.OrderBy(p => p.Nombres).ToList();

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                String JSON = JsonConvert.SerializeObject(clientes);
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;

namespace WebApiSinAlitas.Controllers
{
    public class AgendaExcepcionFilter : ExceptionFilterAttribute
    {
        private const string NULL_PARAMETERS = "Parámetros invalidos";
        private const string NULL_RESPONSE = "Solicitud no encontrada para los parámetros entregados:";
        private const string INVALID_USERNAME_PASS = "Combinación de Usuario/Clave inválida";
        private const string INVALID_USER_DOMAIN = "El usuario no tiene permiso para entrar a este dominio";
        public override void OnException(HttpActionExecutedContext context)
        {
            ErrorMessageObject errorMessage = new ErrorMessageObject();
            if (context.Exception is ArgumentNullException)
            {
                ArgumentNullException e = context.Exception as ArgumentNullException;
                errorMessage.Message = NULL_PARAMETERS;
                errorMessage.InternalMessage = string.Format("{0}. Parametro: {1}", NULL_RESPONSE, e.ParamName);
                errorMessage.ErrorCode = (int)HttpStatusCode.BadRequest;
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else if (context.Exception is System.Web.Http.HttpResponseException)
            {
                System.Web.Http.HttpResponseException webException = context.Exception as System.Web.Http.HttpResponseException;

                //string t = await (webException.Response.Content.ReadAsStringAsync());
                errorMessage.Message = "Problemas con el request";
                errorMessage.InternalMessage = webException.Response.ReasonPhrase;
                context.Response = new HttpResponseMessage(webException.Response.StatusCode);
                errorMessage.ErrorCode = (int)webException.Response.StatusCode;
                //errorMessage.InternalMessage = await webException.Response.Content.ReadAsStringAsync() ;
            }
            else
            {
                errorMessage.Message = "Error en la solicitud. Si el problema persiste, comuniquese con el administador.";
                errorMessage.InternalMessage = string.Format("Excepción no controlada de tipo {0}. Mensaje: {1}", context.Exception.GetType().ToString(), context.Exception.Message);
            }

            if (errorMessage.ErrorCode == 0)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errorMessage.ErrorCode = (int)HttpStatusCode.InternalServerError;
            }
            context.Response.Content = new ObjectContent<ErrorMessageObject>(errorMessage, new JsonMediaTypeFormatter(), "application/json");

            base.OnException(context);
        }
    }

    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorMessageObject
    {

        private string _message;
        private string _internalMessage;
        private int _errorCode;

        [JsonProperty("Message")]
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        [JsonProperty("InternalMessage")]
        public String InternalMessage
        {
            get
            {
                return _internalMessage;
            }
            set
            {
                _internalMessage = value;
            }
        }

        [JsonProperty("ErrorCode")]
        public int ErrorCode
        {
            get
            {
                return _errorCode;
            }
            set
            {
                _errorCode = value;
            }
        }
    }
}
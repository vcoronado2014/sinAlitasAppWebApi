using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace WebApiSinAlitas
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            #region  Producto
            config.Routes.MapHttpRoute(
                name: "Producto",
                routeTemplate: "api/Producto",
                defaults: new
                {
                    controller = "Producto"
                }
            );
            #endregion

            #region  ProductoCodigo
            config.Routes.MapHttpRoute(
                name: "ProductoCodigo",
                routeTemplate: "api/ProductoCodigo",
                defaults: new
                {
                    controller = "ProductoCodigo"
                }
            );
            #endregion

            #region  Cliente
            config.Routes.MapHttpRoute(
                name: "Cliente",
                routeTemplate: "api/Cliente",
                defaults: new
                {
                    controller = "Cliente"
                }
            );
            #endregion

            #region  Envoltorio
            config.Routes.MapHttpRoute(
                name: "Envoltorio",
                routeTemplate: "api/Envoltorio",
                defaults: new
                {
                    controller = "Envoltorio"
                }
            );
            #endregion

            #region  AceptaCondiciones
            config.Routes.MapHttpRoute(
                name: "AceptaCondiciones",
                routeTemplate: "api/AceptaCondiciones",
                defaults: new
                {
                    controller = "AceptaCondiciones"
                }
            );
            #endregion

            #region  Cupo
            config.Routes.MapHttpRoute(
                name: "Cupo",
                routeTemplate: "api/Cupo",
                defaults: new
                {
                    controller = "Cupo"
                }
            );
            #endregion

            #region  Profesor
            config.Routes.MapHttpRoute(
                name: "Profesor",
                routeTemplate: "api/Profesor",
                defaults: new
                {
                    controller = "Profesor"
                }
            );
            #endregion

            #region  FichaAlumno
            config.Routes.MapHttpRoute(
                name: "FichaAlumno",
                routeTemplate: "api/FichaAlumno",
                defaults: new
                {
                    controller = "FichaAlumno"
                }
            );
            #endregion

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

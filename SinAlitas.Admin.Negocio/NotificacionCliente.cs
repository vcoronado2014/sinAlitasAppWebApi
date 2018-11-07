using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SinAlitas.Admin;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class NotificacionCliente
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];
        public static int Insetar(SinAlitas.Admin.Entidad.NotificacionCliente notificacion)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();

            int id = fac.Insertar<SinAlitas.Admin.Entidad.NotificacionCliente>(notificacion, setCnsWebLun);
            return id;
        }
        public static int Actualizar(SinAlitas.Admin.Entidad.NotificacionCliente notificacion)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();

            int id = fac.Update<SinAlitas.Admin.Entidad.NotificacionCliente>(notificacion, setCnsWebLun);
            return id;
        }
        public static SinAlitas.Admin.Entidad.NotificacionCliente Obtener(string nombreUsuario)
        {
            SinAlitas.Admin.Entidad.NotificacionCliente entidad = new Entidad.NotificacionCliente();
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "NOMBRE_USUARIO";
            filtro.Valor = nombreUsuario;
            filtro.TipoDato = TipoDatoGeneral.Varchar;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.NotificacionCliente>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.NotificacionCliente> lista2 = new List<Entidad.NotificacionCliente>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.NotificacionCliente>().ToList();
            }
            if (lista2 != null && lista.Count == 1)
                entidad = lista2[0];

            return entidad;
        }
    }
}

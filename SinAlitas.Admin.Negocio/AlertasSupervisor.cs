using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Negocio
{
    public class AlertasSupervisor
    {
        public static List<Entidad.AlertasSupervisor> InformarAlertas()
        {
            List<Entidad.AlertasSupervisor> lista = new List<Entidad.AlertasSupervisor>();

            //una de las alertas es cuando un cliente tiene clases agendandas pero no la totalidad que le fueron definidas
            //traemos los prodctos codigo, todos y los evaluamos
            List<Entidad.ProductoCodigo> productosCodigo = Negocio.ProductoCodigo.ObtenerProductosCodigo();
            if (productosCodigo != null && productosCodigo.Count > 0)
            {
                foreach (Entidad.ProductoCodigo pco in productosCodigo)
                {
                    Entidad.EnvoltorioClientePackProducto env = Negocio.EnvoltorioClientePackProducto.Obtener(pco.Id);
                    if (env != null && env.CuposTomados != null)
                    {
                        if (env.CuposTomados != null && env.ProductoCodigo != null)
                        {
                            //ahora verificamos que packs tienen menos clases agendadas de las contratadas
                            if (env.CuposTomados.Count() < env.ProductoCodigo.CantidadClases)
                            {
                                //hay menos cupos tomados que los contratados
                                StringBuilder sbDetalle = new StringBuilder();
                                Entidad.AlertasSupervisor alerta = new Entidad.AlertasSupervisor();
                                sbDetalle.AppendFormat("El cliente tiene {0} clases agendadas de un total de {1} clases contratadas. Contacte al Cliente en el Email {2} o al fono {3}.", env.CuposTomados.Count().ToString(), env.ProductoCodigo.CantidadClases.ToString(), env.Cliente.Email, env.Cliente.TelefonosContacto);
                                alerta.Detalle = sbDetalle.ToString();
                                alerta.Nombre = env.ProductoCodigo.CodigoCliente + " " + env.Cliente.Nombres + " " + env.Cliente.PrimerApellido;
                                alerta.Fecha = env.ProductoCodigo.FechaCreacion.ToShortDateString();
                                alerta.Prioridad = "Media";
                                alerta.Inicial = "M";

                                lista.Add(alerta);
                            }
                            if (env.CuposTomados.Count() > 0)
                            {
                                //ahora alertaremos de aquellos clientes que tienen mas de 4 cupos en estado 3 cerrados.
                                int cantidadCerrados = env.CuposTomados.FindAll(p => p.EstadoCupo == 3).Count();
                                bool estaPagado = false;
                                if (env.ProductoCodigo.EstaPagado == 1)
                                    estaPagado = true;
                                if (cantidadCerrados >= 4 && estaPagado == false)
                                {
                                    StringBuilder sbDetalle = new StringBuilder();
                                    Entidad.AlertasSupervisor alerta = new Entidad.AlertasSupervisor();
                                    sbDetalle.AppendFormat("El cliente tiene {0} clases cerradas, verifique el pago del pack con el Cliente.", cantidadCerrados.ToString());
                                    alerta.Detalle = sbDetalle.ToString();
                                    alerta.Nombre = env.ProductoCodigo.CodigoCliente + " " + env.Cliente.Nombres + " " + env.Cliente.PrimerApellido;
                                    alerta.Fecha = env.ProductoCodigo.FechaCreacion.ToShortDateString();
                                    alerta.Prioridad = "Alta";
                                    alerta.Inicial = "A";
                                    lista.Add(alerta);
                                }
                            }
                        }
                    }
                }
            }
            return lista;
        }
    }
}

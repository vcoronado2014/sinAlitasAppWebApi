using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class Cliente
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];


        public static SinAlitas.Admin.Entidad.Cliente ObtenerClientePorId(int id)
        {

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "ID";
            filtro2.Valor = id.ToString();
            filtro2.TipoDato = TipoDatoGeneral.Entero;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);

            SinAlitas.Admin.Entidad.Cliente retorno = new Entidad.Cliente();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cliente>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cliente> lista2 = new List<Entidad.Cliente>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cliente>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static Entidad.ClienteFormateado RetornaClienteFormateado(int id)
        {
            Entidad.ClienteFormateado retorno = new Entidad.ClienteFormateado();

            Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(id);
            if (cliente != null && cliente.Id > 0)
            {
                retorno.Celular = cliente.TelefonosContacto;
                retorno.Comuna = Territorio.ObtenerComunanPorId(cliente.ComId).Nombre;
                retorno.Correo = cliente.Email;
                retorno.Id = cliente.Id;
                retorno.NombreFormateado = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                retorno.Region = Territorio.ObtenerRegionPorId(cliente.RegId).Nombre;
                retorno.Direccion = cliente.Direccion;

            }

            return retorno;
        }

        public static List<SinAlitas.Admin.Entidad.Cliente> ObtenerClientes()
        {

            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);

            SinAlitas.Admin.Entidad.Cliente retorno = new Entidad.Cliente();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cliente>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cliente> lista2 = new List<Entidad.Cliente>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cliente>().ToList();
            }


            return lista2;
        }

        public static List<SinAlitas.Admin.Entidad.Cliente> ObtenerClientesPorNombreYApellido(string nombre, string apellidoPaterno)
        {
            List<SinAlitas.Admin.Entidad.Cliente> lista = new List<Entidad.Cliente>();
            if (nombre != null && apellidoPaterno != null)
            {
                lista = ObtenerClientes();

                if (lista != null && lista.Count > 0)
                {
                    lista = lista.FindAll(p => p.Nombres.ToUpper().Contains(nombre.ToUpper()) || p.PrimerApellido.ToUpper().Contains(apellidoPaterno.ToUpper()));
                }
            }

            return lista;
        }
        public static List<SinAlitas.Admin.Entidad.ClienteBusqueda> ObtenerClientesPorNombreYApellidoLiviano(string nombre, string apellidoPaterno)
        {
            List<Entidad.ClienteBusqueda> listaDevolver = new List<Entidad.ClienteBusqueda>();
            List<SinAlitas.Admin.Entidad.Cliente> lista = new List<Entidad.Cliente>();
            if (nombre != null && apellidoPaterno != null)
            {
                lista = ObtenerClientes();

                if (lista != null && lista.Count > 0)
                {
                    lista = lista.FindAll(p => p.Nombres.ToUpper().Contains(nombre.ToUpper()) || p.PrimerApellido.ToUpper().Contains(apellidoPaterno.ToUpper()));
                }

                if (lista != null && lista.Count > 0)
                {
                    foreach(SinAlitas.Admin.Entidad.Cliente clie in lista)
                    {
                        Entidad.ClienteBusqueda cliente = new Entidad.ClienteBusqueda();
                        Entidad.Comuna com = Negocio.Territorio.ObtenerComunanPorId(clie.ComId);
                        cliente.Comuna = com.Nombre;
                        cliente.Id = clie.Id;
                        cliente.NombreFormateado = clie.Nombres + " " + clie.PrimerApellido + " " + clie.SegundoApellido;
                        listaDevolver.Add(cliente);
                    }
                }
            }

            return listaDevolver;
        }
        public static SinAlitas.Admin.Entidad.Cliente ObtenerClientePorUsuarioPassword(string usuario, string password)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOMBRE_USUARIO";
            filtro2.Valor = usuario;
            filtro2.TipoDato = TipoDatoGeneral.Varchar;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PASSWORD";
            filtro3.Valor = password;
            filtro3.TipoDato = TipoDatoGeneral.Varchar;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            SinAlitas.Admin.Entidad.Cliente retorno = new Entidad.Cliente();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cliente>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cliente> lista2 = new List<Entidad.Cliente>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cliente>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }

        public static Entidad.EnvoltorioCliente ObtenerEnvoltorioClientePorUsuarioPassword(string usuario, string password)
        {
            Entidad.EnvoltorioCliente cliente = new Entidad.EnvoltorioCliente();

            cliente.Cliente = ObtenerClientePorUsuarioPassword(usuario, password);

            if (cliente.Cliente != null && cliente.Cliente.Id > 0)
            {
                cliente.Pais = new Entidad.Pais();
                cliente.Pais = Negocio.Territorio.ObtenerPaisPorId(cliente.Cliente.PaiId);

                cliente.Region = new Entidad.Region();
                cliente.Region = Negocio.Territorio.ObtenerRegionPorId(cliente.Cliente.RegId);

                cliente.Comuna = new Entidad.Comuna();
                cliente.Comuna = Negocio.Territorio.ObtenerComunanPorId(cliente.Cliente.ComId);

                cliente.Nodo = new Entidad.Nodo();
                cliente.Nodo = Negocio.Territorio.ObtenerNodoPorId(cliente.Cliente.NodId);

            }

            return cliente;
        }
        public static int Insetar(SinAlitas.Admin.Entidad.Cliente cliente)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            int idCliente = fac.Insertar<SinAlitas.Admin.Entidad.Cliente>(cliente, setCnsWebLun);
            return idCliente;
        }
        public static SinAlitas.Admin.Entidad.Cliente ObtenerClientePorNombres(string nombres, string apellidoPaterno)
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro1 = new FiltroGenerico();
            filtro1.Campo = "ACTIVO";
            filtro1.Valor = "1";
            filtro1.TipoDato = TipoDatoGeneral.Entero;

            FiltroGenerico filtro2 = new FiltroGenerico();
            filtro2.Campo = "NOMBRES";
            filtro2.Valor = nombres;
            filtro2.TipoDato = TipoDatoGeneral.Varchar;

            FiltroGenerico filtro3 = new FiltroGenerico();
            filtro3.Campo = "PRIMER_APELLIDO";
            filtro3.Valor = apellidoPaterno;
            filtro3.TipoDato = TipoDatoGeneral.Varchar;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);
            filtros.Add(filtro3);

            SinAlitas.Admin.Entidad.Cliente retorno = new Entidad.Cliente();

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.Cliente>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.Cliente> lista2 = new List<Entidad.Cliente>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.Cliente>().ToList();
            }
            if (lista2 != null && lista2.Count == 1)
                retorno = lista2[0];

            return retorno;
        }
    }
}

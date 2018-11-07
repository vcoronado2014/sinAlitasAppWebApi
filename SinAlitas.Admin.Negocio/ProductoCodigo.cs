using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCFramework.Negocio.Factory;

namespace SinAlitas.Admin.Negocio
{
    public class ProductoCodigo
    {
        public static System.Configuration.ConnectionStringSettings setCnsWebLun = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MSUsuarioLunConectionString"];

        public static List<SinAlitas.Admin.Entidad.ProductoCodigo> ObtenerProductosCodigoPorId(int id)
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

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.ProductoCodigo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.ProductoCodigo> lista2 = new List<Entidad.ProductoCodigo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.ProductoCodigo>().ToList();
            }

            return lista2;
        }
        public static List<SinAlitas.Admin.Entidad.ProductoCodigo> ObtenerProductosCodigo(string codigoPack)
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
            filtro2.Campo = "CODIGO_CLIENTE";
            filtro2.Valor = codigoPack;
            filtro2.TipoDato = TipoDatoGeneral.Varchar;

            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);
            filtros.Add(filtro1);
            filtros.Add(filtro2);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.ProductoCodigo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.ProductoCodigo> lista2 = new List<Entidad.ProductoCodigo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.ProductoCodigo>().ToList();
            }

            return lista2;
        }
        public static List<SinAlitas.Admin.Entidad.ProductoCodigo> ObtenerProductosCodigo()
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

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.ProductoCodigo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.ProductoCodigo> lista2 = new List<Entidad.ProductoCodigo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.ProductoCodigo>().ToList();
            }

            return lista2;
        }
        public static List<SinAlitas.Admin.Entidad.ProductoCodigo> ObtenerProductosCodigoSinFiltroActivo()
        {
            Factory fac = new Factory();
            FiltroGenerico filtro = new FiltroGenerico();
            filtro.Campo = "ELIMINADO";
            filtro.Valor = "0";
            filtro.TipoDato = TipoDatoGeneral.Entero;


            List<FiltroGenerico> filtros = new List<FiltroGenerico>();
            filtros.Add(filtro);

            List<object> lista = fac.Leer<SinAlitas.Admin.Entidad.ProductoCodigo>(filtros, setCnsWebLun);
            List<SinAlitas.Admin.Entidad.ProductoCodigo> lista2 = new List<Entidad.ProductoCodigo>();
            if (lista != null)
            {

                lista2 = lista.Cast<Entidad.ProductoCodigo>().ToList();
            }

            return lista2;
        }


        public static int Pagar(int Id, string Texto, bool EstaPagado)
        {
            int idCupo = 0;
            List<Entidad.ProductoCodigo> pro = Negocio.ProductoCodigo.ObtenerProductosCodigoPorId(Id);
            if (pro != null && pro.Count == 1)
            {
                if (EstaPagado)

                    pro[0].EstaPagado = 1;
                else
                    pro[0].EstaPagado = 0;
                VCFramework.Negocio.Factory.Factory fac = new Factory();
                idCupo = fac.Update<SinAlitas.Admin.Entidad.ProductoCodigo>(pro[0], setCnsWebLun);
            }
            return idCupo;

        }

        public static List<Entidad.ProductoCodigoTexto> ObtenerProductoCodigoGrilla(int estado)
        {
            List<Entidad.ProductoCodigoTexto> lista = new List<Entidad.ProductoCodigoTexto>();
            //List<Entidad.ProductoCodigo> listaProcesar = ObtenerProductosCodigoPorEstado(estado);
            List<Entidad.ProductoCodigo> listaProcesar = ObtenerProductosCodigo();
            if (listaProcesar != null && listaProcesar.Count > 0)
            {
                foreach (Entidad.ProductoCodigo pro in listaProcesar)
                {
                    bool agregar = false;
                    bool tieneAceptaCondiciones = false;
                    bool tieneFicha = false;
                    //vamos a determinar los estados para devolver la grilla
                    Entidad.AceptaCondiciones acpetaCondiciones = Negocio.AceptaCondiciones.ObtenerAceptaCondicionesPcoId(pro.ProId);
                    if (acpetaCondiciones != null && acpetaCondiciones.Id > 0)
                        tieneAceptaCondiciones = true;
                    List<Entidad.FichaPack> fichas = Negocio.FichaPack.ObtenerFichasPack(pro.ProId);
                    if (fichas != null && fichas.Count > 0)
                        tieneFicha = true;


                    string nombrePack = string.Empty;
                    string nombreCliente = string.Empty;
                    string region = string.Empty;
                    string comuna = string.Empty;
                    string cantidadAlumnos = string.Empty;
                    string cantidadClases = string.Empty;
                    string direccion = string.Empty;
                    string estadoT = string.Empty;
                    switch (pro.Estado)
                    {
                        case 0:
                            estadoT = "Todos";
                            break;
                        case 1:
                            estadoT = "Creado por El Supervisor";
                            break;
                        case 2:
                            estadoT = "Aceptado por el Cliente";
                            break;
                        case 3:
                            estadoT = "En Curso";
                            break;
                        case 4:
                            estadoT = "Terminado";
                            break;
                    }

                    Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(pro.ClieId);
                    if (cliente.Id > 0)
                    {
                        nombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                        region = Negocio.Territorio.ObtenerRegionPorId(cliente.RegId).Nombre;
                        comuna = Negocio.Territorio.ObtenerComunanPorId(cliente.ComId).Nombre;
                        direccion = cliente.Direccion;
                    }
                    nombrePack = pro.CodigoCliente;
                    cantidadAlumnos = pro.CantidadAlumnos.ToString();
                    cantidadClases = pro.CantidadClases.ToString();
                    //armamos al respuesta.
                    Entidad.ProductoCodigoTexto pr = new Entidad.ProductoCodigoTexto();
                    pr.EstaPagado = false;
                    string estaPagado = "NO PAGADO.";
                    if (pro.EstaPagado == 1)
                    {
                        pr.EstaPagado = true;
                        estaPagado = "PAGADO.";
                    }
                    pr.Id = pro.Id;
                    
                    pr.UrlAbrir = "SeleccionHorasCliente.aspx?PCO_ID=" + pro.Id.ToString();

                    switch (estado)
                    {
                        case 0:
                            agregar = true;
                            estadoT = "Todos";
                            break;
                        case 1:
                            agregar = true;
                            estadoT = "Creado por El Supervisor";
                            break;
                        case 2:
                            //si esta aceptada por el cliente entonces se agrega en este estado
                            if (tieneAceptaCondiciones)
                            {
                                agregar = true;
                                estadoT = "Aceptado por el Cliente";
                            }
                            break;
                        case 3:
                            //si tiene ficha esta en este estado
                            if (tieneFicha)
                            {
                                agregar = true;
                                estadoT = "En Curso";
                            }
                            break;
                    }
                    string textoAcepta = "";
                    string textoFicha = "";
                    if (tieneAceptaCondiciones == false)
                        textoAcepta = "No ha aceptado condiciones";
                    else
                        textoAcepta = "Ha aceptado condiciones";

                    if (tieneFicha == false)
                        textoFicha = "No ha creado las fichas de los alumnos";
                    else
                        textoFicha = "Tiene fichas de los alumnos creada";

                    pr.Texto = nombrePack + " ,creado el " + pro.FechaCreacion.ToShortDateString() + ", para el cliente " + nombreCliente + " que vive en " + direccion + ", " + comuna + ", " + region + " cuenta con " + cantidadClases + " clases y " + cantidadAlumnos + " alumnos programados, se encuentra en estado " + estadoT + ", el pack esta " + estaPagado + ", " + textoAcepta + ", " + textoFicha;
                    if (tieneFicha && tieneAceptaCondiciones)
                        pr.MostrarAbrir = true;
                    else
                        pr.MostrarAbrir = false;

                    if (agregar)
                        lista.Add(pr);
                }
            }
            return lista;
        }

        public static List<Entidad.ProductoCodigoTexto> ObtenerProductoCodigoGrilla(int estado, string codigo)
        {
            List<Entidad.ProductoCodigoTexto> lista = new List<Entidad.ProductoCodigoTexto>();
            //List<Entidad.ProductoCodigo> listaProcesar = ObtenerProductosCodigoPorEstado(estado);
            List<Entidad.ProductoCodigo> listaProcesar = ObtenerProductosCodigo();
            if (listaProcesar != null && listaProcesar.Count > 0)
            {
                if (codigo != null && codigo != "")
                {
                    listaProcesar = listaProcesar.FindAll(p => p.CodigoCliente.ToUpper() == codigo.ToUpper());
                    estado = 0;
                }

                foreach (Entidad.ProductoCodigo pro in listaProcesar)
                {
                    bool agregar = false;
                    bool tieneAceptaCondiciones = false;
                    bool tieneFicha = false;
                    //vamos a determinar los estados para devolver la grilla
                    Entidad.AceptaCondiciones acpetaCondiciones = Negocio.AceptaCondiciones.ObtenerAceptaCondicionesPcoId(pro.ProId);
                    if (acpetaCondiciones != null && acpetaCondiciones.Id > 0)
                        tieneAceptaCondiciones = true;
                    List<Entidad.FichaPack> fichas = Negocio.FichaPack.ObtenerFichasPack(pro.ProId);
                    if (fichas != null && fichas.Count > 0)
                        tieneFicha = true;


                    string nombrePack = string.Empty;
                    string nombreCliente = string.Empty;
                    string region = string.Empty;
                    string comuna = string.Empty;
                    string cantidadAlumnos = string.Empty;
                    string cantidadClases = string.Empty;
                    string direccion = string.Empty;
                    string estadoT = string.Empty;
                    switch (pro.Estado)
                    {
                        case 0:
                            estadoT = "Todos";
                            break;
                        case 1:
                            estadoT = "Creado por El Supervisor";
                            break;
                        case 2:
                            estadoT = "Aceptado por el Cliente";
                            break;
                        case 3:
                            estadoT = "En Curso";
                            break;
                        case 4:
                            estadoT = "Terminado";
                            break;
                    }

                    Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(pro.ClieId);
                    if (cliente.Id > 0)
                    {
                        nombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                        region = Negocio.Territorio.ObtenerRegionPorId(cliente.RegId).Nombre;
                        comuna = Negocio.Territorio.ObtenerComunanPorId(cliente.ComId).Nombre;
                        direccion = cliente.Direccion;
                    }
                    nombrePack = pro.CodigoCliente;
                    cantidadAlumnos = pro.CantidadAlumnos.ToString();
                    cantidadClases = pro.CantidadClases.ToString();
                    //armamos al respuesta.
                    Entidad.ProductoCodigoTexto pr = new Entidad.ProductoCodigoTexto();
                    pr.EstaPagado = false;
                    string estaPagado = "NO PAGADO.";
                    if (pro.EstaPagado == 1)
                    {
                        pr.EstaPagado = true;
                        estaPagado = "PAGADO.";
                    }
                    pr.Id = pro.Id;

                    pr.UrlAbrir = "SeleccionHorasCliente.aspx?PCO_ID=" + pro.Id.ToString();

                    switch (estado)
                    {
                        case 0:
                            agregar = true;
                            estadoT = "Todos";
                            break;
                        case 1:
                            agregar = true;
                            estadoT = "Creado por El Supervisor";
                            break;
                        case 2:
                            //si esta aceptada por el cliente entonces se agrega en este estado
                            if (tieneAceptaCondiciones)
                            {
                                agregar = true;
                                estadoT = "Aceptado por el Cliente";
                            }
                            break;
                        case 3:
                            //si tiene ficha esta en este estado
                            if (tieneFicha)
                            {
                                agregar = true;
                                estadoT = "En Curso";
                            }
                            break;
                    }
                    string textoAcepta = "";
                    string textoFicha = "";
                    if (tieneAceptaCondiciones == false)
                        textoAcepta = "No ha aceptado condiciones";
                    else
                        textoAcepta = "Ha aceptado condiciones";

                    if (tieneFicha == false)
                        textoFicha = "No ha creado las fichas de los alumnos";
                    else
                        textoFicha = "Tiene fichas de los alumnos creada";

                    pr.Texto = nombrePack + " ,creado el " + pro.FechaCreacion.ToShortDateString() + ", para el cliente " + nombreCliente + " que vive en " + direccion + ", " + comuna + ", " + region + " cuenta con " + cantidadClases + " clases y " + cantidadAlumnos + " alumnos programados, se encuentra en estado " + estadoT + ", el pack esta " + estaPagado + ", " + textoAcepta + ", " + textoFicha;
                    if (tieneFicha && tieneAceptaCondiciones)
                        pr.MostrarAbrir = true;
                    else
                        pr.MostrarAbrir = false;

                    if (agregar)
                        lista.Add(pr);
                }
            }
            return lista;
        }

        public static List<Entidad.ProductoCodigoTexto> ObtenerProductoCodigoGrillaCliente(string nombrePackP)
        {
            List<Entidad.ProductoCodigoTexto> lista = new List<Entidad.ProductoCodigoTexto>();

            try {
                string[] packs = nombrePackP.Split('-');
                int idPack = 0;
                if (packs != null && packs.Length > 1)
                    idPack = int.Parse(packs[0]);


                List<Entidad.ProductoCodigo> listaProcesar = ObtenerProductosCodigoPorEstado(0).FindAll(p => p.Id == idPack && p.CodigoCliente.ToUpper() == nombrePackP.ToUpper());
                if (listaProcesar != null && listaProcesar.Count > 0)
                {
                    foreach (Entidad.ProductoCodigo pro in listaProcesar)
                    {
                        string nombrePack = string.Empty;
                        string nombreCliente = string.Empty;
                        string region = string.Empty;
                        string comuna = string.Empty;
                        string cantidadAlumnos = string.Empty;
                        string cantidadClases = string.Empty;
                        string direccion = string.Empty;
                        string estadoT = string.Empty;
                        switch (pro.Estado)
                        {
                            case 0:
                                estadoT = "Todos";
                                break;
                            case 1:
                                estadoT = "Creado por El Supervisor";
                                break;
                            case 2:
                                estadoT = "Aceptado por el Cliente";
                                break;
                            case 3:
                                estadoT = "En Curso";
                                break;
                            case 4:
                                estadoT = "Terminado";
                                break;
                        }


                        Entidad.Cliente cliente = Negocio.Cliente.ObtenerClientePorId(pro.ClieId);
                        if (cliente.Id > 0)
                        {
                            nombreCliente = cliente.Nombres + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                            region = Negocio.Territorio.ObtenerRegionPorId(cliente.RegId).Nombre;
                            comuna = Negocio.Territorio.ObtenerComunanPorId(cliente.ComId).Nombre;
                            direccion = cliente.Direccion;
                        }
                        nombrePack = pro.CodigoCliente;
                        cantidadAlumnos = pro.CantidadAlumnos.ToString();
                        cantidadClases = pro.CantidadClases.ToString();
                        //armamos al respuesta.
                        Entidad.ProductoCodigoTexto pr = new Entidad.ProductoCodigoTexto();
                        pr.Id = pro.Id;
                        pr.Texto = nombrePack + " ,creado el " + pro.FechaCreacion.ToShortDateString() + ", para el cliente " + nombreCliente + " que vive en " + direccion + ", " + comuna + ", " + region + " cuenta con " + cantidadClases + " clases y " + cantidadAlumnos + " alumnos programados, se encuentra en estado " + estadoT;
                        lista.Add(pr);
                    }
                }
            }
            catch
            {
                lista = new List<Entidad.ProductoCodigoTexto>();
            }
            return lista;
        }
        public static List<SinAlitas.Admin.Entidad.ProductoCodigo> ObtenerProductosCodigoPorEstado(int estado)
        {
            List<Entidad.ProductoCodigo> listaDevolver = new List<Entidad.ProductoCodigo>();

            switch(estado)
            {
                case 0:
                    listaDevolver = ObtenerProductosCodigo();
                    break;
                default:
                    listaDevolver = ObtenerProductosCodigo().FindAll(p => p.Estado == estado);
                    break;

            }

            return listaDevolver;
        }
        public static int UltimoId()
        {
            int retorno = 1;
            List<Entidad.ProductoCodigo> lista = ObtenerProductosCodigoSinFiltroActivo();
            if (lista != null && lista.Count > 0)
                retorno = lista.Max(p => p.Id) + 1;
            return retorno;

        }
        public static int Insetar(SinAlitas.Admin.Entidad.ProductoCodigo productoCodigo)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            productoCodigo.FechaCreacion = DateTime.Now;
            int idCupo = fac.Insertar<SinAlitas.Admin.Entidad.ProductoCodigo>(productoCodigo, setCnsWebLun);
            return idCupo;
        }
        public static int Actualizar(SinAlitas.Admin.Entidad.ProductoCodigo productoCodigo)
        {
            VCFramework.Negocio.Factory.Factory fac = new Factory();
            productoCodigo.FechaCreacion = DateTime.Now;
            int idCupo = fac.Update<SinAlitas.Admin.Entidad.ProductoCodigo>(productoCodigo, setCnsWebLun);
            return idCupo;
        }
    }
}

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
    public class PlanillaProfesoresController : ApiController
    {
        /*
                 int hoy = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio);
        int diaDos = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(1));
        int diaTres = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(2));
        int diaCuatro = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(3));
        int diaCinco = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(4));
        int diaSeis = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(5));
        int diaSiete = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(6));
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosUno = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(hoy);
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosDos = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaDos);
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosTres = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaTres);
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosCuatro = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaCuatro);
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosCinco = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaCinco);
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosSeis = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaSeis);
        List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosSiete = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaSiete);


        //ahora que tenemos un listado de profesores con sus packs y segmentos armamos la nueva lista
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaUno = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaDos = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaTres = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaCuatro = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaCinco = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaSeis = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaSiete = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();

        List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaCompleta = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
        #region primer dia
        if (resultadosUno != null && resultadosUno.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosUno)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaUno.Add(planilla);
            }
        }
        #endregion
        #region segundo dia
        if (resultadosDos != null && resultadosDos.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosDos)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaDos.Add(planilla);
            }
        }
        #endregion
        #region tercer dia
        if (resultadosTres != null && resultadosTres.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosTres)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaTres.Add(planilla);
            }
        }
        #endregion
        #region cuarto dia
        if (resultadosCuatro != null && resultadosCuatro.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosCuatro)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaCuatro.Add(planilla);
            }
        }
        #endregion
        #region quinto dia
        if (resultadosCinco != null && resultadosCinco.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosCinco)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaCinco.Add(planilla);
            }
        }
        #endregion
        #region sexto dia
        if (resultadosSeis != null && resultadosSeis.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosSeis)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaSeis.Add(planilla);
            }
        }
        #endregion
        #region septimo dia
        if (resultadosSiete != null && resultadosSiete.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosSiete)
            {
                SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                planilla.IdProfesor = resul.Id;
                planilla.NombreProfesor = resul.Nombres;
                planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete);
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 09:00"))
                {
                    planilla.SegmentoUno = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 10:15"))
                {
                    planilla.SegmentoDos = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 11:30"))
                {
                    planilla.SegmentoTres = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 13:00"))
                {
                    planilla.SegmentoCuatro = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 14:15"))
                {
                    planilla.SegmentoCinco = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 15:30"))
                {
                    planilla.SegmentoSeis = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 16:45"))
                {
                    planilla.SegmentoSiete = resul.CodigoCliente;
                }
                if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 18:00"))
                {
                    planilla.SegmentoOcho = resul.CodigoCliente;
                }
                listaPlanillaSiete.Add(planilla);
            }
        }
        #endregion

        listaPlanillaCompleta.AddRange(listaPlanillaUno);
        listaPlanillaCompleta.AddRange(listaPlanillaDos);
        listaPlanillaCompleta.AddRange(listaPlanillaTres);
        listaPlanillaCompleta.AddRange(listaPlanillaCuatro);
        listaPlanillaCompleta.AddRange(listaPlanillaCinco);
        listaPlanillaCompleta.AddRange(listaPlanillaSeis);
        listaPlanillaCompleta.AddRange(listaPlanillaSiete);

        List<SinAlitas.Admin.Entidad.ItemProfesor> listaFinal = new List<SinAlitas.Admin.Entidad.ItemProfesor>();

        var agrupdas = listaPlanillaCompleta.GroupBy(p => p.NombreProfesor);
        if (agrupdas != null)
        {
            foreach (var group in agrupdas)
            {
                SinAlitas.Admin.Entidad.ItemProfesor plan = new SinAlitas.Admin.Entidad.ItemProfesor();
                plan.NombreProfesor = group.Key;
                plan.Detalle = new List<SinAlitas.Admin.Entidad.ItemDetalle>();
                //Console.WriteLine("Group {0}", group.Key);
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> items = group.ToList();

                var groupFechas = items.GroupBy(p => p.Fecha);

                foreach (var groupF in groupFechas)
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = groupF.Key;
                    List<SinAlitas.Admin.Entidad.PlanillaProfesor> fechitas = groupF.ToList();

                    if (fechitas != null && fechitas.Count > 0)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor plan1 = fechitas.Find(p => p.SegmentoUno != null);
                        detalle.SegmentoUno = plan1 == null ? "" : plan1.SegmentoUno;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan2 = fechitas.Find(p => p.SegmentoDos != null);
                        detalle.SegmentoDos = plan2 == null ? "" : plan2.SegmentoDos;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan3 = fechitas.Find(p => p.SegmentoTres != null);
                        detalle.SegmentoTres = plan3 == null ? "" : plan3.SegmentoTres;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan4 = fechitas.Find(p => p.SegmentoCuatro != null);
                        detalle.SegmentoCuatro = plan4 == null ? "" : plan4.SegmentoCuatro;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan5 = fechitas.Find(p => p.SegmentoCinco != null);
                        detalle.SegmentoCinco = plan5 == null ? "" : plan5.SegmentoCinco;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan6 = fechitas.Find(p => p.SegmentoSeis != null);
                        detalle.SegmentoSeis = plan6 == null ? "" : plan6.SegmentoSeis;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan7 = fechitas.Find(p => p.SegmentoSiete != null);
                        detalle.SegmentoSiete = plan7 == null ? "" : plan7.SegmentoSiete;

                        SinAlitas.Admin.Entidad.PlanillaProfesor plan8 = fechitas.Find(p => p.SegmentoOcho != null);
                        detalle.SegmentoOcho = plan8 == null ? "" : plan8.SegmentoOcho;

                    }

                    plan.Detalle.Add(detalle);
                }

                listaFinal.Add(plan);
            }
        }
        //Ahora procesamos la lista final
        if (listaFinal != null && listaFinal.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ItemProfesor prof in listaFinal)
            {
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy);
                    prof.Detalle.Add(detalle);
                }
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos);
                    prof.Detalle.Add(detalle);
                }
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres);
                    prof.Detalle.Add(detalle);
                }
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro);
                    prof.Detalle.Add(detalle);
                }
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco);
                    prof.Detalle.Add(detalle);
                }
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis);
                    prof.Detalle.Add(detalle);
                }
                if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete)))
                {
                    SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                    detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete);
                    prof.Detalle.Add(detalle);
                }
            }
        }
        if (listaFinal != null && listaFinal.Count > 0)
        {
            foreach (SinAlitas.Admin.Entidad.ItemProfesor prof in listaFinal)
            {
                if (prof.Detalle != null && prof.Detalle.Count > 0)
                {
                    prof.Detalle = prof.Detalle.OrderBy(p => p.Fecha).ToList();
                }
            }
        } 
        */

        [AcceptVerbs("OPTIONS")]
        public void Options()
        { }

        [System.Web.Http.AcceptVerbs("POST")]
        public HttpResponseMessage Post(dynamic DynamicClass)
        {

            string Input = JsonConvert.SerializeObject(DynamicClass);

            dynamic data = JObject.Parse(Input);

            //validaciones antes de ejecutar la llamada.
            if (data.ClieId == null)
                throw new ArgumentNullException("Id");


            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string clieId = data.ClieId;


                List<VCFramework.Entidad.FichaAlumno> alumnos = new List<VCFramework.Entidad.FichaAlumno>();
                DateTime fechaInicio = DateTime.Now;
                #region codigo
                int hoy = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio);
                int diaDos = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(1));
                int diaTres = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(2));
                int diaCuatro = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(3));
                int diaCinco = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(4));
                int diaSeis = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(5));
                int diaSiete = SinAlitas.Admin.Entidad.Utiles.RetornaFechaEntera(fechaInicio.AddDays(6));
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosUno = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(hoy);
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosDos = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaDos);
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosTres = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaTres);
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosCuatro = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaCuatro);
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosCinco = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaCinco);
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosSeis = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaSeis);
                List<SinAlitas.Admin.Entidad.ResultadosProfesor> resultadosSiete = SinAlitas.Admin.Negocio.PlanillaProfesor.ObtenerMatriz(diaSiete);


                //ahora que tenemos un listado de profesores con sus packs y segmentos armamos la nueva lista
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaUno = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaDos = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaTres = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaCuatro = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaCinco = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaSeis = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaSiete = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();

                List<SinAlitas.Admin.Entidad.PlanillaProfesor> listaPlanillaCompleta = new List<SinAlitas.Admin.Entidad.PlanillaProfesor>();
                #region primer dia
                if (resultadosUno != null && resultadosUno.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosUno)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaUno.Add(planilla);
                    }
                }
                #endregion
                #region segundo dia
                if (resultadosDos != null && resultadosDos.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosDos)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaDos.Add(planilla);
                    }
                }
                #endregion
                #region tercer dia
                if (resultadosTres != null && resultadosTres.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosTres)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaTres.Add(planilla);
                    }
                }
                #endregion
                #region cuarto dia
                if (resultadosCuatro != null && resultadosCuatro.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosCuatro)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaCuatro.Add(planilla);
                    }
                }
                #endregion
                #region quinto dia
                if (resultadosCinco != null && resultadosCinco.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosCinco)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaCinco.Add(planilla);
                    }
                }
                #endregion
                #region sexto dia
                if (resultadosSeis != null && resultadosSeis.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosSeis)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaSeis.Add(planilla);
                    }
                }
                #endregion
                #region septimo dia
                if (resultadosSiete != null && resultadosSiete.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ResultadosProfesor resul in resultadosSiete)
                    {
                        SinAlitas.Admin.Entidad.PlanillaProfesor planilla = new SinAlitas.Admin.Entidad.PlanillaProfesor();
                        planilla.IdProfesor = resul.Id;
                        planilla.NombreProfesor = resul.Nombres;
                        planilla.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete);
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 09:00"))
                        {
                            planilla.SegmentoUno = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 10:15"))
                        {
                            planilla.SegmentoDos = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 11:30"))
                        {
                            planilla.SegmentoTres = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 13:00"))
                        {
                            planilla.SegmentoCuatro = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 14:15"))
                        {
                            planilla.SegmentoCinco = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 15:30"))
                        {
                            planilla.SegmentoSeis = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 16:45"))
                        {
                            planilla.SegmentoSiete = resul.CodigoCliente;
                        }
                        if (resul.FechaHoraInicio == Convert.ToDateTime(SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete).ToShortDateString() + " 18:00"))
                        {
                            planilla.SegmentoOcho = resul.CodigoCliente;
                        }
                        listaPlanillaSiete.Add(planilla);
                    }
                }
                #endregion

                listaPlanillaCompleta.AddRange(listaPlanillaUno);
                listaPlanillaCompleta.AddRange(listaPlanillaDos);
                listaPlanillaCompleta.AddRange(listaPlanillaTres);
                listaPlanillaCompleta.AddRange(listaPlanillaCuatro);
                listaPlanillaCompleta.AddRange(listaPlanillaCinco);
                listaPlanillaCompleta.AddRange(listaPlanillaSeis);
                listaPlanillaCompleta.AddRange(listaPlanillaSiete);

                List<SinAlitas.Admin.Entidad.ItemProfesor> listaFinal = new List<SinAlitas.Admin.Entidad.ItemProfesor>();

                var agrupdas = listaPlanillaCompleta.GroupBy(p => p.NombreProfesor);
                if (agrupdas != null)
                {
                    foreach (var group in agrupdas)
                    {
                        SinAlitas.Admin.Entidad.ItemProfesor plan = new SinAlitas.Admin.Entidad.ItemProfesor();
                        plan.NombreProfesor = group.Key;
                        plan.Detalle = new List<SinAlitas.Admin.Entidad.ItemDetalle>();
                        //Console.WriteLine("Group {0}", group.Key);
                        List<SinAlitas.Admin.Entidad.PlanillaProfesor> items = group.ToList();

                        var groupFechas = items.GroupBy(p => p.Fecha);

                        foreach (var groupF in groupFechas)
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = groupF.Key;
                            List<SinAlitas.Admin.Entidad.PlanillaProfesor> fechitas = groupF.ToList();

                            if (fechitas != null && fechitas.Count > 0)
                            {
                                SinAlitas.Admin.Entidad.PlanillaProfesor plan1 = fechitas.Find(p => p.SegmentoUno != null);
                                detalle.SegmentoUno = plan1 == null ? "" : plan1.SegmentoUno;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan2 = fechitas.Find(p => p.SegmentoDos != null);
                                detalle.SegmentoDos = plan2 == null ? "" : plan2.SegmentoDos;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan3 = fechitas.Find(p => p.SegmentoTres != null);
                                detalle.SegmentoTres = plan3 == null ? "" : plan3.SegmentoTres;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan4 = fechitas.Find(p => p.SegmentoCuatro != null);
                                detalle.SegmentoCuatro = plan4 == null ? "" : plan4.SegmentoCuatro;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan5 = fechitas.Find(p => p.SegmentoCinco != null);
                                detalle.SegmentoCinco = plan5 == null ? "" : plan5.SegmentoCinco;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan6 = fechitas.Find(p => p.SegmentoSeis != null);
                                detalle.SegmentoSeis = plan6 == null ? "" : plan6.SegmentoSeis;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan7 = fechitas.Find(p => p.SegmentoSiete != null);
                                detalle.SegmentoSiete = plan7 == null ? "" : plan7.SegmentoSiete;

                                SinAlitas.Admin.Entidad.PlanillaProfesor plan8 = fechitas.Find(p => p.SegmentoOcho != null);
                                detalle.SegmentoOcho = plan8 == null ? "" : plan8.SegmentoOcho;

                            }

                            plan.Detalle.Add(detalle);
                        }

                        listaFinal.Add(plan);
                    }
                }
                //Ahora procesamos la lista final
                if (listaFinal != null && listaFinal.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ItemProfesor prof in listaFinal)
                    {
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(hoy);
                            prof.Detalle.Add(detalle);
                        }
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaDos);
                            prof.Detalle.Add(detalle);
                        }
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaTres);
                            prof.Detalle.Add(detalle);
                        }
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCuatro);
                            prof.Detalle.Add(detalle);
                        }
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaCinco);
                            prof.Detalle.Add(detalle);
                        }
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSeis);
                            prof.Detalle.Add(detalle);
                        }
                        if (!prof.Detalle.Exists(p => p.Fecha == SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete)))
                        {
                            SinAlitas.Admin.Entidad.ItemDetalle detalle = new SinAlitas.Admin.Entidad.ItemDetalle();
                            detalle.Fecha = SinAlitas.Admin.Entidad.Utiles.RetornaFecha(diaSiete);
                            prof.Detalle.Add(detalle);
                        }
                    }
                }
                if (listaFinal != null && listaFinal.Count > 0)
                {
                    foreach (SinAlitas.Admin.Entidad.ItemProfesor prof in listaFinal)
                    {
                        if (prof.Detalle != null && prof.Detalle.Count > 0)
                        {
                            prof.Detalle = prof.Detalle.OrderBy(p => p.Fecha).ToList();
                        }
                    }
                }
                #endregion



                if (listaFinal != null && listaFinal.Count > 0)
                {
                    httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    String JSON = JsonConvert.SerializeObject(listaFinal);
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
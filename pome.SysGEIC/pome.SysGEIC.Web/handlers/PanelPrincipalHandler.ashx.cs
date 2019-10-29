using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

using pome.SysGEIC.Web.Helpers;

namespace pome.SysGEIC.Web.handlers
{
    /// <summary>
    /// Descripción breve de PanelPrincipalHandler
    /// </summary>
    public class PanelPrincipalHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            
            string resultReturn = string.Empty;
            string accion = string.Empty;

            try
            {
                if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    context.Response.Redirect("../ErrorAcceso.aspx");


                if (context.Request["accion"] != null)
                    accion = context.Request["accion"].ToString();

                switch (accion)
                {
                    case "OBTENER-PROXIMAACTA":
                        resultReturn = this.CargarProximaActa();
                        break;

                    case "CARGARGRAFICO-ESTUDIOSSEMAFORO":
                        resultReturn = this.CargarDatosGraficoEstudiosSemaforo();
                        break;

                    case "CARGARGRAFICO-ESTUDIOSESTADOS":
                        resultReturn = this.CargarDatosGraficoEstudiosEstados();
                        break;

                    case "CARGARGRAFICO-DOCSEVAUADOSMES":
                        resultReturn = this.CargarDatosDocsEvaluadosMes();
                        break;

                    

                }
            }
            catch (Exception ex)
            {
                resultReturn = serializer.Serialize(new { resultado = "Error", mensaje = "Ocurrió un error al obtener información." });
            }

            context.Response.ContentType = "application/json";
            context.Response.Write(resultReturn);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string CargarProximaActa()
        {
            ServicioActas servActas = new ServicioActas();
            return servActas.ObtenerProximaActaDTO().SerializaToJson();
        }

        private string CargarDatosGraficoEstudiosEstados()
        {
            ServicioReportes servicio = new ServicioReportes();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;

            try
            {
                List<string> labels = new List<string>();
                List<int> values = new List<int>();

                List<ItemChart> items = servicio.ObtenerDatosGraficoEstados();

                items.ForEach(delegate(ItemChart item)
                {
                    labels.Add(string.Format("{0}: {1}", item.Descripcion, item.Valor.ToString()));
                    values.Add(item.Valor);
                });

                return serializer.Serialize(new
                {
                    labels = labels.ToArray(),
                    values = values.ToArray()
                });
            }
            catch
            {
                return serializer.Serialize(new { result = "Error", message = "Ha ocurrido un error al obtener datos" });
            }
        }

        private string CargarDatosGraficoEstudiosSemaforo()
        {
            ServicioReportes servicio = new ServicioReportes();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;

            try
            {
                List<string> labels = new List<string>();
                List<int> values = new List<int>();

                List<ItemChart> items = servicio.ObtenerDatosGraficoSemaforo();

                items.ForEach(delegate(ItemChart item)
                {
                    labels.Add(string.Format("{0}: {1}", item.Descripcion, item.Valor.ToString()));
                    values.Add(item.Valor);
                });

                return serializer.Serialize(new
                {
                    labels = labels.ToArray(),
                    values = values.ToArray()
                });
            }
            catch
            {
                return serializer.Serialize(new { result = "Error", message = "Ha ocurrido un error al obtener datos" });
            }
        }

        private string CargarDatosDocsEvaluadosMes()
        {
            ServicioReportes servicio = new ServicioReportes();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string datosReturn = string.Empty;

            /*              
                Id  EstadoDocumento	
                1	INGRESADO
                2	EN EVALUACION
                3	APROBADO
                4	PEDIDO DE CAMBIOS
                5	TOMA CONOCIMIENTO
             */
            try
            {
                //List<string> labels = new List<string>();
                List<int> valuesIngresado = new List<int>();
                List<int> valuesEnEvaluacion = new List<int>();
                List<int> valuesAprobado= new List<int>();
                List<int> valuesPedidoCambio = new List<int>();
                List<int> valuesTomaConocimiento = new List<int>();

                List<ItemChart> items = servicio.ObtenerDatosDocsEvaluadosMes();

                for (int mes = 1; mes <= 12; mes++)
                {
                    valuesIngresado.Add(0);
                    valuesEnEvaluacion.Add(0);
                    valuesAprobado.Add(0);
                    valuesPedidoCambio.Add(0);
                    valuesTomaConocimiento.Add(0);

                    items.FindAll(item => item.Valor == mes)
                        .ForEach(delegate(ItemChart item) 
                    {
                        switch (item.Descripcion)
                        {
                            case "INGRESADO":
                                valuesIngresado[mes - 1] = item.Valor2;
                                break;
                            case "EN EVALUACION":
                                valuesEnEvaluacion[mes - 1] = item.Valor2;
                                break;
                            case "APROBADO":
                                valuesAprobado[mes - 1] = item.Valor2;
                                break;
                            case "PEDIDO DE CAMBIOS":
                                valuesPedidoCambio[mes - 1] = item.Valor2;
                                break;
                            case "TOMA CONOCIMIENTO":
                                valuesTomaConocimiento[mes - 1] = item.Valor2;
                                break;
                        }
                    });
                }

                return serializer.Serialize(new
                {
                    valuesIngresado = valuesIngresado.ToArray(),
                    valuesEnEvaluacion = valuesEnEvaluacion.ToArray(),
                    valuesAprobado = valuesAprobado.ToArray(),
                    valuesPedidoCambio = valuesPedidoCambio.ToArray(),
                    valuesTomaConocimiento = valuesTomaConocimiento.ToArray()
                });
            }
            catch
            {
                return serializer.Serialize(new { result = "Error", message = "Ha ocurrido un error al obtener datos" });
            }
        }
    }
}
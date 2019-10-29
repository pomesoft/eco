using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Script.Serialization;

using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web.handlerspublic
{
    /// <summary>
    /// Descripción breve de MailHandler
    /// </summary>
    public class MailHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string resultReturn = string.Empty;
            string accion = string.Empty;

            try
            {
                
                if (context.Request["accion"] != null)
                    accion = context.Request["accion"].ToString();

                switch (accion)
                {
                    case "HOME-ENVIARMAIL":
                        resultReturn = this.EnviarMailContacto(context.Request["datosContacto"]);
                        break;

                }

            }
            catch (Exception ex)
            {

                resultReturn = serializer.Serialize(new { result = "Error", message = ex.Message });
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

        public string EnviarMailContacto(string datosContacto)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                ServicioCorreoElectronico servMail = new ServicioCorreoElectronico();
                servMail.EnviarMailContactoHomePage(datosContacto);
                
                return serializer.Serialize(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return serializer.Serialize(new { result = "Error", message = ex.Message });
            }
        }
    }
}
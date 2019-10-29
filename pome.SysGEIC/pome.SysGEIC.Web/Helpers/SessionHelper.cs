using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.ServiciosAplicacion;

namespace pome.SysGEIC.Web.Helpers
{
    public static class SessionHelper
    {        
        private static string prefixSession = "SysGEIC:";

        public static void GuardarEnSession(string nombreKey, object objeto)
        {
            HttpContext contextActual = HttpContext.Current;
            string key = prefixSession + nombreKey;
            
            contextActual.Session[key] = objeto;
        }
        public static object ObtenerDeSession(string nombreKey)
        {
            HttpContext contextActual = HttpContext.Current;
            string key = prefixSession + nombreKey;
            return contextActual.Session[key];
        }
        public static Usuario ObtenerUsuarioLogin()
        {
            ServicioAccesoUsuarios servUsuarios = new ServicioAccesoUsuarios();
            string idUsuario = Thread.CurrentPrincipal.Identity.Name.Substring(0, Thread.CurrentPrincipal.Identity.Name.IndexOf('-') - 1);
            Usuario usr = servUsuarios.ObtenerUsuario(int.Parse(idUsuario.TrimEnd()));
            return usr;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace pome.SysGEIC.Comunes
{
    public static class Logger
    {

        //private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog logger = LogManager.GetLogger("pomeLogger");

        public static void LogError(string Identificacion, Exception ex)
        {
            logger.ErrorFormat("{0} - {1} - {2} \n\t- Error Original: {3} \n\t- StackTrace: {4}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), Identificacion, ex.Message, ex.GetExceptionOriginal().Message, ex.StackTrace);
        }

        public static void LogError(Exception ex)
        {
            logger.ErrorFormat("{0} - {1} \n\t- Error Original: {2} \n\t- StackTrace: {3}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Message, ex.GetExceptionOriginal().Message, ex.StackTrace);
        }

        public static void LogInfo(string Identificacion, string MensajeInfo)
        {
            logger.InfoFormat("{0} - {1} {2}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), Identificacion, MensajeInfo);
        }

        public static Exception GetExceptionOriginal(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetExceptionOriginal();
        }
    }
}

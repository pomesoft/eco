using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

using pml.Utilidades;
using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public static class ServiciosHelpers
    {
        public const char ID_SEP = ';';

        static string key = null;
        static string claveEncriptada = null;
        static string baseDatos = null;
        static string servidor = null;
        static Parametro comite = null;
        static Parametro fechaImplementacion = null;


        public static string ConvertirString(this object valor)
        {
            return valor == null ? string.Empty : valor.ToString();
        }

        public static int ConvertirInt(this object valor)
        {
            int _valor = -1;
            if (valor != null && !int.TryParse(valor.ToString(), out _valor))
                _valor = -1;
            return _valor;
        }

        public static bool ConvertirBool(this object valor)
        {
            bool _valor = false;
            if (valor != null && !bool.TryParse(valor.ToString(), out _valor))
                _valor = false;
            return _valor;
        }

        public static bool? ConvertirBoolNulleable(this object valor)
        {
            bool? _valorReturn = null;
            bool _valor = false;
            if (valor != null && bool.TryParse(valor.ToString(), out _valor))
                _valorReturn = _valor;
            return _valorReturn;
        }

        public static DateTime ConvertirDateTime(this object valor)
        {
            DateTime _valor = DateTime.MinValue;
            if (valor != null && !DateTime.TryParse(valor.ToString(), out _valor))
                _valor = DateTime.MinValue;
            return _valor;
        }

       
        public static bool ClaveInicializada
        {
            get 
            {
                InicializarClave();
                return (comite != null && fechaImplementacion != null && key != null && claveEncriptada != null); 
            }
        }

        public static string GetClaveInicializada()
        {
            string clave = string.Empty;

            if (ClaveInicializada)
            {
                clave = string.Format("{0}{1}{2}{3}{4}", "pome@soft",
                                                        comite.Valor,
                                                        fechaImplementacion.Valor,
                                                        baseDatos,
                                                        servidor);
            }
            return clave;            
        }

        public static string DescodificarUTF8(string texto)
        {
            return Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(texto));
        }

        private static void InicializarClave()
        {
            try
            {
                AppSettingsReader webConfigReader = new AppSettingsReader();
                key = "QEzBx86jTAs=";
                claveEncriptada = (string)webConfigReader.GetValue("nhibernateConfiguration", typeof(string));
                
                ServicioParametricas servicio = new ServicioParametricas();
                baseDatos = servicio.BaseDatos;
                servidor = servicio.ServidorBaseDatos;
                comite = servicio.ObtenerObjeto<Parametro>("Descripcion", "Comite");
                fechaImplementacion = servicio.ObtenerObjeto<Parametro>("Descripcion", "FechaImplementacion");
            }
            catch { }
        }

        public static bool ValidarClave()
        {
            bool claveOK = true;
            
            InicializarClave();

            if (ClaveInicializada)
            {
                string claveDesencriptada = Encriptar.Decrypt(claveEncriptada, new DESCryptoServiceProvider(), key);
                string clave = GenerarClave();
                claveOK = clave.Equals(claveDesencriptada);
            }

            return claveOK;
        }

        public static string GenerarClave()
        {
            InicializarClave();
            return string.Format("{0}{1}{2}{3}{4}", "pome@soft",
                                                    comite.Valor,
                                                    fechaImplementacion.Valor,
                                                    baseDatos,
                                                    servidor);
        }

        public static void ValidarClave(Estudio estudio)
        {
            if (!ClaveInicializada && estudio.Documentos.Count >= 5)
                throw new ApplicationException("ApplicationExceptionFatal. <br />No se pueden actualizar Documento.");
        }

        public static void ValidarClave(Acta acta)
        {
            if (!ClaveInicializada && acta.Documentos.Count >= 5)
                throw new ApplicationException("ApplicationExceptionFatal. <br />No se pueden actualizar Acta.");
        }

        public static dynamic DeserializarGenerico(string datos)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            return serializer.Deserialize<object>(datos); 
        }

        public static string ObtenerTextoPlano(string strSrc)
        {
            try
            {

                string resultStr = string.Empty;

                strSrc = strSrc.Replace("&lt;", "<");
                strSrc = strSrc.Replace("&gt;", ">");

                strSrc = strSrc.Replace("&iquest;", "¿");
                strSrc = strSrc.Replace("&aacute;", "á");
                strSrc = strSrc.Replace("&eacute;", "é");
                strSrc = strSrc.Replace("&iacute;", "í");
                strSrc = strSrc.Replace("&oacute;", "ó");
                strSrc = strSrc.Replace("&uacute;", "ú");
                strSrc = strSrc.Replace("&ntilde;", "ñ");
                strSrc = strSrc.Replace("&Aacute;", "Á");
                strSrc = strSrc.Replace("&Eacute;", "É");
                strSrc = strSrc.Replace("&Iacute;", "Í");
                strSrc = strSrc.Replace("&Oacute;", "Ó");
                strSrc = strSrc.Replace("&Uacute;", "Ú");
                strSrc = strSrc.Replace("&Ntilde;", "Ñ");


                // Ignore the <p> tag if it is in very start of the text
                if (strSrc.StartsWith("<p>"))
                    resultStr = strSrc.Substring(3);
                else
                    resultStr = strSrc;

                // Replace <p> with two newlines
                resultStr = resultStr.Replace("<p>", "\r\n\r\n");
                // Replace <br /> with one newline
                resultStr = resultStr.Replace("<br />", "\r\n");
                resultStr = resultStr.Replace("<br>", "\r\n");

                resultStr = resultStr.Replace("&nbsp;", "\r\n");

                //-+-+-+-+-+-+-+-+-+-+-+
                // Strip off other HTML tags.
                //-+-+-+-+-+-+-+-+-+-+-+


                resultStr = System.Text.RegularExpressions.Regex.Replace(resultStr,
                         "<[^<|>]+?>", "",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                return resultStr;
            }
            catch
            {
                return strSrc;
            }
        }
            
    }
}

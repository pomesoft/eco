using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Web;

namespace pome.SysGEIC.Web.Helpers
{
    public enum Permisos : short
    {
        SIN_ACCESO_ = 1,
        SOLO_LECTURA_ = 2,
        AGREGAR_EDITAR_ = 3,
        ELIMINAR_ = 4
    }

    public static class UtilHelper
    {
        public static string PathAplicacion
        {
            get
            {
                string path = string.Empty;
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().EscapedCodeBase).Substring(6);
                return path.Substring(0, path.Length - 4);
            }
        }

        public static string DirectorioImagenes
        {
            get { return PathAplicacion + @"\img"; }
        }

        public static string DirectorioArchivos
        {
            get { return PathAplicacion + @"\Archivos"; }
        }

        public static string DirectorioArchivosTemp
        {
            get { return DirectorioArchivos + @"\temp"; }
        }
        
        public static void GuardarArchivo(string archivoTemp, string nombreArchivo)
        {
            string archivoOrigen = Path.Combine(DirectorioArchivosTemp, archivoTemp);
            string archivoDestino = Path.Combine(DirectorioArchivos, nombreArchivo);

            if (!Directory.Exists(DirectorioArchivos))
                Directory.CreateDirectory(DirectorioArchivos);

            if (!Directory.Exists(DirectorioArchivosTemp))
                Directory.CreateDirectory(DirectorioArchivosTemp);

            if (File.Exists(archivoDestino))
                File.Delete(archivoDestino);

            File.Move(archivoOrigen, archivoDestino);
        }
    }
}
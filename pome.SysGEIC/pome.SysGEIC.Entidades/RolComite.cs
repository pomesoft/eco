using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class RolComite : EntidadBaseParametrica
    {        
        public RolComite() { }

        public static int RolPresidente()
        {
            return 1;
        }

        public static int RolVocalTitular()
        {
            return 2;
        }

        public static int RolVocalSuplente()
        {
            return 3;
        }

        public static int RolFuncionesPresidente()
        {
            return 4;
        }
    }
}

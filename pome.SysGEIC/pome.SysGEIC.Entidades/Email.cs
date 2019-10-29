using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Email : EntidadBaseParametrica
    {
        public Email() { }

        public Email(int id, string descripcion) 
        {
            Id = id;
            Descripcion = descripcion;
            Vigente = true;
        }
    }
}

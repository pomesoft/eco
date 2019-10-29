using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace pome.SysGEIC.ConsolaTesting
{
    [DataContract]
    public class Cliente
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public string Direccion { get; set; }

        public Cliente(int _id, string _nombre, string _apellido, string _documento, string _direccion) 
        {
            Id = _id;
            Nombre = _nombre;
            Apellido = _apellido;
            Documento = _documento;
            Direccion = _direccion;
        }

    }
}

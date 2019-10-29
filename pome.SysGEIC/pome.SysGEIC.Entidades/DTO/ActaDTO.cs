using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class ActaDTO
    {
        [DataMember]
        public virtual int IdActa { get; set; }
        [DataMember]
        public virtual string Descripcion { get; set; }
        
        public virtual DateTime FechaActa { get; set; }
        [DataMember]
        public virtual string Fecha
        {
            get { return FechaActa.ToShortDateString(); }
            set { }
        }
        [DataMember]
        public virtual string Hora { get; set; }
        [DataMember]
        public virtual string ComentarioInicialFijo { get; set; }
        [DataMember]
        public virtual string ComentarioInicial { get; set; }
        [DataMember]
        public virtual string ComentarioFinal { get; set; }
        [DataMember]
        public virtual bool Cerrada { get; set; }
        [DataMember]
        public virtual List<ActaEstudioDTO> EstudiosTratados { get; set; }
        [DataMember]
        public virtual IList<ActaProfesional> Participantes { get; set; }

        public ActaDTO()
        {
            EstudiosTratados = new List<ActaEstudioDTO>();            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class Acta : EntidadBaseParametrica
    {
        public virtual DateTime Fecha { get; set; }        
        [DataMember]
        public virtual string FechaToString
        {
            get { return Fecha.ToShortDateString(); }
            set { }
        }
        [DataMember]
        public virtual string Hora { get; set; }        
        [DataMember]
        public virtual IList<ActaProfesional> Participantes { get; set; }
        //TODO: Refactorizar el modelo, debe ser Acta -> Estudio -> Documento. Se deberia quitar la relaciona Acta -> Documento
        [DataMember]
        public virtual IList<ActaEstudio> Estudios { get; set; }
        [DataMember]
        public virtual IList<ActaDocumento> Documentos { get; set; }
        [DataMember]
        public virtual string ComentarioInicialFijo { get; set; }
        [DataMember]
        public virtual string ComentarioInicial { get; set; }
        [DataMember]
        public virtual string ComentarioFinal { get; set; }
        [DataMember]
        public virtual bool Cerrada { get; set; }
        [DataMember]
        public virtual IList<Nota> Notas { get; set; }
        [DataMember]
        public virtual Profesional PresidenteComite 
        {
            get 
            {
                List<ActaProfesional> actaProfesionales = this.ObtenerParticipantes(RolComite.RolPresidente());
                if (actaProfesionales == null || actaProfesionales.Count == 0)
                    actaProfesionales = this.ObtenerParticipantes(RolComite.RolFuncionesPresidente());
                return (actaProfesionales == null || actaProfesionales.Count == 0) ? null : actaProfesionales[0].Profesional;
            }
            set { }
        }
        [DataMember]
        public virtual Profesional RolPresidenteComite
        {
            get
            {
                List<ActaProfesional> actaProfesionales = this.ObtenerParticipantes(RolComite.RolPresidente());
                if (actaProfesionales == null || actaProfesionales.Count == 0)
                    actaProfesionales = this.ObtenerParticipantes(RolComite.RolFuncionesPresidente());
                return (actaProfesionales == null || actaProfesionales.Count == 0) ? null : actaProfesionales[0].Profesional;
            }
            set { }
        }
        [DataMember]
        public virtual List<Profesional> Vocales
        {
            get
            {
                List<ActaProfesional> actaProfesionales = new List<ActaProfesional>();
                actaProfesionales.AddRange(this.ObtenerParticipantes(RolComite.RolVocalTitular()));
                actaProfesionales.AddRange(this.ObtenerParticipantes(RolComite.RolVocalSuplente()));

                List<Profesional> vocales = new List<Profesional>();
                actaProfesionales.ForEach(delegate(ActaProfesional actaProfesional)
                { 
                    vocales.Add(actaProfesional.Profesional); 
                });
                return vocales.OrderBy(item => item.OrdenActa).ToList<Profesional>();
            }
            set { }
        }
        [DataMember]
        public virtual List<Profesional> VocalesTitulares
        {
            get
            {
                List<ActaProfesional> actaProfesionales = this.ObtenerParticipantes(RolComite.RolVocalTitular());
                List<Profesional> vocales = new List<Profesional>();
                actaProfesionales.ForEach(delegate(ActaProfesional actaProfesional) 
                { vocales.Add(actaProfesional.Profesional); });
                return vocales;
            }
            set { }
        }
        [DataMember]
        public virtual string ParticipantesToString
        {
            get 
            {
                string profesionales = string.Empty;
                Participantes.ToList<ActaProfesional>()
                    .OrderBy(item=>item.Profesional.OrdenActa)
                    .ToList<ActaProfesional>()
                    .ForEach(delegate(ActaProfesional actaProfesional)
                {
                    profesionales += string.Format(", {0}", actaProfesional.Profesional.NombreYApellido);
                });
                return profesionales != string.Empty ? profesionales.Substring(1) : string.Empty;
            }
            set { }
        }
        
        public Acta() 
        {
            Estudios = new List<ActaEstudio>();
            Documentos = new List<ActaDocumento>();
            Participantes = new List<ActaProfesional>();
            Notas = new List<Nota>();
        }

        public virtual ActaDocumento ObtenerDocumento(int idActaDocumento)
        {
            ActaDocumento documentoReturn = null;
            Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDocumento)
            {
                if (actaDocumento.Id == idActaDocumento)
                    documentoReturn = actaDocumento;
            });
            return documentoReturn;
        }

        public virtual bool EstudioTieneNotas(int idEstudio)
        {            
            bool tieneNotas = Notas.ToList<Nota>()
                                     .FindAll(item => item.IdEstudio == idEstudio)
                                     .Count > 0;

            return tieneNotas;
        }

        public virtual int ObtenerOrdenEstudio(int idEstudio)
        {
            int orden = -1;

            ActaDocumento actaDoc = Documentos.ToList<ActaDocumento>().Find(delegate(ActaDocumento actaDocumento) 
            { 
                return actaDocumento.DocumentoVersion.Documento.Estudio.Id == idEstudio; 
            });

            if (actaDoc != null)
            {
                orden = actaDoc.OrdenEstudio;
            }
            else
            {
                idEstudio = -1;
                Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDocumento)
                {
                    if (actaDocumento.DocumentoVersion.Documento.Estudio.Id != idEstudio)
                    {
                        idEstudio = actaDocumento.DocumentoVersion.Documento.Estudio.Id;
                        if (orden <= actaDocumento.OrdenEstudio)
                            orden = actaDocumento.OrdenEstudio + 1;
                    }                        
                });
            }
            return orden != -1 ? orden : 1;
        }

        public virtual int ObtenerOrdenUltimoDocumentoDelEstudio(int idEstudio)
        {
            int orden = 0;
            Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDocumento)
            {
                if (actaDocumento.DocumentoVersion.Documento.Estudio.Id == idEstudio)
                    orden = actaDocumento.OrdenDocumento;
            });
            return ++orden;
        }
        //TODO: Refactorizar el modelo, debe ser Acta -> Estudio -> Documento. Se deberia quitar la relaciona Acta -> Documento
        public virtual void AgregarEstudio(ActaEstudio actaEstudio)
        {
            if (actaEstudio.Id == -1)
            {
                Estudios.ToList<ActaEstudio>().ForEach(delegate(ActaEstudio actaEst)
                {
                    if (actaEst.Estudio == actaEstudio.Estudio)
                        throw new ApplicationException(string.Format("El Estudio {0} ya existe en la acta {1}",
                                                                    actaEstudio.Estudio.Descripcion,
                                                                    this.Descripcion));

                });
            }

            actaEstudio.Acta = this;
            Estudios.Add(actaEstudio);
        }

        public virtual void EliminarEstudio(int idEstudio)
        {
            Estudios.ToList<ActaEstudio>().ForEach(delegate(ActaEstudio actaEst)
            {
                if (actaEst.Estudio.Id == idEstudio)
                    Estudios.Remove(actaEst);
            });
        }

        public virtual ActaEstudio ObtenerEstudio(int idEstudio)
        {            
            ActaEstudio EstudioReturn = null;
            Estudios.ToList<ActaEstudio>().ForEach(delegate(ActaEstudio actaEstudio)
            {
                if (actaEstudio.Estudio.Id == idEstudio)
                    EstudioReturn = actaEstudio;
            });
            return EstudioReturn;
        }

        public virtual void AgregarDocumento(ActaDocumento actaDocumento)
        {
            if (actaDocumento.Id == -1)
            {
                Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDoc)
                {
                    if (actaDoc.DocumentoVersion == actaDocumento.DocumentoVersion)
                        throw new ApplicationException(string.Format("El Documento {0} versión {1} ya existe en la acta {2}",
                                                                    actaDocumento.DocumentoVersion.Documento.Descripcion,
                                                                    actaDocumento.DocumentoVersion.Descripcion,
                                                                    this.Descripcion));

                });
            }

            actaDocumento.Acta = this;
            Documentos.Add(actaDocumento);
        }

        public virtual void EliminarDocumento(ActaDocumento actaDocumento)
        {
            Documentos.ToList<ActaDocumento>().ForEach(delegate(ActaDocumento actaDoc)
            {
                if (actaDoc == actaDocumento)
                    Documentos.Remove(actaDoc);
            });
        }

        public virtual void EliminarParticipante(ActaProfesional actaProfesional)
        {
            Participantes.ToList<ActaProfesional>().ForEach(delegate(ActaProfesional actaProf)
            {
                if (actaProf == actaProfesional)
                    Participantes.Remove(actaProf);
            });
        }

        public virtual void AgregarDocumento(Documento documento)
        {
            ActaDocumento actaDocumento = new ActaDocumento();
            actaDocumento.Acta = this;
            actaDocumento.DocumentoVersion.Documento = documento;
            Documentos.Add(actaDocumento);
        }

        public virtual void EliminarDocumentos()
        {
            if (Documentos.Count > 0)
                Documentos.Clear();
        }
        public virtual ActaProfesional ObtenerParticipante(int idActaProfesional)
        {
            ActaProfesional participanteReturn = null;
            participanteReturn = Participantes.ToList<ActaProfesional>().Find(delegate(ActaProfesional actaProfesional)
            {
                return actaProfesional.Id == idActaProfesional;
            });
            return participanteReturn;
        }
        
        public virtual void AgregarParticipante(ActaProfesional actaProfesional)
        {
            if (actaProfesional.Id == -1)
            {
                ActaProfesional participanteExistente = null;
                participanteExistente = Participantes.ToList<ActaProfesional>().Find(delegate(ActaProfesional actaProf)
                {
                    return actaProf.Profesional == actaProfesional.Profesional;
                });
                if (participanteExistente != null)
                {
                    throw new ApplicationException(string.Format("El profesional {0} ya existe en la acta {1}",
                                                                actaProfesional.Profesional.NombreCompleto,
                                                                this.Descripcion));
                }
            }
            actaProfesional.Acta = this;
            Participantes.Add(actaProfesional);
        }

        public virtual void ActualizarParticipanteRolcomite(int idActaParticipante, RolComite rolComite)
        {
            ActaProfesional actaProfesional = Participantes.ToList<ActaProfesional>().Find(delegate(ActaProfesional actaProf) { return actaProf.Id == idActaParticipante; });
            if (actaProfesional != null)
                actaProfesional.RolComite = rolComite;
        }

        public virtual void AgregarParticipante(Profesional profesional)
        {
            ActaProfesional actaProfesional = new ActaProfesional();
            actaProfesional.Acta = this;
            actaProfesional.Profesional = profesional;
            Participantes.Add(actaProfesional);
        }

        public virtual void EliminarParticipantes()
        {
            if (Participantes.Count > 0)
                Participantes.Clear();
        }

        public virtual List<ActaProfesional> ObtenerParticipantes(int idRolComite)
        {
            List<ActaProfesional> participanteReturn = null;
            participanteReturn = Participantes.ToList<ActaProfesional>().FindAll(delegate(ActaProfesional actaProfesional)
            {
                return actaProfesional.RolComite != null 
                    && actaProfesional.RolComite.Id == idRolComite;
            });
            return participanteReturn;
        }

        public virtual RolComite ObtenerRolComiteParticipantes(Profesional profesional)
        {
            ActaProfesional actaProf = Participantes.ToList<ActaProfesional>().Find(delegate(ActaProfesional actaProfesional)
            {
                return actaProfesional.Profesional.Id == profesional.Id;
            });
            return actaProf != null ? actaProf.RolComite : null;
        }

        public virtual void AgregarNota(Nota nota)
        {
            Notas.ToList<Nota>().ForEach(delegate(Nota actaNota)
            {
                if (nota.Id == -1)
                {
                    if (this.Id == actaNota.IdActa && actaNota.ActaImprimeAlFinal == nota.ActaImprimeAlFinal)
                        throw new ApplicationException(string.Format("La Nota {0} ya existe en la acta {2}",
                                                                    nota.Descripcion,
                                                                    this.Descripcion));
                }
                else
                {
                    if (nota.Id == actaNota.Id)
                        throw new ApplicationException(string.Format("La Nota {0} ya existe en la acta {2}",
                                                                    nota.Descripcion,
                                                                    this.Descripcion));
                }
            });
            nota.Acta = this;
            Notas.Add(nota);
        }

        public virtual void EliminarNota(Nota nota)
        {
            Notas.ToList<Nota>().ForEach(delegate(Nota actaNota)
            {
                if (actaNota == nota)
                    Notas.Remove(actaNota);
            });
        }

        public override void Validar()
        {
            base.Validar();

            if (Fecha == DateTime.MinValue)
                throw new ApplicationException("Debe ingresar fecha.");

            if (Hora.Trim().Length==0)
                throw new ApplicationException("Debe ingresar hora.");

        }
    }
}

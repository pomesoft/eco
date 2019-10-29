using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class Estudio : EntidadBaseParametrica
    {
        [DataMember]
        public virtual string Codigo { get; set; }
        [DataMember]
        public virtual string NombreCompleto { get; set; }
        [DataMember]
        public virtual string Poblacion { get; set; }
        [DataMember]
        public virtual EstadoEstudio Estado { get; set; }
        [DataMember]
        public virtual Patologia Patologia { get; set; }        
        [DataMember]
        public virtual IList<EstudioPatrocinador> Patrocinadores { get; set; }
        [DataMember]
        public virtual IList<EstudioMonodroga> Monodrogas { get; set; }
        [DataMember]
        public virtual IList<EstudioCentro> CentrosHabilitados { get; set; }
        [DataMember]
        public virtual IList<Documento> Documentos { get; set; }
        [DataMember]
        public virtual IList<Nota> Notas { get; set; }
        [DataMember]
        public virtual IList<EstudioParticipante> Participantes { get; set; }
        [DataMember]
        public virtual DateTime? FechaPresentacion { get; set; }
        [DataMember]
        public virtual bool RequiereAlerta { get; set; }
        [DataMember]
        public virtual int MesesAlerta { get; set; }
        [DataMember]
        public virtual int IdRecordatorioRenovacionAnual { get; set; }
        [DataMember]
        public virtual int EstadoSemaforo { get; set; }
        [DataMember]
        public virtual int? IdTipoEstudio { get; set; }
        [DataMember]
        public virtual TipoEstudio TipoEstudio { get; set; }

        public virtual IList<EstudioTipoDocumento> TiposDocumentoSemaforo { get; set; }

        [DataMember]
        public virtual string NombreEstudioListados
        {
            get { return string.Format("{0} {2} {1}", this.Codigo, this.Descripcion, (this.Descripcion.Trim().Length > 0 ? "-" : "")); }
            set { }
        }

        public virtual List<EstudioParticipante> InvestigadoresPrincipales 
        {
            get { return ObtenerInvestigadoresPrincipales(); }
        }
        [DataMember]
        public virtual List<Profesional> InvestigadoresPrincipalesProfesional
        {
            get 
            {
                List<EstudioParticipante> investigadores = ObtenerInvestigadoresPrincipales();
                List<Profesional> profesionales = new List<Profesional>();
                investigadores.ForEach(delegate(EstudioParticipante estInvestigador) 
                {
                    profesionales.Add(estInvestigador.Profesional);
                });
                return profesionales;
            }
        }
        
        public virtual Centro CentroHabilitado
        {
            get
            {
                Centro centroReturn = null;
                List<EstudioCentro> centros = CentrosHabilitados.ToList<EstudioCentro>();
                if (centros.Count > 0)
                    centroReturn = centros[0].Centro;
                return centroReturn;
            }

        }

        [DataMember]
        public virtual string FechaPresentacionToString
        {
            get { return FechaPresentacion.HasValue ? FechaPresentacion.Value.ToShortDateString() : string.Empty; }
            set { }
        }

        public Estudio() 
        {
            Patrocinadores = new List<EstudioPatrocinador>();
            CentrosHabilitados = new List<EstudioCentro>();
            Monodrogas = new List<EstudioMonodroga>();
            Participantes = new List<EstudioParticipante>();
            Documentos = new List<Documento>();
            Notas = new List<Nota>();
        }

        public override void Validar()
        {
            string mensajes = string.Empty;

            if (string.IsNullOrEmpty(Codigo))
                mensajes += "Debe ingresar código." + "<br />";
            if (string.IsNullOrEmpty(NombreCompleto))
                mensajes += "Debe ingresar nombre completo." + "<br />";
            if (Patologia == null)
                mensajes += "Debe seleccionar patologia." + "<br />";

            if (!string.IsNullOrEmpty(mensajes))
                throw new ApplicationException(mensajes);
        }

        
        public virtual void AgregarPatrocinador(EstudioPatrocinador estudioPatrocinador)
        {
            if (estudioPatrocinador.Id == -1)
            {
                /*Patrocinadores.ToList<EstudioPatrocinador>().ForEach(delegate(EstudioPatrocinador estPatrocinador)
                {
                    if (estPatrocinador.Patrocinador == estudioPatrocinador.Patrocinador)
                        throw new ApplicationException(string.Format("El Patrocinador Habilitado {0} ya existe en el estudio {1}",
                                                                    estudioPatrocinador.Patrocinador.Descripcion,
                                                                    this.Descripcion));
                });*/

                EstudioPatrocinador patrocinadorExiste = this.ObtenerPatrocinador(estudioPatrocinador.Patrocinador.Id);
                if (patrocinadorExiste != null)
                    throw new ApplicationException(string.Format("El Patrocinador Habilitado {0} ya existe en el estudio {1}",
                                                                    estudioPatrocinador.Patrocinador.Descripcion,
                                                                    this.Descripcion));
            }
            estudioPatrocinador.Estudio = this;
            Patrocinadores.Add(estudioPatrocinador);
        }

        public virtual EstudioPatrocinador ObtenerPatrocinador(int idPatrocinador)
        {   
            EstudioPatrocinador patrocinadorReturn = Patrocinadores.ToList<EstudioPatrocinador>()
                                                                   .Find(delegate(EstudioPatrocinador match) 
            { 
                return match.Id == idPatrocinador; 
            });
            
            return patrocinadorReturn;
        }

        public virtual void EliminarPatrocinador(EstudioPatrocinador estudioPatrocinador)
        {
            Patrocinadores.ToList<EstudioPatrocinador>().ForEach(delegate(EstudioPatrocinador estPatrocinador)
            {
                if (estPatrocinador == estudioPatrocinador)
                    Patrocinadores.Remove(estPatrocinador);
            });
        } 

        public virtual void AgregarCentro(Centro centro)
        {
            CentrosHabilitados.ToList<EstudioCentro>().ForEach(delegate(EstudioCentro estCentro)
            {
                if (estCentro.Centro == centro)
                    throw new ApplicationException(string.Format("El Centro {0} ya existe en el estudio {1}",
                                                                centro.Descripcion,
                                                                this.Descripcion));
            });
            EstudioCentro estudioCentro = new EstudioCentro();
            estudioCentro.Centro = centro;
            estudioCentro.Estudio = this;
            estudioCentro.Vigente = true;
            CentrosHabilitados.Add(estudioCentro);
        }

        public virtual void AgregarCentroHabilitado(EstudioCentro estudioCentro)
        {
            if (estudioCentro.Id == -1)
            {
                CentrosHabilitados.ToList<EstudioCentro>().ForEach(delegate(EstudioCentro estCentro)
                {
                    if (estCentro.Centro == estudioCentro.Centro)
                        throw new ApplicationException(string.Format("El Centro Habilitado {0} ya existe en el estudio {1}",
                                                                    estudioCentro.Centro.Descripcion,
                                                                    this.Descripcion));
                });
            }
            estudioCentro.Estudio = this;
            CentrosHabilitados.Add(estudioCentro);
        }

        public virtual EstudioCentro ObtenerCentroHabilitado(int idCentroHabilitado)
        {
            EstudioCentro centroHabilitadoReturn = null;
            CentrosHabilitados.ToList<EstudioCentro>().ForEach(delegate(EstudioCentro estCentro)
            {
                if (estCentro.Id == idCentroHabilitado)
                    centroHabilitadoReturn = estCentro;
            });
            return centroHabilitadoReturn;
        }

        public virtual void EliminarCentroHabilitado(EstudioCentro estudioCentro)
        {
            CentrosHabilitados.ToList<EstudioCentro>().ForEach(delegate(EstudioCentro estCentro)
            {
                if (estCentro == estudioCentro)
                    CentrosHabilitados.Remove(estCentro);
            });
        } 

        public virtual void EliminarCentro(Centro centro)
        {
            CentrosHabilitados.ToList<EstudioCentro>().ForEach(delegate(EstudioCentro estCentro)
            {
                if (estCentro.Centro == centro)
                    CentrosHabilitados.Remove(estCentro);
            });
        }

        public virtual void AgregarMonodroga(Monodroga monodroga)
        {
            Monodrogas.ToList<EstudioMonodroga>().ForEach(delegate(EstudioMonodroga estMonodroga)
            {
                if (estMonodroga.Monodroga == monodroga)
                    throw new ApplicationException(string.Format("El Centro {0} ya existe en el estudio {1}",
                                                                monodroga.Descripcion,
                                                                this.Descripcion));
            });
            EstudioMonodroga estudioMonodroga = new EstudioMonodroga();
            estudioMonodroga.Monodroga = monodroga;
            estudioMonodroga.Estudio = this;
            estudioMonodroga.Vigente = true;
            Monodrogas.Add(estudioMonodroga);
        }

        public virtual EstudioMonodroga ObtenerMonodroga(int idMonodroga)
        {
            EstudioMonodroga participanteReturn = null;
            Monodrogas.ToList<EstudioMonodroga>().ForEach(delegate(EstudioMonodroga estMonodroga)
            {
                if (estMonodroga.Id == idMonodroga)
                    participanteReturn = estMonodroga;
            });
            return participanteReturn;
        }

        public virtual void AgregarMonodroga(EstudioMonodroga estudioMonodroga)
        {
            if (estudioMonodroga.Id == -1)
            {
                Monodrogas.ToList<EstudioMonodroga>().ForEach(delegate(EstudioMonodroga estMonodroga)
                {
                    if (estMonodroga.Monodroga == estudioMonodroga.Monodroga)
                        throw new ApplicationException(string.Format("El Monodroga Habilitado {0} ya existe en el estudio {1}",
                                                                    estudioMonodroga.Monodroga.Descripcion,
                                                                    this.Descripcion));
                });
            }
            estudioMonodroga.Estudio = this;
            Monodrogas.Add(estudioMonodroga);
        }

        public virtual void EliminarMonodroga(Monodroga monodroga)
        {
            Monodrogas.ToList<EstudioMonodroga>().ForEach(delegate(EstudioMonodroga estMonodroga)
            {
                if (estMonodroga.Monodroga == monodroga)
                    Monodrogas.Remove(estMonodroga);
            });
        }

        public virtual void EliminarMonodroga(EstudioMonodroga estudioMonodroga)
        {
            Monodrogas.ToList<EstudioMonodroga>().ForEach(delegate(EstudioMonodroga estMonodroga)
            {
                if (estMonodroga == estudioMonodroga)
                    Monodrogas.Remove(estMonodroga);
            });
        }
        
        public virtual void AgregarParticipante(Profesional profesional, Rol rol, DateTime? desde, DateTime? hasta)
        {
            Participantes.ToList<EstudioParticipante>().ForEach(delegate(EstudioParticipante estParticipante)
            {
                if (estParticipante.Profesional == profesional)
                    throw new ApplicationException(string.Format("El Participante {0} ya existe en el estudio {1}",
                                                                profesional.NombreCompleto,
                                                                this.Descripcion));
            });
            EstudioParticipante estudioParticipante = new EstudioParticipante();
            estudioParticipante.Estudio = this;
            estudioParticipante.Profesional = profesional;
            estudioParticipante.Rol = rol;
            if (desde.HasValue)
                estudioParticipante.Desde = desde.Value;
            if (hasta.HasValue)
                estudioParticipante.Hasta = hasta.Value;
            
            Participantes.Add(estudioParticipante);
        }

        public virtual void AgregarParticipante(EstudioParticipante participante)
        {
            if (participante.Id == -1)
            {
                Participantes.ToList<EstudioParticipante>().ForEach(delegate(EstudioParticipante estParticipante)
                {
                    if (estParticipante.Profesional == participante.Profesional)
                        throw new ApplicationException(string.Format("El Participante {0} ya existe en el estudio {1}",
                                                                    participante.Profesional.NombreCompleto,
                                                                    this.Descripcion));
                });
            }
            participante.Estudio = this;
            Participantes.Add(participante);
        }

        public virtual void EliminarParticipante(EstudioParticipante participante)
        {
            Participantes.ToList<EstudioParticipante>().ForEach(delegate(EstudioParticipante estParticipante)
            {
                if (estParticipante == participante)
                    Participantes.Remove(estParticipante);
            });
        }

        public virtual EstudioParticipante ObtenerParticipante(int idParticipante)
        {
            EstudioParticipante participanteReturn = null;
            Participantes.ToList<EstudioParticipante>().ForEach(delegate(EstudioParticipante estParticipante)
            {
                if (estParticipante.Id == idParticipante)
                    participanteReturn = estParticipante;
            });
            return participanteReturn;
        }

        private List<EstudioParticipante> ObtenerInvestigadoresPrincipales()
        {
            List<EstudioParticipante> investigadoresPrincipales = new List<EstudioParticipante>();
            Participantes.ToList<EstudioParticipante>().ForEach(delegate(EstudioParticipante estParticipante)
            {
                if (estParticipante.EsInvestigadorPrincipal)
                    investigadoresPrincipales.Add(estParticipante);
            });
            return investigadoresPrincipales;
        }

        public virtual void AgregarDocumento(Documento documento)
        {
            Documentos.ToList<Documento>().ForEach(delegate(Documento estDocumento)
            {
                if (documento.Descripcion != string.Empty && estDocumento.Descripcion.Equals(documento.Descripcion))
                    throw new ApplicationException(string.Format("El Documento {0} ya existe en el estudio",
                                                                documento.Descripcion));
            });
            documento.Estudio = this;
            Documentos.Add(documento);
        }
        
        public virtual void EliminarDocumento(Documento documento)
        {
            Documentos.ToList<Documento>().ForEach(delegate(Documento estDocumento)
            {
                if (estDocumento == documento)
                    Documentos.Remove(estDocumento);
            });
        }

        public virtual Documento ObtenerDocumento(int idDocumento)
        {
            Documento docReturn = null;
            Documentos.ToList<Documento>().ForEach(delegate(Documento estDocumento)
            {
                if (estDocumento.Id == idDocumento)
                    docReturn = estDocumento;
            });
            return docReturn;
        }
        public virtual Documento ObtenerDocumento(string descripcion)
        {
            Documento docReturn = null;
            Documentos.ToList<Documento>().ForEach(delegate(Documento estDocumento)
            {
                if (estDocumento.Descripcion == descripcion)
                    docReturn = estDocumento;
            });
            return docReturn;
        }

        public virtual void AgregarNota(Nota nota)
        {
            nota.Estudio = this;
            Notas.Add(nota);
        }

        public virtual void EliminarNota(Nota nota)
        {
            Notas.ToList<Nota>().ForEach(delegate(Nota estNota)
            {
                if (estNota == nota)
                    Notas.Remove(estNota);
            });
        }
        public virtual Nota ObtenerNota(int idNota)
        {
            Nota notaReturn = null;
            Notas.ToList<Nota>().ForEach(delegate(Nota estNota)
            {
                if (estNota.Id == idNota)
                    notaReturn = estNota;
            });
            return notaReturn;
        }
    }
}


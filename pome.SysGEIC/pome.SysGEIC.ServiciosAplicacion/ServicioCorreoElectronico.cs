using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class ServicioCorreoElectronico
    {
        public ServicioCorreoElectronico() { }

        public void EnviarMailContactoHomePage(string datosContacto)
        {
            ServicioParametricas servParametricas = new ServicioParametricas();

            Parametro parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_HOST");
            string hostSMTP = (parms != null) ? parms.Valor : string.Empty;
            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_PUERTO");
            string puertoSMTP = (parms != null) ? parms.Valor : "0";

            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_USER");
            string userSMTP = (parms != null) ? parms.Valor : string.Empty;
            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "SMTP_PWD");
            string pwdSMTP = (parms != null) ? parms.Valor : string.Empty;

            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "REMITENTE_AVISOS");
            string remitente = (parms != null) ? parms.Valor : string.Empty;

            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "EMAIL_CONTACTOHOMEPAGE_ASUNTO");
            string asunto = (parms != null) ? parms.Valor : string.Empty;

            parms = servParametricas.ObtenerObjeto<Parametro>("Descripcion", "EMAIL_CONTACTOHOMEPAGE_DESTINATARIOS");
            string mailDestinatarios = (parms != null) ? parms.Valor : string.Empty;
            
            CorreoElectronico email = new CorreoElectronico(hostSMTP, puertoSMTP.ConvertirInt(), userSMTP, pwdSMTP);

            dynamic datosAux = ServiciosHelpers.DeserializarGenerico(datosContacto);
            
            ContactoHP contacto = new ContactoHP();
            contacto.Fecha = DateTime.Now;
            contacto.Email = datosAux.email;
            contacto.Telefono = datosAux.telefono;
            contacto.Nombre = datosAux.nombre;
            contacto.Comite = datosAux.comite;
            contacto.Mensaje = datosAux.mensaje;

            RepositoryGenerico<ContactoHP> repositoryContacto = new RepositoryGenerico<ContactoHP>();
            repositoryContacto.Actualizar(contacto);


            string[] destinatarios = mailDestinatarios.Split(';');
            email.Destinatarios.AddRange(destinatarios.ToList<string>());

            email.Remitente = remitente;
            email.Asunto = asunto;
            email.ContenidoHTML = false;
            email.Contenido = string.Format(" Se contactó: {0}\n Comité de Ética: {1}\n Email: {2}\n Teléfopno: {3}\n Mensaje:\n{4}",
                                            contacto.Nombre,
                                            contacto.Comite,
                                            contacto.Email,
                                            contacto.Telefono,
                                            contacto.Mensaje);
            email.EnviarMail();


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.ServiciosAplicacion
{
    public class CorreoElectronico
    {
        string hostSMTP;
        int puertoSMTP;
        string asunto;
        string contenido;
        bool contenidoHTML;
        string remitente;
        List<string> destinatarios;
        string usuario;
        string password;


        public List<string> Destinatarios
        {
            get { return destinatarios; }
            set { destinatarios = value; }
        }
        public string Remitente
        {
            get { return remitente; }
            set { remitente = value; }
        }
        public string Contenido
        {
            get { return contenido; }
            set { contenido = value; }
        }
        public bool ContenidoHTML
        {
            get { return contenidoHTML; }
            set { contenidoHTML = value; }
        }
        public string Asunto
        {
            get { return asunto; }
            set { asunto = value; }
        }
        public int PuertoSMTP
        {
            get { return puertoSMTP; }
            set { puertoSMTP = value; }
        }
        public string HostSMTP
        {
            get { return hostSMTP; }
            set { hostSMTP = value; }
        }
        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public CorreoElectronico()
        {
            destinatarios = new List<string>();
            remitente = string.Empty;
            hostSMTP = string.Empty;
            puertoSMTP = 0;
        }

        public CorreoElectronico(string host, int puerto, string user, string pwd)
        {
            destinatarios = new List<string>();
            remitente = string.Empty;
            hostSMTP = host;
            puertoSMTP = puerto;
            usuario = user;
            password = pwd;
        }

        public void EnviarMail()
        {
            this.Validar();

            SmtpClient conexion = new SmtpClient();
            
            conexion.Host = this.hostSMTP;
            conexion.Credentials = new NetworkCredential(this.usuario, this.password);
            if (this.puertoSMTP > 0) conexion.Port = this.puertoSMTP;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(this.remitente);

            foreach (string mailTo in this.destinatarios)
                if (mailTo.Trim().Length > 0)
                    mail.To.Add(new MailAddress(mailTo));

            mail.Subject = this.asunto;

            mail.IsBodyHtml = this.contenidoHTML;
            mail.Body = this.contenido;

            conexion.Send(mail);
        }

        private void Validar()
        {
            StringBuilder errores = new StringBuilder();

            if (hostSMTP.Trim().Length == 0)
                errores.AppendLine("No está configurado host SMTP." + Constantes.SaldoLinea);
            if (destinatarios.Count == 0)
                errores.AppendLine("Falta destinatario del correo electrónico." + Constantes.SaldoLinea);
            if (remitente.Trim().Length == 0)
                errores.AppendLine("Falta remitente del correo electrónico" + Constantes.SaldoLinea);

            if (errores.Length != 0)
                throw new ApplicationException(errores.ToString());
        }
    }
}

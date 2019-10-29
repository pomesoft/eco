using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace pome.SysGEIC.Entidades
{
    [Serializable]
    [DataContract]
    public class ListaEmails : EntidadBaseParametrica
    {
        public virtual IList<ListaEmail> ListEmails { get; set; }

        [DataMember]
        public virtual List<Email> Emails 
        {
            get
            {
                List<Email> listEmails = new List<Email>();
                ListEmails.ToList<ListaEmail>().ForEach(delegate(ListaEmail item) { listEmails.Add(item.Email); });
                return listEmails;
            }
            set { }
        }

        public ListaEmails() 
        {
            ListEmails = new List<ListaEmail>();
        }

        public void AgregarEmail(Email email)
        {
            if (ListEmails.ToList<ListaEmail>().Find(item => item.Email.Descripcion == email.Descripcion) != null)
                throw new ApplicationException("El correo electrónico ya está agregado a la lista.");

            ListaEmail listaEmail = new ListaEmail();
            listaEmail.Email = email;
            listaEmail.ListaEmails = this;
            ListEmails.Add(listaEmail);
        }
    }
}

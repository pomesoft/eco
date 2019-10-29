using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using pome.SysGEIC.Entidades;

namespace pome.SysGEIC.Entidades
{
    [DataContract]
    public class MenuDTO
    {
        [DataMember]
        public int IdMenuPrincipal { get; set; }
        [DataMember]
        public string MenuPrincipal { get; set; }
        [DataMember]
        public int IdMenuSecundario { get; set; }
        [DataMember]
        public string MenuSecundario { get; set; }
        [DataMember]
        public int IdNivelAcceso { get; set; }
        [DataMember]
        public string NivelAcceso { get; set; }

        public MenuDTO() { }

        public MenuDTO(int _idMenuPrincipal, string _menuPrincipal, int _idMenuSecundario, string _menuSecundario, int _idNivelAcceso, string _nivelAcceso) 
        {
            this.IdMenuPrincipal = _idMenuPrincipal;
            this.MenuPrincipal = _menuPrincipal;
            this.IdMenuSecundario = _idMenuSecundario;
            this.MenuSecundario = _menuSecundario;
            this.IdNivelAcceso = _idNivelAcceso;
            this.NivelAcceso = _nivelAcceso;
        }
    }
}

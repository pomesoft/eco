using System.Collections.Generic; 
using System.Text; 
using System; 


namespace pome.SysGEIC.Entidades 
{
    [Serializable]    
    public class NivelAcceso : EntidadBaseParametrica
    {
        public NivelAcceso() { }

        public NivelAcceso(int _id, string _descripcion) 
        {
            this.Id = _id;
            this.Descripcion = _descripcion;
        }
    }
}

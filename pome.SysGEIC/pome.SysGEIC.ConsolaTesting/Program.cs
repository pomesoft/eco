using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

using pome.SysGEIC.Entidades;
using pome.SysGEIC.Repositorios;

namespace pome.SysGEIC.ConsolaTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            string caracter = Console.ReadLine();

            if (caracter.Equals("menu"))
                ListarMenu();

            if (caracter.Equals("json"))
                ListarClienteJSON();

            if (caracter.Equals("TEST-ENTITY"))
                ListarTipoDocumentoFlujo();

            if (caracter == string.Empty)
            {
                ListarTipoUsuario();
                Console.Read();
                //ListarUsuarios();
                //AgregarUsuario();
                ListarUsuarios();
            }
            
            Console.ReadLine();
        }

        static void ListarUsuarios()
        {
            UsuariosRepository userRepositorio = new UsuariosRepository();

            List<Usuario> usuarios = userRepositorio.ObtenerUsuarios();

            foreach (Usuario user in usuarios)
                Console.Write(string.Format("\n{0} || {1}", user.ToString(), user.TipoUsuario.ToString()));

            Console.Write(string.Format("\nCantidad total: {0}", usuarios.Count()));
        }

        static void ListarTipoUsuario()
        {
            RepositoryGenerico<TipoUsuario> paramRepositorio = new RepositoryGenerico<TipoUsuario>();

            List<TipoUsuario> tiposUser = paramRepositorio.ObtenerTodos() as List<TipoUsuario>;

            foreach (TipoUsuario tipoUser in tiposUser)
                Console.Write(string.Format("\n{0}", tipoUser.ToString()));

            Console.Write(string.Format("\nCantidad total: {0}", tiposUser.Count()));
        }

        static void AgregarUsuario()
        {
            UsuariosRepository userRepositorio = new UsuariosRepository();
            RepositoryGenerico<TipoUsuario> paramRepositorio = new RepositoryGenerico<TipoUsuario>();

            Usuario newUser = new Usuario();
            newUser.Apellido = "Montigel";
            newUser.Nombre = "Mora";
            newUser.TipoUsuario = paramRepositorio.Obtener(1) as TipoUsuario;
            userRepositorio.ActualizarUsuario(newUser);

            Console.Write(string.Format("\nSe agregó: {0}", newUser.ToString()));
        }
        
        static void ListarMenu()
        {
            RepositoryGenerico<MenuPrincipal> menuRepositorio = new RepositoryGenerico<MenuPrincipal>();
            StringBuilder itemsHTML = new StringBuilder();
            
            List<MenuPrincipal> menuPrincipal = menuRepositorio.ObtenerTodos() as List<MenuPrincipal>;

            itemsHTML.Append(@"<ul class=""menu"">");
            menuPrincipal.ForEach(delegate(MenuPrincipal menu)
            {
                itemsHTML.Append(string.Format(@"<li><a href=#{0}>{1}</a>", menu.NavigateURL.Trim(), menu.Texto.Trim()));
                
                List<MenuSecundario> items = menu.Items.ToList<MenuSecundario>();
                if (items != null && items.Count > 0)
                {
                    itemsHTML.Append(@"<ul class=""submenu"">");
                    items.ForEach(delegate(MenuSecundario item)
                    {
                        itemsHTML.Append(string.Format(@"<li><a href=#{0}>{1}</a></li>", item.NavigateURL.Trim(), item.Texto.Trim()));
                    });
                    itemsHTML.Append(@"</ul>");
                }
                itemsHTML.Append("</li>");
            });
            itemsHTML.Append("</ul>");

            Console.Write(itemsHTML.ToString());
        }

        static void ListarClienteJSON()
        {
            List<Cliente> lista = new List<Cliente>();
            lista.Add(new Cliente(1, "NOMBRE 1", "APELLIDO 1", "11111111", "DIRECCION 1"));
            lista.Add(new Cliente(2, "NOMBRE 2", "APELLIDO 2", "11111111", "DIRECCION 1"));
            lista.Add(new Cliente(3, "NOMBRE 3", "APELLIDO 3", "11111111", "DIRECCION 1"));
            lista.Add(new Cliente(4, "NOMBRE 4", "APELLIDO 4", "11111111", "DIRECCION 1"));
            lista.Add(new Cliente(5, "NOMBRE 5", "APELLIDO 5", "11111111", "DIRECCION 1"));

            string resultado = lista.SerializaToJson();

            Console.Write(string.Format("\nlistado serializado: {0}", resultado));
        }

        static void ListarTipoDocumentoFlujo()
        {
            RepositoryGenerico<TipoDocumentoFlujo> repository = new RepositoryGenerico<TipoDocumentoFlujo>();
            List<TipoDocumentoFlujo> lista = repository.ObtenerTodosVigentes().ToList<TipoDocumentoFlujo>();

            lista.ForEach(delegate(TipoDocumentoFlujo item)
            {
                Console.Write(string.Format("\n{0}", item.ToString()));
            });
            Console.Write(string.Format("\nCantidad total: {0}", lista.Count().ToString()));
        }
       
    }

    public static class JsonSerializer
    {
        /// <summary>
        /// Método extensor para serializar JSON cualquier objeto
        /// </summary>
        public static string SerializaToJson(this object objeto)
        {
            string jsonResult = string.Empty;
            try
            {
                DataContractJsonSerializer jsonSerializer =
                  new DataContractJsonSerializer(objeto.GetType());
                MemoryStream ms = new MemoryStream();
                jsonSerializer.WriteObject(ms, objeto);
                jsonResult = Encoding.Default.GetString(ms.ToArray());
            }
            catch { throw; }
            return jsonResult;
        }
    }

    
}


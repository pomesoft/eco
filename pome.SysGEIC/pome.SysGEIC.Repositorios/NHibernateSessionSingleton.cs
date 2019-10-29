using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Type;

namespace pome.SysGEIC.Repositorios
{
    internal class NHibernateSessionSingleton
    {
        private static readonly string KEY = "NHSESSION";
        private static ISessionFactory _sessionFactory;
        private static ISession _session;

        public static string DataBase
        {
            get { return _session.Connection.Database; }
        }
        public static string DataSource
        {
            get 
            {
                string conn= _session.Connection.ConnectionString;
                string source = conn.Substring(0, conn.IndexOf(";"));
                return source.Substring(source.IndexOf("=") + 1);
            }
        }

        public static ISession GetSession()
        {   
            if (_sessionFactory == null)
                GetConfiguracion();

            if (_session != null && _session.IsOpen && !_session.IsConnected)
                _session.Reconnect();

            if (_session == null || !_session.IsOpen)
                _session = _sessionFactory.OpenSession();

                return _session;
        }

        public static void CloseSession()
        {
            if (_session != null)
                _session.Close();
        }

        protected static void GetConfiguracion()
        {
            Configuration cfg = new Configuration();
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString();

            cfg.Configure(path + @"hibernate.cfg.xml");

            cfg.AddAssembly("pome.SysGEIC.Repositorios");

            _sessionFactory = cfg.BuildSessionFactory();
        }
       
    }
}

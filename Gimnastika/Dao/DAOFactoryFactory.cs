using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Dao.NHibernate;

namespace Gimnastika.Dao
{
    class DAOFactoryFactory
    {
        public static readonly DAOFactory DAOFactory;

        static DAOFactoryFactory()
        {
            DAOFactory = new NHibernateDAOFactory();
        }
    }
}

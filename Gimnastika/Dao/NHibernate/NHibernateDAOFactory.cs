using System;
using Gimnastika.Exceptions;

namespace Gimnastika.Dao.NHibernate
{
    /**
	 * Returns NHibernate-specific instances of DAOs.
	 */

    public class NHibernateDAOFactory : DAOFactory
    {
        public override GimnasticarDAO GetGimnasticarDAO()
        {
            return new GimnasticarDAOImpl();
        }
        
        public override VezbaDAO GetVezbaDAO()
        {
            return new VezbaDAOImpl();
        }
    }
}
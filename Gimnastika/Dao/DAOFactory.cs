using Gimnastika.Dao.NHibernate;

namespace Gimnastika.Dao
{
    public abstract class DAOFactory
    {
        public abstract GimnasticarDAO GetGimnasticarDAO();
        public abstract VezbaDAO GetVezbaDAO();
        public abstract ElementVezbeDAO GetElementVezbeDAO();
        public abstract GrupaDAO GetGrupaDAO();
        public abstract ElementDAO GetElementDAO();
    }
}
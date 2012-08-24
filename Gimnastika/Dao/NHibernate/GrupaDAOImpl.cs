using System;
using System.Collections.Generic;
using NHibernate;
using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika;

namespace Gimnastika.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="GimnasticarDAO"/>.
    /// </summary>
    public class GrupaDAOImpl : GenericNHibernateDAO<Grupa, int>, GrupaDAO
    {
        #region GrupaDAO Members

        #endregion

        public override IList<Grupa> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Grupa");
                return q.List<Grupa>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

    }
}
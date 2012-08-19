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
    public class GimnasticarDAOImpl : GenericNHibernateDAO<Gimnasticar, int>, GimnasticarDAO
    {
        #region GimnasticarDAO Members

        public virtual bool postojiGimnasticar(string ime, string prezime)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from Gimnasticar g where g.Ime = :ime
                                                 and g.Prezime = :prezime");
                q.SetString("ime", ime);
                q.SetString("prezime", prezime);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }
        
        #endregion

        public override IList<Gimnasticar> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Gimnasticar");
                return q.List<Gimnasticar>();
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
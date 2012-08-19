using System;
using System.Collections.Generic;
using NHibernate;
using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika;

namespace Gimnastika.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="VezbaDAO"/>.
    /// </summary>
    public class VezbaDAOImpl : GenericNHibernateDAO<Vezba, int>, VezbaDAO
    {
        #region VezbaDAO Members

        public virtual bool existsVezbaGimnasticar(Gimnasticar g)
        {
            try
            {

                IQuery q = Session.CreateQuery("select count(*) from Vezba v where v.Gimnasticar = :gimnasticar");
                q.SetEntity("gimnasticar", g);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool postojiVezba(Sprava sprava, string naziv, Nullable<int> gimId)
        {
            throw new ArgumentException("TODO: This method is not yet implemented.");
        }
        
        #endregion

        public override IList<Vezba> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Vezba");
                return q.List<Vezba>();
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
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

                IQuery q = Session.CreateQuery(@"select count(*) from Vezba v
                                                 where v.Gimnasticar = :gimnasticar");
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

        public virtual bool postojiVezba(Sprava sprava, string naziv, Gimnasticar g)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from Vezba v
                                                 where v.Sprava = :sprava
                                                 and v.Naziv = :naziv
                                                 and v.Gimnasticar = :gimnasticar");
                q.SetByte("sprava", (Byte)sprava);
                q.SetString("naziv", naziv);
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
        
        #endregion

        public override IList<Vezba> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Vezba v
                                                 left join fetch v.Gimnasticar
                                                 left join fetch v.Pravilo");
                return q.List<Vezba>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public override Vezba FindById(int id)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Vezba v
                                                 left join fetch v.Gimnasticar
                                                 left join fetch v.Pravilo
                                                 where v.Id = :id");
                q.SetInt32("id", id);
                IList<Vezba> result = q.List<Vezba>();
                if (result.Count > 0)
                    return result[0];
                else
                    return null;
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
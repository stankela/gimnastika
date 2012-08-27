using System;
using System.Collections.Generic;
using NHibernate;
using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika;

namespace Gimnastika.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="PraviloOceneVezbeDAO"/>.
    /// </summary>
    public class PraviloOceneVezbeDAOImpl : GenericNHibernateDAO<PraviloOceneVezbe, int>, PraviloOceneVezbeDAO
    {
        #region PraviloOceneVezberDAO Members

        public virtual bool postojiPravilo(string naziv)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from PraviloOceneVezbe p
                                                 where p.Naziv = :naziv");
                q.SetString("naziv", naziv);
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

        public override IList<PraviloOceneVezbe> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select distinct p
                                                 from PraviloOceneVezbe p
                                                 left join fetch p.PocetneOceneIzvedbe");
                return q.List<PraviloOceneVezbe>();
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
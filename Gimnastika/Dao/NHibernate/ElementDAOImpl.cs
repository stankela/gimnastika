using System;
using System.Collections.Generic;
using NHibernate;
using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika;

namespace Gimnastika.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="ElementDAO"/>.
    /// </summary>
    public class ElementDAOImpl : GenericNHibernateDAO<Element, int>, ElementDAO
    {
        public override IList<Element> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select distinct e
                                                 from Element e
                                                 left join fetch e.Varijante
                                                 left join fetch e.Parent
                                                 left join fetch e.Slike
                                                 left join fetch e.VideoKlipovi");
                return q.List<Element>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public override Element FindById(int id)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Element e
                                                 left join fetch e.Varijante
                                                 left join fetch e.Parent
                                                 left join fetch e.Slike
                                                 left join fetch e.VideoKlipovi
                                                 where e.Id = :id");
                q.SetInt32("id", id);
                IList<Element> result = q.List<Element>();
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

        public virtual bool postojiElement(Sprava sprava, string naziv)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from Element e
                                                 where e.Sprava = :sprava
                                                 and e.Naziv = :naziv");
                q.SetByte("sprava", (Byte)sprava);
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

        public virtual bool postojiElementEng(Sprava sprava, string engNaziv)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from Element e
                                                 where e.Sprava = :sprava
                                                 and e.EngleskiNaziv = :engNaziv");
                q.SetByte("sprava", (Byte)sprava);
                q.SetString("engNaziv", engNaziv);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool postojiElementGim(Sprava sprava, string nazivPoGim)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from Element e
                                                 where e.Sprava = :sprava
                                                 and e.NazivPoGimnasticaru = :nazivPoGim");
                q.SetByte("sprava", (Byte)sprava);
                q.SetString("nazivPoGim", nazivPoGim);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool postojiElement(Sprava sprava, GrupaElementa grupa, short broj, byte podBroj)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*) from Element e
                                                 where e.Sprava = :sprava
                                                 and e.Grupa = :grupa
                                                 and e.Broj = :broj
                                                 and e.PodBroj = :podBroj");
                q.SetByte("sprava", (Byte)sprava);
                q.SetByte("grupa", (Byte)grupa);
                q.SetInt16("broj", broj);
                q.SetByte("podBroj", podBroj);
                return (long)q.UniqueResult() > 0;
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
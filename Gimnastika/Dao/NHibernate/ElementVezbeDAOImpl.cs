using System;
using System.Collections.Generic;
using NHibernate;
using Gimnastika.Exceptions;
using Gimnastika.Domain;
using Gimnastika;

namespace Gimnastika.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="ElementVezbeDAO"/>.
    /// </summary>
    public class ElementVezbeDAOImpl : GenericNHibernateDAO<ElementVezbe, int>, ElementVezbeDAO
    {

    }
}
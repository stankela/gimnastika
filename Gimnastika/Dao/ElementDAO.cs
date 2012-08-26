using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;
using NHibernate;

namespace Gimnastika.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Element"/> entity.
    /// </summary>
    public interface ElementDAO : GenericDAO<Element, int>
    {
        bool postojiElement(Sprava sprava, string naziv);
        bool postojiElementEng(Sprava sprava, string engNaziv);
        bool postojiElementGim(Sprava sprava, string nazivPoGim);
        bool postojiElement(Sprava sprava, GrupaElementa grupa, short broj, byte podBroj);
    }
}

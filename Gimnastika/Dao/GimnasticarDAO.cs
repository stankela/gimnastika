using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;

namespace Gimnastika.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Gimnasticar"/> entity.
    /// </summary>
    public interface GimnasticarDAO : GenericDAO<Gimnasticar, int>
    {
        bool postojiGimnasticar(string ime, string prezime);
    }
}

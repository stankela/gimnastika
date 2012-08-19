using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;

namespace Gimnastika.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Gimnasticar"/> entity.
    /// </summary>
    public interface VezbaDAO : GenericDAO<Vezba, int>
    {
        bool existsVezbaGimnasticar(Gimnasticar g);
        bool postojiVezba(Sprava sprava, string naziv, Nullable<int> gimId);
   }
}

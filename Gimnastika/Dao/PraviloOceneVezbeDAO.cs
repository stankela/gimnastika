using System;
using System.Collections.Generic;
using System.Text;
using Gimnastika.Domain;

namespace Gimnastika.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.PraviloOceneVezbe"/> entity.
    /// </summary>
    public interface PraviloOceneVezbeDAO : GenericDAO<PraviloOceneVezbe, int>
    {
        bool postojiPravilo(string naziv);
    }
}

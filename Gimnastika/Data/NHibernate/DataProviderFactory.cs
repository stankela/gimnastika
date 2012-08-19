using Gimnastika.Data;

namespace Gimnastika.Data.NHibernate
{
    public class DataProviderFactory : IDataProviderFactory
	{
        public IDataContext GetDataContext()
        {
            return new NHibernateDataContext();
        }
    }
}

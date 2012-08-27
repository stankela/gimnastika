using System.Collections.Generic;


// TODO: Smisli bolje ime za namespace u kojem je DAO implementiran na nacin iz knjige NHibernate in Action
// (Bilten.Dao i Bilten.Dao.NHibernate)
namespace Gimnastika.Dao
{
    /// <summary>
    /// An interface shared by all business data access objects.
    ///
    /// All CRUD (create, read, update, delete) basic data access operations are
    /// isolated in this interface and shared accross all DAO implementations.
    /// The current design is for a state-management oriented persistence layer
    /// (for example, there is no UDPATE statement function) that provides
    /// automatic transactional dirty checking of business objects in persistent
    /// state.
    /// </summary>
    public interface GenericDAO<T, ID>
    {
        T FindById(ID id);

        T FindByIdAndLock(ID id);

        IList<T> FindAll();

        IList<T> FindByExample(T exampleInstance, params string[] excludeProperties);

        T MakePersistent(T entity);

        void MakeTransient(T entity);

        /// <summary>
        /// Affects every managed instance in the current persistence context!
        /// </summary>
        void Flush();

        /// <summary>
        /// Affects every managed instance in the current persistence context!
        /// </summary>
        void Clear();

        void Attach(T item, bool update);
    
        void Evict(T item);
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using Gimnastika.Entities;
using Gimnastika.Exceptions;

namespace Gimnastika.Dao
{
    public abstract class DAO<T> where T : DomainObject
    {
        public virtual void insert(T entity)
        {
            string sqlGetId = "SELECT @@IDENTITY";
            SqlCeConnection conn = new SqlCeConnection(Opcije.ConnectionString);
            SqlCeCommand cmd = new SqlCeCommand(getInsertSQL(), conn);
            addInsertParameters(cmd, entity);
            SqlCeTransaction tr = null;
            try
            {
                conn.Open();
                tr = conn.BeginTransaction();
                cmd.Transaction = tr;
                int recordsAffected = cmd.ExecuteNonQuery();
                if (recordsAffected != 1)
                {
                    throw new DatabaseException(getInsertErrorMsg());
                }
                SqlCeCommand cmd2 = new SqlCeCommand(sqlGetId, conn, tr);
                object id = cmd2.ExecuteScalar();
                if (!Convert.IsDBNull(id))
                    entity.Id = Convert.ToInt32(id);
                insertDependents(entity, conn, tr);
                persistDetails(entity, null, conn, tr);
                tr.Commit(); // TODO: this can throw Exception and InvalidOperationException
            }
            catch (SqlCeException e)
            {
                // in Open()
                if (tr != null)
                    tr.Rollback(); // TODO: this can throw Exception and InvalidOperationException
                throw new DatabaseException(getInsertErrorMsg(), e);
            }
            catch (InvalidOperationException e)
            {
                // in ExecuteNonQuery(), ExecureScalar()
                if (tr != null)
                    tr.Rollback();
                throw new DatabaseException(getInsertErrorMsg(), e);
            }
            catch (DatabaseException)
            {
                // in insertDependents()
                if (tr != null)
                    tr.Rollback();
                throw;
            }
            // za svaki slucaj
            catch (Exception)
            {
                if (tr != null)
                    tr.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void update(T entity, T original)
        {
            SqlCeConnection conn = new SqlCeConnection(Opcije.ConnectionString);
            SqlCeCommand cmd = new SqlCeCommand(getUpdateSQL(), conn);
            addUpdateParameters(cmd, entity);
            SqlCeTransaction tr = null;
            try
            {
                conn.Open();
                tr = conn.BeginTransaction(); // zbog updateDependents, inace ne mora
                cmd.Transaction = tr;
                int recordsAffected = cmd.ExecuteNonQuery();
                if (recordsAffected != 1)
                {
                    // TODO: ukoliko se radi optimistic offline lock, ova grana se
                    // ce se izvrsavati ako su podaci u bazi u medjuvremenu promenjeni,
                    // pa bi trebalo izbaciti izuzetak koji bi to signalizirao
                    // (npr. NHibernate ima izuzetak StaleObjectStateException).
                    // Generalno, i kod insert i kod update i kod delete bi
                    // slucajevi gde se komanda izvrsi bez problema ali se
                    // recordsAffected razlikuje od ocekivanog
                    // trebalo da se signaliziraju razlicitim tipom izuzetka
                    throw new DatabaseException(getUpdateErrorMsg());
                }
                updateDependents(entity, conn, tr);
                persistDetails(entity, original, conn, tr);
                tr.Commit(); // TODO: this can throw Exception and InvalidOperationException
            }
            catch (SqlCeException e)
            {
                // in Open()
                if (tr != null)
                    tr.Rollback(); // TODO: this can throw Exception and InvalidOperationException
                throw new DatabaseException(getUpdateErrorMsg(), e);
            }
            catch (InvalidOperationException e)
            {
                // in ExecuteNonQuery()
                if (tr != null)
                    tr.Rollback();
                throw new DatabaseException(getUpdateErrorMsg(), e);
            }
            catch (DatabaseException)
            {
                // in updateDependents()
                if (tr != null)
                    tr.Rollback();
                throw;
            }
            catch (Exception)
            {
                if (tr != null)
                    tr.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public virtual void delete(T entity)
        {
            SqlCeConnection conn = new SqlCeConnection(Opcije.ConnectionString);
            SqlCeCommand cmd = new SqlCeCommand(getDeleteSQL(), conn);
            addDeleteParameters(cmd, entity.Id, entity);
            SqlCeTransaction tr = null;
            try
            {
                conn.Open();
                tr = conn.BeginTransaction(); // zbog updateDependents, inace ne mora
                cmd.Transaction = tr;
                int recordsAffected = cmd.ExecuteNonQuery();
                if (recordsAffected != 1)
                {
                    throw new DatabaseException(getDeleteErrorMsg());
                }
                deleteDependents(entity, conn, tr);
                deleteDetails(entity, conn, tr);
                tr.Commit(); // TODO: this can throw Exception and InvalidOperationException
            }
            catch (SqlCeException e)
            {
                // in Open()
                if (tr != null)
                    tr.Rollback(); // TODO: this can throw Exception and InvalidOperationException
                throw new DatabaseException(getDeleteErrorMsg(), e);
            }
            catch (InvalidOperationException e)
            {
                // in ExecuteNonQuery()
                if (tr != null)
                    tr.Rollback();
                throw new DatabaseException(getDeleteErrorMsg(), e);
            }
            catch (DatabaseException)
            {
                // in deleteDependents()
                if (tr != null)
                    tr.Rollback();
                throw;
            }
            catch (Exception)
            {
                if (tr != null)
                    tr.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        protected abstract string getInsertSQL();
        protected abstract void addInsertParameters(SqlCeCommand cmd, T entity);
        protected abstract string getInsertErrorMsg();
        protected virtual void insertDependents(T entity,
            SqlCeConnection conn, SqlCeTransaction tr) { }
        protected virtual void persistDetails(T entity, T original,
            SqlCeConnection conn, SqlCeTransaction tr) { }

        protected abstract string getUpdateSQL();
        protected abstract void addUpdateParameters(SqlCeCommand cmd, T entity);
        protected abstract string getUpdateErrorMsg();
        protected virtual void updateDependents(T entity, SqlCeConnection conn,
            SqlCeTransaction tr) { }


        protected abstract string getTableName();

        protected virtual string getDeleteSQL()
        {
            return "DELETE FROM " + getTableName() + " WHERE Id = @Id";
        }

        protected virtual void addDeleteParameters(SqlCeCommand cmd, int id, T entity)
        {
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        }

        protected abstract string getDeleteErrorMsg();
        protected virtual void deleteDependents(T entity,
            SqlCeConnection conn, SqlCeTransaction tr) { }
        protected virtual void deleteDetails(T entity, SqlCeConnection conn,
            SqlCeTransaction tr) { }

        // can throw DatabaseException
        public List<T> getAll()
        {
            SqlCeCommand cmd = new SqlCeCommand(getSelectAllSQL());
            SqlCeDataReader rdr = Database.executeReader(cmd, getGetManyErrorMsg());
            List<T> result = loadAll(rdr);
            rdr.Close(); // obavezno, da bi se zatvorila konekcija otvorena u executeReader
            return result;
        }

        protected virtual string getSelectAllSQL()
        {
            return "SELECT * FROM " + getTableName();
        }

        protected abstract string getGetManyErrorMsg();
        protected abstract string getGetOneErrorMsg();

        protected virtual List<T> loadAll(SqlCeDataReader rdr)
        {
            List<T> result = new List<T>();
            while (rdr.Read())
            {
                result.Add(load(rdr));
            }
            return result;
        }

        protected abstract T load(SqlCeDataReader rdr);

        // can throw DatabaseException
        public T getById(int id)
        {
            SqlCeCommand cmd = new SqlCeCommand(getSelectByIdSQL());
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            T result = null;
            if (rdr.Read())
                result = load(rdr);
            rdr.Close();
            return result;
        }

        protected virtual string getSelectByIdSQL()
        {
            return "SELECT * FROM " + getTableName() +
                " WHERE Id = @Id";
        }
    }
}

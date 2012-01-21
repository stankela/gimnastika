using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using Gimnastika.Exceptions;
using System.Data;

namespace Gimnastika.Dao
{
    public class Database
    {
        public static SqlCeDataReader executeReader(SqlCeCommand cmd, string readErrorMsg)
        {
            SqlCeConnection conn = new SqlCeConnection(Opcije.ConnectionString);
            cmd.Connection = conn;
            try
            {
                conn.Open();
                // CommandBehavior.CloseConnection znaci da kada se DataReader zatvori
                // zatvara se i konekcija
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlCeException e)
            {
                // in Open()
                conn.Close();
                throw new DatabaseException(readErrorMsg, e);
            }
            catch (InvalidOperationException e)
            {
                // in ExecuteReader()
                conn.Close();
                throw new DatabaseException(readErrorMsg, e);
            }
        }

    }
}

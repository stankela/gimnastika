using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using Gimnastika.Entities;

namespace Gimnastika.Dao
{
    public class GimnasticarDAO : DAO<Gimnasticar>
    {
        protected override string getInsertSQL()
        {
            return "INSERT INTO Gimnasticari " +
                "(Ime, Prezime) " +
                "VALUES (@Ime, @Prezime)";
        }

        protected override void addInsertParameters(SqlCeCommand cmd, Gimnasticar g)
        {
            cmd.Parameters.Add("@Ime", SqlDbType.NVarChar, 
                Gimnasticar.IME_MAX_LENGTH).Value = g.Ime;
            cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar, 
                Gimnasticar.PREZIME_MAX_LENGTH).Value = g.Prezime;
        }

        protected override string getInsertErrorMsg()
        {
            return "Neuspesan upis gimnasticara u bazu.";
        }

        protected override string getUpdateSQL()
        {
            return "UPDATE Gimnasticari " +
                "SET Ime = @Ime, Prezime = @Prezime " +
                "WHERE Id = @Id";
        }

        protected override void addUpdateParameters(SqlCeCommand cmd, Gimnasticar g)
        {
            cmd.Parameters.Add("@Ime", SqlDbType.NVarChar, Gimnasticar.IME_MAX_LENGTH).Value = g.Ime;
            cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar, Gimnasticar.PREZIME_MAX_LENGTH).Value = g.Prezime;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = g.Id;
        }

        protected override string getUpdateErrorMsg()
        {
            return "Neuspesna promena gimnasticara u bazi.";
        }

        protected override string getTableName()
        {
            return "Gimnasticari";
        }

        protected override string getDeleteErrorMsg()
        {
            return "Neuspesno brisanje gimnasticara iz baze.";
        }

        protected override string getGetManyErrorMsg()
        {
            return "Neuspesno citanje gimnasticara iz baze.";
        }

        protected override string getGetOneErrorMsg()
        {
            return getGetManyErrorMsg();
        }

        protected override Gimnasticar load(SqlCeDataReader rdr)
        {
            Gimnasticar gimnasticar;
            string ime = (string)rdr["Ime"];
            string prezime = (string)rdr["Prezime"];
            gimnasticar = new Gimnasticar(ime, prezime);
            gimnasticar.Id = (int)rdr["Id"];
            return gimnasticar;
        }

        // can throw DatabaseException
        public bool postojiGimnasticar(string ime, string prezime)
        {
            string selectByImePrezime = "SELECT * FROM Gimnasticari " +
                "WHERE Ime = @Ime AND Prezime = @Prezime";
            SqlCeCommand cmd = new SqlCeCommand(selectByImePrezime);
            cmd.Parameters.Add("@Ime", SqlDbType.NVarChar, Gimnasticar.IME_MAX_LENGTH).Value = ime;
            cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar, Gimnasticar.PREZIME_MAX_LENGTH).Value = prezime;

            SqlCeDataReader rdr = Database.executeReader(cmd, getGetOneErrorMsg());
            bool result = false;
            if (rdr.Read())
                result = true;
            rdr.Close();
            return result;
        }

    }
}

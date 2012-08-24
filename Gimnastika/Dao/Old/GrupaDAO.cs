using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

using Gimnastika.Domain;

namespace Gimnastika.Dao.Old
{
    public class GrupaDAO : DAO<Grupa>
    {
        protected override string getInsertSQL()
        {
            return "INSERT INTO Grupe (Sprava, Grupa, Naziv, EngNaziv) " +
                "VALUES (@Sprava, @Grupa, @Naziv, @EngNaziv)";
        }

        protected override void addInsertParameters(SqlCeCommand cmd, Grupa entity)
        {
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = entity.Sprava;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value 
                = entity.GrupaElemenata;
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Grupa.NAZIV_MAX_LENGTH).Value 
                = entity.Naziv;
            cmd.Parameters.Add("@EngNaziv", SqlDbType.NVarChar, Grupa.NAZIV_MAX_LENGTH).Value 
                = entity.EngNaziv;
        }

        protected override string getInsertErrorMsg()
        {
            return "Neuspesan upis grupe u bazu.";
        }

        protected override string getUpdateSQL()
        {
            return "UPDATE Grupe SET Naziv = @Naziv, EngNaziv = @EngNaziv " +
                "WHERE Sprava = @Sprava AND Grupa = @Grupa";
        }

        protected override void addUpdateParameters(SqlCeCommand cmd, Grupa entity)
        {
            cmd.Parameters.Add("@Naziv", SqlDbType.NVarChar, Grupa.NAZIV_MAX_LENGTH).Value
                = entity.Naziv;
            cmd.Parameters.Add("@EngNaziv", SqlDbType.NVarChar, Grupa.NAZIV_MAX_LENGTH).Value
                = entity.EngNaziv;
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = entity.Sprava;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value
                = entity.GrupaElemenata;
        }

        protected override string getUpdateErrorMsg()
        {
            return "Neuspesna promena grupe u bazi.";
        }

        protected override string getTableName()
        {
            return "Grupe";
        }

        protected override string getDeleteSQL()
        {
            return "DELETE FROM " + getTableName() + 
                " WHERE Sprava = @Sprava AND Grupa = @Grupa";
        }

        protected override void addDeleteParameters(SqlCeCommand cmd, int id, Grupa entity)
        {
            cmd.Parameters.Add("@Sprava", SqlDbType.TinyInt).Value = entity.Sprava;
            cmd.Parameters.Add("@Grupa", SqlDbType.TinyInt).Value
                = entity.GrupaElemenata;
        }

        protected override string getDeleteErrorMsg()
        {
            return "Neuspesno brisanje grupe u bazi.";
        }

        protected override string getGetManyErrorMsg()
        {
            return "Neuspesno citanje grupa iz baze.";
        }

        protected override string getGetOneErrorMsg()
        {
            return "Neuspesno citanje grupe iz baze.";
        }

        protected override Grupa load(SqlCeDataReader rdr)
        {
            Sprava sprava = (Sprava)(byte)rdr["Sprava"];
            GrupaElementa grupa = (GrupaElementa)(byte)rdr["Grupa"];
            string naziv = (string)rdr["Naziv"];
            string engNaziv = (string)rdr["EngNaziv"];
            return new Grupa(sprava, grupa, naziv, engNaziv);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasininisMokymas
{
    public class Database
    {
        SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=Zmogus.db; Version = 3; New = True; Compress = True; ");
        public List<Human> GetData()
        {
            sqlite_conn.Open();
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "Select * From Pacientas";
            SQLiteDataReader sQLiteDataReader = sqlite_cmd.ExecuteReader();
            List<Human> man = new List<Human>();
            while (sQLiteDataReader.Read())
            {
                man.Add(new Human { id = sQLiteDataReader.GetInt32(0), vardas = sQLiteDataReader.GetString(1), lytis = sQLiteDataReader.GetInt32(2), ugis = sQLiteDataReader.GetInt32(3), svoris = sQLiteDataReader.GetInt32(4) });
            }
            sqlite_conn.Close();
            return man;
        }
    }
}

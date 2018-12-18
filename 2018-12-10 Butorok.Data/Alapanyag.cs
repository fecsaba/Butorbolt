using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butorok.Data
{
    public class Alapanyag
    {
        public int? Id { get; set; }
        public string Megnevezes { get; set; }

        public Alapanyag()
        { }

        public Alapanyag(MySqlDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.Megnevezes = reader["Megnevezes"].ToString();
        }

        public static List<Alapanyag> Select()
        {
            List<Alapanyag> lista = new List<Alapanyag>()
            {
                new Alapanyag()
            };
            string conString = ConfigurationManager.ConnectionStrings["butorok"].ConnectionString;
            using (var con = new MySqlConnection(conString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("Alapanyagok_Select", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(new Alapanyag(reader));
                        return lista;
                    }
                }
            }
        }
    }
}


using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butorok.Data
{
    public class Butor
    {
        public int Id { get; set; }
        public string Megnevezes { get; set; }
        public int AlapanyagId { get; set; }
        public string Meret { get; set; }
        public decimal? Ar { get; set; }

        public string AlapanyagNev { get; set; }

        public Butor()
        {}

        public Butor(MySqlDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.Megnevezes = reader["Megnevezes"].ToString();
            this.AlapanyagId = Convert.ToInt32(reader["AlapanyagId"]);
            this.Meret = reader["Meret"].ToString();
            this.Ar = reader["Ar"] == DBNull.Value ? 
                      null : 
                      (decimal?)Convert.ToDecimal(reader["Ar"]);
            this.AlapanyagNev = reader["AlapanyagNev"].ToString();
        }

        private static string conString = ConfigurationManager.ConnectionStrings["butorok"].ConnectionString;

        public static Butor Get(int id)
        {
            using (var con = new MySqlConnection(conString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("Butorok_Select", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_id", id);
                    cmd.Parameters.AddWithValue("_megnevezes", null);
                    cmd.Parameters.AddWithValue("_alapanyagid", null);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Butor(reader);
                        else
                            return null;
                    }
                }
            }
        }

        public static List<Butor> Select(string megnevezes, int? alapanyagid)
        {
            List<Butor> lista = new List<Butor>();
            using (var con = new MySqlConnection(conString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("Butorok_Select", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_id", null);
                    cmd.Parameters.AddWithValue("_megnevezes", megnevezes);
                    cmd.Parameters.AddWithValue("_alapanyagid", alapanyagid);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(new Butor(reader));
                        return lista;
                    }
                }
            }
        }

        private static void SetParams(MySqlParameterCollection parameters, Butor model)
        {
            parameters.AddWithValue("_megnevezes", model.Megnevezes);
            parameters.AddWithValue("_meret", model.Meret);
            parameters.AddWithValue("_alapanyagId", model.AlapanyagId);
            parameters.AddWithValue("_ar", model.Ar);
        }

        public static Butor Insert(Butor model)
        {
            using (var con = new MySqlConnection(conString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("Butorok_Insert", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SetParams(cmd.Parameters, model);
                    using (var reader = cmd.ExecuteReader())
                        if (reader.Read())
                            return new Butor(reader);
                        else
                            return null;
                }
            }
        }

        public static Butor Update(Butor model)
        {
            using (var con = new MySqlConnection(conString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("Butorok_Update", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SetParams(cmd.Parameters, model);
                    cmd.Parameters.AddWithValue("_id", model.Id);
                    using (var reader = cmd.ExecuteReader())
                        if(reader.Read())
                            return new Butor(reader);
                        else
                            return null;
                }
            }
        }

        public static void Delete(Butor model)
        {
            using (var con = new MySqlConnection(conString))
            {
                con.Open();
                using (var cmd = new MySqlCommand("Butorok_Delete", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_id", model.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

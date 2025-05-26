using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace SAE_S2._02
{
    public class BD
    {
        private static MySqlConnection conn;

        public static MySqlConnection Conn { get => conn; set => conn = value; }

        public static void Ouverture()
        {
            string serveur = "10.1.139.236";
            string login = "d3";
            string mdp = "based3";
            string bd = "based3";

            string chaineConnexion = $"Server={serveur};Database={bd};Uid={login};Pwd={mdp}";
            conn = new MySqlConnection(chaineConnexion);
            conn.Open();
        }

        public static void Fermeture()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static void LectureNomArret(ref List<string> nomArrets, ref List<Tuple<double, double>> positionArrets, int nb_arrets)
        {
            string reqSQL = $"SELECT nom_arret, latitude_arret, longitude_arret FROM Arret;";
            MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nomArrets.Add(reader.GetString(0));
                positionArrets.Add(new Tuple<double, double>(reader.GetDouble(1), reader.GetDouble(2)));
            }
            reader.Close();

        }
    }

}

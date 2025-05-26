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

        public static void LectureNomArret(ref List<string> nomArrets, int nb_arrets)
        {
            for (int i = 0; i <= nb_arrets; i++)
            {
                string reqSQL = $"SELECT nom_arret FROM Arret WHERE id_arret = {i};";
                MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nomArrets.Add(reader.GetString(0));
                }
                reader.Close();
            }

        }

        public static void LecturePosition(ref List<Tuple<double, double>> positionArrets, int nb_arrets)
        {
            for (int i = 0; i <= nb_arrets; i++)
            {
                string reqSQL = $"SELECT latitude_arret, longitude_arret FROM Arret WHERE id_arret = {i};";
                MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    positionArrets.Add(new Tuple<double, double>(reader.GetDouble(0), reader.GetDouble(1)));
                }
                reader.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Policy;
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
            // Connexion à la base de données MySQL
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
            // Fermeture de la connexion à la base de données
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static void LectureNomArret(ref List<string> nomArrets, ref List<Tuple<double, double>> positionArrets, int nb_arrets, ref List<int> idArret)
        {
            // Lecture des noms et positions des arrêts depuis la base de données
            string reqSQL = $"SELECT nom_arret, latitude_arret, longitude_arret, id_arret FROM Arret;";
            MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nomArrets.Add(reader.GetString(0));
                positionArrets.Add(new Tuple<double, double>(reader.GetDouble(1), reader.GetDouble(2)));
                idArret.Add(reader.GetInt32(3));
            }
            reader.Close();
        }

        public static int LectureNombreArret()
        {
            // Lecture du nombre d'arrêts dans la base de données
            string reqSQL = "SELECT COUNT(*) FROM Arret;";
            MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
            int nombreArrets = Convert.ToInt32(cmd.ExecuteScalar());
            return nombreArrets;
        }

        public static Dictionary<int, List<int>> LectureSuivant()
        {
            // Lecture des successeurs des arrêts depuis la base de données
            Dictionary<int, List<int>> listeArretsAdjacents = new Dictionary<int, List<int>>();
            string reqSQL = $"SELECT * FROM Suivant;";
            MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!listeArretsAdjacents.ContainsKey(reader.GetInt32(0)))
                {
                    listeArretsAdjacents[reader.GetInt32(0)] = new List<int>();
                    listeArretsAdjacents[reader.GetInt32(0)].Add(reader.GetInt32(1));
                }
                else
                {
                    listeArretsAdjacents[reader.GetInt32(0)].Add(reader.GetInt32(1));
                }
            }
            reader.Close();
            return listeArretsAdjacents;
        }
        public static Dictionary<int, List<int>> LecturePredecesseur()
        {
            // Lecture des prédécesseurs des arrêts depuis la base de données
            Dictionary<int, List<int>> ArretByID = new Dictionary<int, List<int>>();
            string reqSQL = $"SELECT * FROM Suivant;";
            MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!ArretByID.ContainsKey(reader.GetInt32(1)))
                {
                    ArretByID[reader.GetInt32(1)] = new List<int>();
                    ArretByID[reader.GetInt32(1)].Add(reader.GetInt32(0));
                }
                else
                {
                    ArretByID[reader.GetInt32(1)].Add(reader.GetInt32(0));
                }
            }
            reader.Close();
            return ArretByID;
        }

        public static Dictionary<int, List<int>> LectureCroisement()
        {
            // Lecture des prédécesseurs des arrêts depuis la base de données
            Dictionary<int, List<int>> ArretByLigne = new Dictionary<int, List<int>>();
            string reqSQL = $"SELECT * FROM Croisement;";
            MySqlCommand cmd = new MySqlCommand(reqSQL, BD.Conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (!ArretByLigne.ContainsKey(reader.GetInt32(0)))
                {
                    ArretByLigne[reader.GetInt32(0)] = new List<int>();
                    ArretByLigne[reader.GetInt32(0)].Add(reader.GetInt32(1));
                }
                else
                {
                    ArretByLigne[reader.GetInt32(0)].Add(reader.GetInt32(1));
                }
            }
            reader.Close();
            return ArretByLigne;
        }
    }

}

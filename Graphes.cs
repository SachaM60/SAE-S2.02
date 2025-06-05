using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_S2._02
{
    /// <summary>
    /// Classe représentant un graphe de transports en commun, contenant des arrêts et permettant de calculer le chemin le plus court entre deux arrêts.
    /// </summary>
    public class Graphes
    {
        private Dictionary<int, Arret> arrets;

        public Graphes(Dictionary<int, Arret> arrets)
        {
            this.arrets = arrets;
        }

        public Dictionary<int, Arret> Arrets { get => arrets; set => arrets = value; }

        public string Djikstra(int idDepart, int idArrivee, Dictionary<int, bool> arretFavoris)
        {
            if (!arrets.ContainsKey(idDepart) || !arrets.ContainsKey(idArrivee))
                return "Erreur : arrêt de départ ou d’arrivée introuvable.";

            Arret depart = arrets[idDepart];
            Arret arrivee = arrets[idArrivee];

            var distances = new Dictionary<int, double>();
            var precedent = new Dictionary<int, int?>();
            var nonVisites = new HashSet<int>(arrets.Keys);

            foreach (var id in arrets.Keys)
            {
                distances[id] = double.MaxValue;
                precedent[id] = null;
            }

            distances[idDepart] = 0;

            while (nonVisites.Count > 0)
            {
                int u = nonVisites.OrderBy(id => distances[id]).First();

                if (u == idArrivee || distances[u] == double.MaxValue)
                    break;

                nonVisites.Remove(u);

                foreach (var voisin in arrets[u].Successeurs)
                {
                    int vId = arrets.FirstOrDefault(pair => pair.Value == voisin.Arret).Key;

                    // Favoriser les arrêts favoris : appliquer une réduction sur la distance
                    double bonus = (arretFavoris.TryGetValue(vId, out bool estFavori) && estFavori) ? 0.9 : 1.0;
                    double alt = distances[u] + (voisin.Distance * bonus);

                    if (alt < distances[vId])
                    {
                        distances[vId] = alt;
                        precedent[vId] = u;
                    }
                }
            }

            if (distances[idArrivee] == double.MaxValue)
                return $"Aucun chemin trouvé de {depart.Nom} à {arrivee.Nom}.";

            // Reconstruction du chemin
            var chemin = new Stack<int>();
            int? current = idArrivee;

            while (current != null)
            {
                chemin.Push(current.Value);
                current = precedent[current.Value];
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Temps de trajet de {depart.Nom} à {arrivee.Nom} : {Math.Round(distances[idArrivee] * 60, 0)} minutes.");
            sb.Append("Chemin : ");

            while (chemin.Count > 0)
            {
                int id = chemin.Pop();
                sb.Append(arrets[id].Nom);
                if (chemin.Count > 0)
                    sb.Append(" -> ");
            }

            return sb.ToString();
        }




        public override string ToString()
        {
            /// <summary>
            /// Méthode pour afficher le graphe
            /// </summary>
            string graphes;
            if (arrets != null && arrets.Count > 0)
            {
                graphes = "Graphe:\n";
                foreach (var arret in arrets)
                {
                    graphes += arret.ToString() + "\n";
                }
            }
            else
            {
                graphes = "Aucun arrêt dans le graphe.";
            }
            return graphes;
        }
    }
}

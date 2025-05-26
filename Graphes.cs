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
        private List<Arret> arrets;

        public Graphes(List<Arret> arrets)
        {
            this.arrets = arrets;
        }

        public List<Arret> Arrets { get => arrets; set => arrets = value; }

        public string Djikstra(Arret depart, Arret arrivee)
        {
            var distances = new Dictionary<Arret, double>();
            var precedent = new Dictionary<Arret, Arret>();
            var nonVisites = new List<Arret>(arrets);

            // Initialisation
            foreach (var arret in arrets)
            {
                distances[arret] = double.MaxValue;
                precedent[arret] = null;
            }

            distances[depart] = 0;

            while (nonVisites.Count > 0)
            {
                // Trouver l'arrêt non visité avec la distance minimale
                var u = nonVisites.OrderBy(a => distances[a]).First();

                if (u == arrivee)
                    break;

                nonVisites.Remove(u);

                // Parcourir les successeurs
                foreach (var voisin in u.Successeurs)
                {
                    var v = voisin.Arret;
                    var alt = distances[u] + voisin.Distance;

                    if (alt < distances[v])
                    {
                        distances[v] = alt;
                        precedent[v] = u;
                    }
                }
            }

            // Construction du chemin
            if (distances[arrivee] == double.MaxValue)
                return $"Aucun chemin trouvé de {depart.Nom} à {arrivee.Nom}.";

            var chemin = new Stack<Arret>();
            var courant = arrivee;

            while (courant != null)
            {
                chemin.Push(courant);
                courant = precedent[courant];
            }

            var resultat = new StringBuilder();
            resultat.AppendLine($"Distance minimale de {depart.Nom} à {arrivee.Nom} : {distances[arrivee]}");
            resultat.Append("Chemin : ");

            while (chemin.Count > 0)
            {
                var a = chemin.Pop();
                resultat.Append(a.Nom);
                if (chemin.Count > 0)
                    resultat.Append(" -> ");
            }

            return resultat.ToString();
        }


        public override string ToString()
        {
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

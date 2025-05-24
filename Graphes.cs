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
            // Implémentation de l'algorithme de Dijkstra
            // Pour l'instant, on retourne une chaîne vide
            return string.Empty;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_S2._02
{
    /// <summary>
    /// Classe représentant un arrêt de transport en commun, avec ses coordonnées, ses prédécesseurs et ses successeurs.
    /// </summary>
    public class Arret
    {
        private string nom;
        private double x;
        private double y;
        private List<ArretAdjacent> predecesseurs;
        private List<ArretAdjacent> successeurs;

        public Arret(string nom, double x, double y, List<ArretAdjacent> predecesseurs, List<ArretAdjacent> successeurs)
        {
            this.nom = nom;
            this.x = x;
            this.y = y;
            this.predecesseurs = predecesseurs;
            this.successeurs = successeurs;
        }

        public string Nom { get => nom; set => nom = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public List<ArretAdjacent> Predecesseurs { get => predecesseurs; set => predecesseurs = value; }
        public List<ArretAdjacent> Successeurs { get => successeurs; set => successeurs = value; }

        public void Add_successeur(ArretAdjacent arretAdjacent)
        {
            if (arretAdjacent != null)
            {
                this.successeurs.Add(arretAdjacent);
            }
        }

        public void Add_predecesseur(ArretAdjacent arretAdjacent)
        {
            if (arretAdjacent != null)
            {
                this.predecesseurs.Add(arretAdjacent);
            }
        }

        public override string ToString()
        {
            return $"Arret: {nom}, Position: ({x}, {y}), Predecesseurs: [{string.Join(", ", predecesseurs.Select(a => a.Arret.Nom))}], Successeurs: [{string.Join(", ", successeurs.Select(a => a.Arret.Nom))}]";
        }
    }
}

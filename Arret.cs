﻿using System;
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
        private int id_arret;

        public Arret(string nom, double x, double y, List<ArretAdjacent> predecesseurs, List<ArretAdjacent> successeurs, int id_arret)
        {
            // Constructeur de la classe Arret, initialise les propriétés de l'arrêt
            this.nom = nom;
            this.x = x;
            this.y = y;
            this.predecesseurs = predecesseurs;
            this.successeurs = successeurs;
            this.id_arret = id_arret;
        }

        public string Nom { get => nom; set => nom = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public List<ArretAdjacent> Predecesseurs { get => predecesseurs; set => predecesseurs = value; }
        public List<ArretAdjacent> Successeurs { get => successeurs; set => successeurs = value; }
        public int Id_arret { get => id_arret; set => id_arret = value; }

        public void Add_successeur(ArretAdjacent arretAdjacent)
        {
            // Ajoute un successeur à la liste des successeurs de l'arrêt, si l'arrêt adjacent n'est pas null
            if (arretAdjacent != null)
            {
                this.successeurs.Add(arretAdjacent);
            }
        }

        public void Add_predecesseur(ArretAdjacent arretAdjacent)
        {
            // Ajoute un prédécesseur à la liste des prédécesseurs de l'arrêt, si l'arrêt adjacent n'est pas null
            if (arretAdjacent != null)
            {
                this.predecesseurs.Add(arretAdjacent);
            }
        }

        public override string ToString()
        {
            // Retourne une représentation en chaîne de caractères de l'arrêt, incluant son nom, sa position, ses prédécesseurs et successeurs
            return $"Arret: {nom}, Position: ({x}, {y}), Predecesseurs: [{string.Join(", ", predecesseurs.Select(a => a.Arret.Nom))}], Successeurs: [{string.Join(", ", successeurs.Select(a => a.Arret.Nom))}], Identifiant : {Id_arret}";
        }

        public double DistanceVers(Arret autre)
        {
            const double R = 6371.0; // Rayon de la Terre en kilomètres

            // Conversion des degrés en radians
            double lat1 = DegreesToRadians(this.y);
            double lon1 = DegreesToRadians(this.x);
            double lat2 = DegreesToRadians(autre.y);
            double lon2 = DegreesToRadians(autre.x);

            double dlat = lat2 - lat1;
            double dlon = lon2 - lon1;

            double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dlon / 2) * Math.Sin(dlon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}

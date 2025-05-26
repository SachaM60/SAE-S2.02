// See https://aka.ms/new-console-template for more information
using MySql.Data.MySqlClient;
using SAE_S2._02;
using System.Collections.Generic;

BD.Ouverture();
int[,] construction_matrice(List<Arret> arrets)
{
    /// Fonction qui sert à construire une matrice d'adjacence à partir d'une liste d'arrêts.
    int n = arrets.Count;
    int[,] matrice = new int[n, n];
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            matrice[i, j] = int.MaxValue; // Initialisation à l'infini
        }
    }
    for (int i = 0; i < n; i++)
    {
        foreach (ArretAdjacent successeur in arrets[i].Successeurs)
        {
            int j = arrets.IndexOf(successeur.Arret);
            matrice[i, j] = (int)successeur.Distance; // On met la distance dans la matrice
        }
    }
    return matrice;
}

int nbarrets = 274; 
List<string> nom = new List<string>(nbarrets);  
List<Tuple<double, double>> pos = new List<Tuple<double, double>>(nbarrets);

BD.LectureNomArret(ref nom, ref pos, nbarrets);

List<Arret> vraisArrets = new List<Arret>(nbarrets);

for (int i =0 ; i < nbarrets; i++)
{
    vraisArrets.Add(new Arret(nom[i], pos[i].Item1, pos[i].Item2, new List<ArretAdjacent>(), new List<ArretAdjacent>()));
}

Console.WriteLine($"{vraisArrets.Last()}");

Console.WriteLine($"{vraisArrets.Count} arrêts chargés depuis la base de données.");

Arret arret1 = new Arret("Arret1", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret2 = new Arret("Arret2", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret3 = new Arret("Arret3", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret4 = new Arret("Arret4", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret5 = new Arret("Arret5", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret6 = new Arret("Arret6", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());


arret1.Add_predecesseur(new ArretAdjacent(arret3, 5.0, new List<int> { 1 }));
arret1.Add_predecesseur(new ArretAdjacent(arret6, 12.0, new List<int> { 1 }));
arret1.Add_successeur(new ArretAdjacent(arret4, 3.0, new List<int> { 1 }));

arret2.Add_predecesseur(new ArretAdjacent(arret3, 15.0, new List<int> { 1 }));

arret3.Add_predecesseur(new ArretAdjacent(arret4, 19.0, new List<int> { 1 }));
arret3.Add_successeur(new ArretAdjacent(arret1, 5.0, new List<int> { 1 }));
arret3.Add_successeur(new ArretAdjacent(arret2, 15.0, new List<int> { 1 }));
arret3.Add_successeur(new ArretAdjacent(arret5, 10.0, new List<int> { 1 }));
arret3.Add_successeur(new ArretAdjacent(arret6, 20.0, new List<int> { 1 }));

arret4.Add_predecesseur(new ArretAdjacent(arret1, 3.0, new List<int> { 1 }));
arret4.Add_successeur(new ArretAdjacent(arret3, 19.0, new List<int> { 1 }));

arret5.Add_predecesseur(new ArretAdjacent(arret3, 10.0, new List<int> { 1 }));

arret6.Add_predecesseur(new ArretAdjacent(arret3, 20.0, new List<int> { 1 }));
arret6.Add_successeur(new ArretAdjacent(arret1, 12.0, new List<int> { 1 }));

List<Arret> arrets = new List<Arret>
{
    arret1, arret2, arret3, arret4, arret5, arret6
};

int[,] matrice = construction_matrice(arrets);
Console.WriteLine($"Matrice d'adjacence des arrêts :");

for (int i = 0; i < matrice.GetLength(0); i++)
{
    for (int j = 0; j < matrice.GetLength(1); j++)
    {
        if (matrice[i, j] == int.MaxValue)
            Console.Write("inf\t");
        else
            Console.Write($"{matrice[i, j]}\t");
    }
    Console.WriteLine();
}

Graphes Reseau = new Graphes(arrets);

Console.WriteLine(Reseau.ToString());

Arret arret_actuel = arret1;
Arret arret_stop = arret6;
string chemin="";


chemin = Reseau.Djikstra(arret_actuel, arret_stop);

Console.WriteLine($"Chemin le plus court de {arret1.Nom} à {arret_stop.Nom}: {chemin}");
BD.Fermeture();
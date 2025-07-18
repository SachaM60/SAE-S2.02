﻿// See https://aka.ms/new-console-template for more information
using MySql.Data.MySqlClient;
using SAE_S2._02;
using System.Collections.Generic;

BD.Ouverture();
int[,] ConstructionMatrice(Dictionary<int, Arret> arretByID)
{
    /// <summary>
    /// Cette méthode construit une matrice d'adjacence à partir d'un dictionnaire d'arrêts.
    /// </summary>
    int n = arretByID.Count;
    int[,] matrice = new int[n, n];

    // Crée un mapping ID -> index (0 à n-1)
    Dictionary<int, int> idToIndex = new Dictionary<int, int>();
    Dictionary<int, int> indexToId = new Dictionary<int, int>();
    int idx = 0;
    foreach (var id in arretByID.Keys)
    {
        idToIndex[id] = idx;
        indexToId[idx] = id;
        idx++;
    }

    // Initialiser la matrice avec "infini"
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            matrice[i, j] = int.MaxValue;
        }
    }

    // Remplir les distances des successeurs
    foreach (var pair in arretByID)
    {
        int i = idToIndex[pair.Key];
        foreach (var succ in pair.Value.Successeurs)
        {
            // Chercher l’ID du successeur
            int succId = arretByID.FirstOrDefault(x => x.Value == succ.Arret).Key;
            if (idToIndex.ContainsKey(succId))
            {
                int j = idToIndex[succId];
                matrice[i, j] = (int)succ.Distance;
            }
        }
    }

    return matrice;
}

// Lecture du nombre d'arrêts dans la base de données
int nbarrets = BD.LectureNombreArret();

// Initialisation des listes pour stocker les noms et positions des arrêts
List<string> nom = new List<string>(nbarrets);  

List<Tuple<double, double>> pos = new List<Tuple<double, double>>(nbarrets);

// Liste pour stocker les IDs des arrêts
List<int> idArret = new List<int>(nbarrets);

// Dictionnaire pour stocker les arrêts par ID
Dictionary<int, Arret> ArretByID = new Dictionary<int, Arret>();

// Initialize the dictionary ArretByIsFavoris before its usage to fix CS0165 error.  
Dictionary<int, bool> ArretByIsFavoris = new Dictionary<int, bool>();

// Lecture des successeurs et prédécesseurs
Dictionary<int, List<int>> SuccesseursbyId = BD.LectureSuivant();

Dictionary<int, List<int>> PredecesseursById = BD.LecturePredecesseur();

//Lecture des lignes passant par chaque arrêt

Dictionary<int, List<int>> ArretByLigne = BD.LectureCroisement();

// Lecture des noms et positions des arrêts
BD.LectureNomArret(ref nom, ref pos, nbarrets, ref idArret);


////Création des arrêts à partir des données lues
for (int i =0 ; i < nbarrets; i++)
{
    ArretByID.Add(idArret[i], new Arret(nom[i], pos[i].Item1, pos[i].Item2, new List<ArretAdjacent>(), new List<ArretAdjacent>(), idArret[i]));
}

//Ajout des successeurs
foreach (int id in SuccesseursbyId.Keys)
{
    Arret arret = ArretByID[id];
    foreach (int adjacentId in SuccesseursbyId[id])
    {
        double distance = arret.DistanceVers(ArretByID[adjacentId]); // Calcul de la distance entre les arrêts
        arret.Add_successeur(new ArretAdjacent(ArretByID[adjacentId], distance/30, ArretByLigne[id])); 
    }
}

//Ajout des prédécesseurs
foreach (int id in PredecesseursById.Keys)
{
    Arret arret = ArretByID[id];
    foreach (int adjacentId in PredecesseursById[id])
    {
        double distance = arret.DistanceVers(ArretByID[adjacentId]); // Calcul de la distance entre les arrêts
        arret.Add_predecesseur(new ArretAdjacent(ArretByID[adjacentId], distance/30, ArretByLigne[id])); 
    }
}



//Création de la matrice d'adjacence
int[,] matrice = ConstructionMatrice(ArretByID);


//Création du graphe à partir des arrêts
Graphes Reseau = new Graphes(ArretByID);

//Définition des variables pour les arrêts de départ et d'arrivée
Arret arret_actuel;
string nom_arret_actuel;
Arret arret_stop;
string nom_arret_stop;
string chemin="";

bool continuer = true;

while (continuer) { 
    Console.WriteLine("Bienvenue dans le système de transport en commun !\n");
    Console.WriteLine("1. Trouver le chemin le plus court entre deux arrêts");
    Console.WriteLine("2. Afficher les arrêts favoris");
    Console.WriteLine("3. Ajouter un arrêt aux favoris");
    Console.WriteLine("4. Quitter");
    int choix = 0;
    while (choix < 1 || choix > 4)
    {
        Console.WriteLine("Veuillez entrer votre choix (1-4) :");
        if (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > 4)
        {
            Console.WriteLine("Choix invalide, veuillez réessayer.");
            choix = 0;
        }
    }
   
    if (choix == 1)
    {
        //Saisie de l'arret de départ et vérifications
        Console.WriteLine("Entrez le nom de l'arrêt de départ :");
        nom_arret_actuel = Console.ReadLine()!;
        arret_actuel = Reseau.Arrets.Values.FirstOrDefault(a => a.Nom.Equals(nom_arret_actuel, StringComparison.OrdinalIgnoreCase))!;
        while (arret_actuel == null)
        {
            Console.WriteLine("Arrêt de départ non trouvé.");
            Console.WriteLine("Veuillez réessayer.");
            nom_arret_actuel = Console.ReadLine()!;
            arret_actuel = Reseau.Arrets.Values.FirstOrDefault(a => a.Nom.Equals(nom_arret_actuel, StringComparison.OrdinalIgnoreCase))!;
        }

        //Saisie de l'arret d'arrivée et vérifications
        Console.WriteLine("Entrez le nom de l'arrêt d'arrivée :");
        nom_arret_stop = Console.ReadLine()!;
        arret_stop = Reseau.Arrets.Values.FirstOrDefault(a => a.Nom.Equals(nom_arret_stop, StringComparison.OrdinalIgnoreCase))!;
        while (arret_stop == null)
        {
            Console.WriteLine("Arrêt d'arrivée non trouvé.");
            Console.WriteLine("Veuillez réessayer.");
            nom_arret_stop = Console.ReadLine()!;
            arret_stop = Reseau.Arrets.Values.FirstOrDefault(a => a.Nom.Equals(nom_arret_stop, StringComparison.OrdinalIgnoreCase))!;
        }


        //Appel de la méthode Djikstra pour trouver le chemin le plus court
        chemin = Reseau.Djikstra(arret_actuel!.Id_arret, arret_stop!.Id_arret, ArretByIsFavoris);


        //Affichage du chemin
        Console.WriteLine($"{chemin}");
    }

    if (choix == 2)
    {
        //Affichage des arrêts favoris
        Console.WriteLine("Arrêts favoris :");

        foreach (var arret in ArretByID.Values)
        {
            if (ArretByIsFavoris.TryGetValue(arret.Id_arret, out bool value) && value)
            {
                Console.WriteLine(arret.Nom);
            }
        }
    }

    if (choix == 3)
    {
        //Ajout d'un arrêt aux favoris
        Console.WriteLine("Entrez le nom de l'arrêt à ajouter aux favoris :");
        string nomArretFavori = Console.ReadLine()!;
        Arret arretFavori = Reseau.Arrets.Values.FirstOrDefault(a => a.Nom.Equals(nomArretFavori, StringComparison.OrdinalIgnoreCase))!;
        if (arretFavori != null)
        {
            if (!ArretByIsFavoris.ContainsKey(arretFavori.Id_arret))
            {
                ArretByIsFavoris[arretFavori.Id_arret] = true;
                Console.WriteLine($"{arretFavori.Nom} a été ajouté aux favoris.");
            }
            else
            {
                Console.WriteLine($"{arretFavori.Nom} est déjà dans les favoris.");
            }
        }
        else
        {
            Console.WriteLine("Arrêt non trouvé.");
        }
    }


    if (choix == 4)
    {
        continuer = false;
        Console.WriteLine("Merci d'avoir utilisé le système de transport en commun. Au revoir !");
        break;
    }
}


BD.Fermeture();
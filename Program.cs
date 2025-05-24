// See https://aka.ms/new-console-template for more information
using SAE_S2._02;
using System.Collections.Generic;

Arret arret1 = new Arret("Arret1", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret2 = new Arret("Arret2", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());
Arret arret3 = new Arret("Arret3", 10.0, 20.0, new List<ArretAdjacent>(), new List<ArretAdjacent>());


arret1.Add_successeur(new ArretAdjacent(arret2, 5.0, new List<int> { 1 }));
arret1.Add_successeur(new ArretAdjacent(arret3, 15.0, new List<int> { 2 }));
arret2.Add_successeur(new ArretAdjacent(arret3, 10.0, new List<int> { 1 }));

Console.WriteLine($"Arret1: {arret1.Nom}, Position: ({arret1.X}, {arret1.Y}), Predecesseurs : {arret1.Predecesseurs.ToString()}, Successeurs : {string.Join(',', arret1.Successeurs)}");
Console.WriteLine($"Arret2: {arret2.Nom}, Position: ({arret2.X}, {arret2.Y}), Predecesseurs : {arret2.Predecesseurs.ToString()}, Successeurs : {arret2.Successeurs[0].ToString()}");
//Console.WriteLine($"Arret3: {arret3.Nom}, Position: ({arret3.X}, {arret3.Y}), Predecesseurs : {arret3.Predecesseurs.ToString()}, Successeurs : {arret3.Successeurs[0].Arret.ToString()}");
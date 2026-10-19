# Vous devez à l’aide de l’IA de votre choix générer deux fichiers CSV :
    - client.csv (Nom@Prénom@Date de naissance@Téléphone@Email)
    - voiture.csv(Marque$Modele$Annee$Prix_HT$Couleur$Vendue)

Le programme doit ajouter les deux CSV en BDD puis attribuer les voitures à un client si elles sont déjà vendues.
Une voiture peut appartenir à plusieurs clients et un client peut avoir plusieurs voitures. Cependant il ne peuvent
posséder chacun que 2 voitures.
Une fois l’initialisation faite, un menu en ligne de commande doit permettre de :
    - Voir l’ensemble des voitures
    - Voir l’ensemble des clients
    - Avoir plus d’information sur une voiture en particulier (ses caractéristiques, à qui elle appartient, …)
    - Avoir plus d’information sur un client en particulier (ses informations, quelles voitures il possède, …)
    - Vendre une voiture à un client que si elle n’est pas vendue
    - Changer la voiture de client si elle est déjà vendue
    - Combien vous avez gagné avec les voitures vendues dans la semaine
Rendu sur Git, avec un beau readme, sans franglais, avec des commentaires et en appliqua

# Setup 
    - dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
    - dotnet add package Microsoft.EntityFrameworkCore.Design
    - dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
    - dotnet add package Microsoft.Extensions.Hosting 
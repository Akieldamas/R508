# R508 - Projet .NET 8 (API REST + Blazor)

## 📌 Description du projet
Ce projet contient :
- Une **API REST en .NET 8** (basée sur ASP.NET Core) permettant de gérer des entités (ex. `Marque`, `Produit`).
- Une **application Blazor** pour la partie front-end.

⚠️ Le projet est **incomplet** et n’offre que des fonctionnalités de base, mais il fournit déjà :
- Des endpoints REST pour créer, lire, mettre à jour et supprimer des produits (`ProduitController`).
- Un mapping simple entre entités et DTOs via AutoMapper.
- Une base pour connecter l’API avec une base PostgreSQL.

---

## 🛠️ Prérequis

Avant de lancer le projet, assurez-vous d’avoir installé les outils suivants :

- **PostgreSQL** : [Télécharger PostgreSQL](https://www.postgresql.org/download/)
- **pgAdmin** (outil graphique pour gérer PostgreSQL) : [Télécharger pgAdmin](https://www.pgadmin.org/download/)
- **.NET 8 SDK** : [Télécharger .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

---

## 🗄️ Base de données

1. Créez une base PostgreSQL nommée **`R508`** via pgAdmin ou en ligne de commande :
   ```sql
   CREATE DATABASE R508;
   ```

2. Vérifiez vos identifiants PostgreSQL (par défaut `postgres` / mot de passe choisi à l’installation).

---

## ⚙️ Configuration d’Entity Framework Core

L’API utilise **Entity Framework Core** pour gérer la base de données.

1. Installez l’outil CLI EF Core :
   ```bash
   dotnet tool install --global dotnet-ef --version 8.0.11
   ```

   > ℹ️ Après installation, redémarrez votre machine pour vous assurer que `dotnet ef` est disponible dans le terminal.

2. Appliquez les migrations et créez le schéma de base :
   ```bash
   dotnet ef database update --project App
   ```

   Cela va créer les tables nécessaires dans la base `R508`.

---

## 🚀 Lancer le projet

1. Lancez l’API REST :
   ```bash
   dotnet run --project App
   ```

   L’API sera accessible (par défaut) sur :  
   👉 https://localhost:5001/swagger (Swagger UI)

2. Lancez l’application **Blazor** (si incluse dans la solution) :
   ```bash
   dotnet run --project BlazorApp
   ```

---

## 📌 Notes
- Le projet est une **maquette pédagogique** (BUT3 Informatique – R508).  
- Il ne couvre pas encore toutes les fonctionnalités (authentification, validation avancée, UI complète Blazor).  
- Il peut servir de point de départ pour vos développements.

---

## 📚 Ressources utiles
- [Documentation officielle ASP.NET Core](https://learn.microsoft.com/fr-fr/aspnet/core/?view=aspnetcore-8.0)
- [Documentation Entity Framework Core](https://learn.microsoft.com/fr-fr/ef/core/)
- [Documentation Blazor](https://learn.microsoft.com/fr-fr/aspnet/core/blazor/?view=aspnetcore-8.0)

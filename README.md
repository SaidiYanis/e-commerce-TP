# Ecommerce BackOffice

Back-office e-commerce réalisé en .NET 8 pour un TP final.

Le projet couvre :

- le catalogue avec catégories et produits ;
- le cycle de vie des commandes : `Cart`, `Paid`, `Cancelled`, `Delivered` ;
- une architecture métier simple, testable et découplée de la base de données.

## Stack

- `.NET 8`
- `C#`
- `ASP.NET Core Web API` avec Minimal API
- `xUnit`
- repositories `InMemory`

## Architecture

Le dépôt suit une logique de Clean Architecture simple à expliquer :

- `Ecommerce.BackOffice.Catalog`
  Contient le domaine catalogue, les cas d’usage applicatifs et les ports de persistance.
- `Ecommerce.BackOffice.Orders`
  Contient le domaine commande, les cas d’usage applicatifs et les ports nécessaires au module commande.
- `Ecommerce.BackOffice.Infrastructure`
  Contient les implémentations InMemory des interfaces de repository et l’adaptateur de stock entre commandes et catalogue.
- `Ecommerce.BackOffice.Api`
  Expose les endpoints HTTP et branche les dépendances.
- `Ecommerce.BackOffice.SharedKernel`
  Contient quelques abstractions transverses minimales (`IEntity`, `Result`).

Le domaine est isolé :

- pas de dépendance à ASP.NET ;
- pas de dépendance à EF Core ;
- pas de dépendance à PostgreSQL ;
- pas de dépendance à un framework d’infrastructure.

Les règles métier sont placées :

- dans `src/Ecommerce.BackOffice.Catalog/Domain` pour `Category` et `Product` ;
- dans `src/Ecommerce.BackOffice.Orders/Domain` pour `Order` et `OrderLine` ;
- dans `Application` quand la règle dépend d’un accès externe.
  Exemple : vérifier qu’une catégorie existe avant de créer un produit.

## Structure

```text
/src
  /Ecommerce.BackOffice.Api
  /Ecommerce.BackOffice.Catalog
    /Domain
    /Application
    /Ports
  /Ecommerce.BackOffice.Orders
    /Domain
    /Application
    /Ports
  /Ecommerce.BackOffice.Infrastructure
    /Repositories
  /Ecommerce.BackOffice.SharedKernel

/tests
  /Ecommerce.BackOffice.Catalog.Tests
  /Ecommerce.BackOffice.Orders.Tests
  /Ecommerce.BackOffice.Application.Tests

/docs
  user-stories.md
  scenarios-given-when-then.md
  architecture.md
```

## Choix de modélisation

- `Category` et `Product` sont des entités avec méthodes `Create` et `Update` qui refusent les états invalides.
- `Order` est l’agrégat principal pour le panier et le cycle de vie de commande.
- le prix d’une ligne de commande est figé au moment de l’ajout dans la commande ;
- l’ajout répété du même produit incrémente la quantité de la ligne existante ;
- le stock est décrémenté au paiement, pas à la création du panier ;
- la vérification “catégorie existante” est faite dans l’application, car elle nécessite un repository.

## Lancer le projet

```bash
dotnet restore Ecommerce.BackOffice.sln
dotnet run --project src/Ecommerce.BackOffice.Api
```

En environnement Development, la documentation Swagger est disponible sur :

```text
/swagger
```

Par défaut, les endpoints principaux sont :

- `GET /categories`
- `POST /categories`
- `PUT /categories/{id}`
- `DELETE /categories/{id}`
- `GET /products`
- `POST /products`
- `PUT /products/{id}`
- `DELETE /products/{id}`
- `GET /orders`
- `POST /orders`
- `POST /orders/{id}/lines`
- `POST /orders/{id}/pay`
- `POST /orders/{id}/cancel`
- `POST /orders/{id}/deliver`

## Lancer les tests

```bash
dotnet test Ecommerce.BackOffice.sln
```

## Où sont les règles métier

- catégories : [Category.cs](./src/Ecommerce.BackOffice.Catalog/Domain/Category.cs)
- produits : [Product.cs](./src/Ecommerce.BackOffice.Catalog/Domain/Product.cs)
- commandes : [Order.cs](./src/Ecommerce.BackOffice.Orders/Domain/Order.cs)

## Isolation du domaine par rapport à la base

Le domaine ne connaît aucune implémentation de stockage.

- les interfaces de repository sont dans `Ports` ;
- les implémentations concrètes sont dans `Infrastructure/Repositories` ;
- l’API dépend des interfaces et des implémentations InMemory ;
- un adaptateur PostgreSQL / EF Core pourrait être ajouté sans modifier le domaine.

## Limites assumées

- pas d’authentification ;
- pas de frontend ;
- pas de persistance durable ;
- pas encore d’adaptateur PostgreSQL / EF Core ;
- les erreurs métier sont retournées sous forme de `Result` simples ;
- l’API reste volontairement minimale pour favoriser la lisibilité en soutenance.

## Préparation soutenance orale

Points à expliquer en 15 minutes :

1. le découpage en modules `Catalog`, `Orders`, `Infrastructure`, `Api` ;
2. la différence entre domaine, application et infrastructure ;
3. pourquoi le domaine ne dépend ni d’ASP.NET ni de la base ;
4. comment les invariants sont protégés dans `Category`, `Product` et `Order` ;
5. pourquoi certaines règles sont dans l’application :
   contrôle d’existence d’une catégorie, lecture de stock, décrément de stock ;
6. comment les tests valident le domaine sans base de données ;
7. comment remplacer l’InMemory par PostgreSQL plus tard.

## Git

Le travail a été organisé avec les branches suivantes :

- `main`
- `develop`
- `feature/init-solution`
- `feature/catalog-domain`
- `feature/catalog-application`
- `feature/orders-domain`
- `feature/orders-application`
- `feature/inmemory-infrastructure`
- `feature/api`
- `feature/tests`
- `feature/documentation`

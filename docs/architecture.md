# Architecture

## Principe général

Le projet est un monolithe modulaire avec deux modules métier :

- `Catalog`
- `Orders`

Chaque module contient :

- `Domain`
- `Application`
- `Ports`

L’infrastructure est séparée dans `Ecommerce.BackOffice.Infrastructure` et l’exposition HTTP dans `Ecommerce.BackOffice.Api`.

## Dépendances

Le sens des dépendances est le suivant :

```text
Api -> Application + Ports + Infrastructure
Infrastructure -> Ports + Domain
Application -> Domain + Ports
Domain -> SharedKernel
```

Le domaine ne dépend jamais d’ASP.NET, d’EF Core ou de la base de données.

## Placement des règles

### Domaine

Le domaine protège les invariants qui ne nécessitent aucune dépendance externe :

- longueur de titre et description ;
- bornes sur les prix ;
- stock non négatif ;
- quantité maximale par produit dans une commande ;
- nombre maximal de produits différents ;
- transitions de statut d’une commande.

### Application

L’application orchestre les règles qui nécessitent des ports externes :

- vérifier qu’une catégorie existe avant de créer un produit ;
- vérifier qu’un produit existe avant de créer une commande ;
- vérifier le stock avant ajout ou paiement ;
- décrémenter le stock lors du paiement.

## Pourquoi ce découpage est simple à défendre

- les entités métier sont peu nombreuses ;
- chaque règle importante a un emplacement clair ;
- les repositories sont des interfaces faciles à mocker ou remplacer ;
- les tests de domaine tournent sans base ;
- l’infrastructure InMemory montre le principe sans bruit technique.

## Évolution possible vers PostgreSQL

Le prochain pas naturel serait :

1. ajouter un projet ou dossier EF Core dans `Infrastructure` ;
2. implémenter les mêmes interfaces de `Ports` ;
3. brancher ces implémentations dans l’API ;
4. conserver le domaine et les services applicatifs inchangés.

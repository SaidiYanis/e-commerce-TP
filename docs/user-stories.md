# User Stories

## Catalogue

### US1 - Créer une catégorie

En tant que gestionnaire du back-office,  
je veux créer une catégorie avec un titre, une description et une couleur,  
afin d’organiser les produits du catalogue.

### US2 - Créer un produit

En tant que gestionnaire du back-office,  
je veux créer un produit associé à une catégorie existante,  
afin de publier une offre vendable dans le catalogue.

### US3 - Modifier un produit

En tant que gestionnaire du back-office,  
je veux mettre à jour le prix, le prix promotionnel, le stock ou la catégorie d’un produit,  
afin de refléter l’état réel du catalogue.

## Commandes

### US4 - Créer un panier

En tant qu’utilisateur métier,  
je veux créer une commande à partir d’un premier produit disponible,  
afin de démarrer un panier.

### US5 - Ajouter un produit dans une commande existante

En tant qu’utilisateur métier,  
je veux ajouter un produit dans un panier existant,  
afin de compléter la commande tant qu’elle est encore au statut `Cart`.

### US6 - Payer une commande

En tant qu’utilisateur métier,  
je veux payer une commande au statut `Cart`,  
afin de figer l’achat et décrémenter le stock au bon moment.

### US7 - Annuler ou livrer une commande

En tant qu’utilisateur métier,  
je veux annuler une commande tant qu’elle est en `Cart` et la livrer uniquement si elle est `Paid`,  
afin de respecter le cycle de vie métier.

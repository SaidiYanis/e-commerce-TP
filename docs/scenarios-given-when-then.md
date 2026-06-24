# Scenarios Given When Then

## Catégorie

### Création valide d’une catégorie

Given un titre de 3 caractères ou plus  
And une description de 3 caractères ou plus  
When je crée une catégorie  
Then la catégorie est créée

### Création invalide d’une catégorie

Given un titre de moins de 3 caractères  
When je crée une catégorie  
Then la création échoue avec une erreur métier

## Produit

### Produit rattaché à une catégorie existante

Given une catégorie existante  
And un prix supérieur à 0  
And un stock supérieur ou égal à 0  
When je crée un produit  
Then le produit est créé

### Produit avec prix promotionnel invalide

Given un prix promotionnel supérieur ou égal au prix normal  
When je crée un produit  
Then la création échoue

## Commande

### Création d’un panier

Given un produit existant avec du stock  
When je crée une commande avec ce produit  
Then une commande au statut `Cart` est créée  
And son total correspond au prix figé de la ligne

### Ajout du même produit

Given une commande au statut `Cart` contenant déjà un produit  
When j’ajoute à nouveau ce même produit  
Then la quantité de la ligne est incrémentée  
And le prix unitaire de la ligne ne change pas

### Paiement d’une commande

Given une commande au statut `Cart`  
And un stock suffisant pour chaque ligne  
When je paie la commande  
Then la commande passe au statut `Paid`  
And le stock est décrémenté

### Paiement refusé

Given une commande au statut `Cart`  
And un stock insuffisant pour au moins une ligne  
When je tente de payer la commande  
Then le paiement échoue

### Annulation d’une commande

Given une commande au statut `Cart`  
When je l’annule  
Then la commande passe au statut `Cancelled`

### Livraison d’une commande

Given une commande au statut `Paid`  
When je la livre  
Then la commande passe au statut `Delivered`

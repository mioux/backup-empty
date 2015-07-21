Ceci est un outil que je me suis codé pour purger les bases de données __EN ENVIRONNEMENT DE DEVELOPPEMENT UNIQUEMENT !!!__

Toutes les opérations opérées par cet outil ne sont pas __DU TOUT__ adaptées à un environnement de prod car :

* Les journaux de transactions sont vidés et non sauvegardés,
* Les bases de données sont passés en backup simple (sans journaux de transaction),
* Les bases sont shrinkées avec 0 espace libre.

Le bouton de restauration permet de remonter une base de données issue d'un autre serveur avec potentiellement des paramétrages différent sur la localisation des fichiers. Il s'occupe de faire le RESTORE avec les WITH MOVE adéquats (si la base de données cible existe au préallable, sinon le bouton ne PEUT pas fonctionner).

Pour information, l'image de la punaise a été trouvée sur [icon-finder](https://www.iconfinder.com/icons/16409/pin_icon#size=16), et modifiée par moi même pour la mettre droite.

Je ne retrouve plus la source de l'image de l'icône.

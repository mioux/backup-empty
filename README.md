Ceci est un outil que je me suis cod� pour purger les bases de donn�es __EN ENVIRONNEMENT DE DEVELOPPEMENT UNIQUEMENT !!!__

Toutes les op�rations op�r�es par cet outil ne sont pas __DU TOUT__ adapt�es � un environnement de prod car :

* Les journaux de transactions sont vid�s et non sauvegard�s,
* Les bases de donn�es sont pass�s en backup simple (sans journaux de transaction),
* Les bases sont shrink�es avec 0 espace libre.

Le bouton de restauration permet de remonter une base de donn�es issue d'un autre serveur avec potentiellement des param�trages diff�rent sur la localisation des fichiers. Il s'occupe de faire le RESTORE avec les WITH MOVE ad�quats (si la base de donn�es cible existe au pr�allable, sinon le bouton ne PEUT pas fonctionner).

Pour information, l'image de la punaise a �t� trouv�e sur [icon-finder](https://www.iconfinder.com/icons/16409/pin_icon#size=16), et modifi�e par moi m�me pour la mettre droite.

Je ne retrouve plus la source de l'image de l'ic�ne.

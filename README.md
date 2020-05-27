# RVT - Runtime Virtual Tour

RVT est un projet étudiant réalisé sur **Unity** dans le cadre des cours d'*Univers Virtuels* au **CÉGEP de Sept-Îles** (Canada, QC).

Le projet a pour but de permettre à un utilisateur de créer ses propres scènes, via un éditeur intégré à l'application, en récupérant un ou plusieurs modèles présent sur [Google Poly]([https://poly.google.com/](https://poly.google.com/)) (en utilisant leur **clé** attribué) puis de visionner les scènes créées en **Réalité Virtuel** à l'aide de l'**Occulus Rift**.

L'application est actuellement rudimentaire et est divisé en deux scènes particulières:
* Une **Scène de Construction**, pour la réalisation des scènes 3D.
* Une **Scène de Visualisation**, pour simuler et visionner les scènes créées en VR.

## Scène de Construction

La scène de construction est composé de cinq menus:
* Menu principal (*Main*)
* Menu de lancement (*Loader*)
* Menu de construction (*Builder*)
* Menu d'importation (*Import*)
* Menu des paramètres (*Settings*)

<p align="center">
	<img src="/Images/Construction/menuOrga.png" />
</p>

Voici quelques informations qui vous permettront de naviguer plus facilement dans cette scène:

### Principal

<p align="center">
	<img src="/Images/Construction/mainMenu.png" />
</p>

Le menu principal ne sert qu'a vous rediriger vers chacun des autres menus.
L'exception est le bouton *Exit* vous permettant de quitter l'application lorsque vous le souhaitez.

### Lancement

<p align="center">
	<img src="/Images/Construction/loaderMenu.png" />
</p>

La scène de lancement fait apparaître la liste des scène enregistrées dans vos documents locaux.
Le chemin de ces fichiers est supposé être le suivant: *C:\Users\\**[User]**\AppData\LocalLow\\**[Company]**\RVT - No Occulus Integration\Scenes* (le dossier *Scenes* est a ajoutée).
Lorsque vous cliquez sur l'une des scènes présentent, vous serez redirigé vers la **Scène de Visualisation**.

### Construction

<p align="center">
	<img src="/Images/Construction/builderMenu.png" />
</p>

Il est possible de créer vos propres scènes à partir de ce menu.
L'élément rouge présent initialement à l'écran représente le **joueur**.
Chaque élément ajouté (ou actuellement présent) peut être déplacé à l'aide d'un **clique gauche** de la souris sur son modèle puis en la glissant.
L'effet de transformation appliqué dépend de l’effet sélectionné (en haut à droite de l'écran) soit:
* **_Translate_**, permet de déplacer l'objet (un roulement de la molette le déplace sur la profondeur).
* **_Rotate_**, permet de tourner l'objet sur lui-même.
* **_Scale_**, permet de changer la taille d'un objet (par déplacement à l'horizontal de la souris).

Le bouton *Save* et *Ok* permettent de sauvegarder le contenu de la scène, puis (dans le cas du bouton *Ok*) l'enregistré dans nos fichiers locaux.
Le bouton *Import* va nous permettre d'accéder au **Menu d'Importation**.

### Importation

<p align="center">
	<img src="/Images/Construction/importMenu.png" />
</p>

En entrant la **clé** d'un modèle sur [Google Poly]([https://poly.google.com/](https://poly.google.com/)) dans le champ d'entrée en haut à gauche du menu et en appuyant sur le bouton *Import*, il est possible d'ajouter directement un modèle présent sur le site dans votre application.
Si l'objet vous convient, appuyer sur le bouton *Insert* pour ajouter l'objet dans votre **Menu de Construction**.

### Paramètres

<p align="center">
	<img src="/Images/Construction/settingsMenu.png" />
</p>

Ce menu n'a pour but de changer que deux paramètres importants de l'appication:
* La main principale de l'utilisateur.
* La taille du joueur par rapport à son environnement.

Une fois changé(s), appuyer sur le bouton *Back* pour revenir au **Menu Principal** ce qui sauvegardera immédiatement vos paramètres dans le fichier **_settings.dat_** (un fichier avant l'emplacement de vos fichiers scènes, dans vos fichiers locaux). Si ce fichier n'existe pas, créer le.

## Scène de Visualisation

La scène de visualisation vous permet, si vous possédez l'équipement **_Occulus Rift_** adapté (le casque + 2 manettes), de parcourir une scène lancée en première personne.
Vous disposez de plusieurs éléments pour faciliter votre expérience:
* Un **Inventaire** (main principale).
* Un **Menu Utilitaire** (main secondaire).

Chacun de ces éléments peut être ouvert avec le bouton **Y** ou **B** selon la main auquel ces derniers appartiennent.
Voici quelques informations à leurs égards:

### Inventaire

L'inventaire est normalement composé de 4 objets particuliers vous donnant un contrôle particulier sur votre expérience d'utilisateur.
Il est possible de changer l'objet en actuellement en main en cliquant sur **X** ou **A** (gauche et droite respectivement).
Fermé l'inventaire vous changera les objets actuellement en main.
Voici la liste de chaque objet actuellement existant dans l'application:

#### MovementController

Cet objet permet de contrôler la position de l'utilisateur dans la scène:
* Le joystick vous déplace horizontalement.
* Le bouton de l'index vous fait courir.
* Le bouton du majeur vous fait sauter.

#### CameraController

Cet objet permet de contrôler le décalage horizontal de la caméra (afin de vous éviter de tourner sur vous même).
Seul le mouvement horizontal du joystick est ici utilisé.

#### Remote

Cet objet vous permet de contrôler un **drone** présent sur votre scène dés son chargement.
Une fois pris en main, cet objet active/allume le drone. Dans le cas contraire, le drone est désactivé/éteint.
Les contrôles du drone avec la **Remote** sont les suivants:
* Le mouvement horizontal du joystick modifie son **lacet (yaw)**.
* Le mouvement vertical du joystick modifie son **roulis (roll)**.
* Le bouton de l'index augmente sa **force (thrust)**, tandis que le bouton du majeur la diminue.

Lorsque le drone est allumé, celui ci restera en vol stationnaire si aucun mouvement n'est effectué. Cependant, une fois éteint, le drone retombe au sol.

#### Tablet

Cet objet vous permet de visualiser la sortie de la caméra du drone (d'ou l'intérêt du drone: filmer les parties inatteignables du monde actuel).
La tablette ajoute aussi de nouveaux contrôle au drone:
* Le joystick déplace le drône à son horizontal.
* Le bouton de l'index, tant qu'il est enfoncé, empêche le drone de tourner en **lacet**.
* Le bouton du majeur, tant qu'il est enfoncé, empêche le drone de tourner en **roulis**.

### Menu Utilitaire

Le menu utilitaire est composé de quatre boutons:
* *Cancel*, n'ayant aucun effet.
* *Respawn*, vous téléportant à votre position de départ.
* *Bring Drone*, apportant le drone sur votre position.
* *Back to Menu*, vous retournant à la **Scène de Construction**.

Il est possible de sélectionner chacun de ces objets en restant appuyé sur le bouton d'ouverture du menu pendant un temps donné (généralement une seconde).
Le bouton actuellement sélectionné apparaît en vert et son effect s'applique lorsque vous fermez le menu (à l'aide d'un clique rapide sur le bouton d'ouverture).
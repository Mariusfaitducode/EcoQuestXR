OBJETS A AVOIR DANS LA SCENE 

- Passthrough

- OVR Scene Manage

- Oculus interaction sample rig 
	- left controller anchor
		- left arm deck 	
			- deck canva 
			- game object vides 

- UI helper
	- LaserPointer
	- Sphere
	- Event system

PREFAB NECESSAIRE 

- Card 
- Grabbable Card 

(instancié dans les scripts) 

SCRIPT A AVOIR SUR LES OBJETS 

- sur les canvas : graphic raycaster 
- left arm deck : deck controller 
- event system : ovr input module 
- grabbale : plusieurs script (voir le prefab) 


IMPORTANT A RENSEIGNER 

- dans cardManager : le prefab grabbableCard, le left arm deck, le depot zone 


AUTRE : 

- le depot zone doit avoir un collider 

- le validate button du draf doit avoir un appel à la fonction play event de card manager

using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



// Control apparence of deck canvas when turning hand

public class DeckController : MonoBehaviour
{
    
    // attribut depuis le prefab 
    internal Transform leftControllerAnchor;
    internal Transform centerEyeAnchor;

    internal ControlMode controlMode;
    
    
    public float distToHand = 0.3f;

    
    private Canvas canva;

    public void InitializeDeckController()
    {
        canva = GetComponentInChildren<Canvas>();
    }

    void Update()
    {
        
        if (controlMode == ControlMode.keyboard) return;
        
        if (leftControllerAnchor.rotation.z < -0.5 || leftControllerAnchor.rotation.z > 0.5)
        {

            canva.enabled = true;

            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                Canvas canvas = child.GetComponent<Canvas>();
                if (canvas != null) canvas.enabled = true;

                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = true;
            }


            this.transform.position = new Vector3(leftControllerAnchor.position.x, leftControllerAnchor.position.y + distToHand, leftControllerAnchor.position.z);

            Vector3 directionToPlayer = centerEyeAnchor.position - leftControllerAnchor.position;
            directionToPlayer.y = 0f;

            transform.LookAt(transform.position + directionToPlayer);
            
            // Utiliser la fonction LookRotation pour obtenir la rotation vers le joueur
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Inverser la rotation sur l'axe des Y pour corriger l'orientation du texte
            targetRotation *= Quaternion.Euler(0, 180f, 0);

            // Appliquer la rotation à l'objet leftArmDeck
            transform.rotation = targetRotation;
        }
        else
        {

            canva.enabled = false;
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                Canvas canvas = child.GetComponent<Canvas>();
                if (canvas != null) canvas.enabled = false;

                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = false;
            }
        }
    }

}

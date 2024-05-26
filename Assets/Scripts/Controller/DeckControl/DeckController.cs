using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    private Canvas canva;
    
    public List<GameObject> list_cards;
    
    public Transform leftControllerAnchor;
    public Transform centerEyeAnchor;
    public float distToHand = (float)0.3;
    
    // Start is called before the first frame update
    void Start()
    {
        canva = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("pos : " + leftControllerAnchor.position + ", rot : " + leftControllerAnchor.rotation);
        
        if (leftControllerAnchor.rotation.z < -0.5 || leftControllerAnchor.rotation.z > 0.5)
        {

            canva.enabled = true;
            
            foreach (GameObject card in list_cards)
            {
                card.GetComponentInChildren<Canvas>().enabled = true;
                card.GetComponent<Renderer>().enabled = true;
            }


            this.transform.position = new Vector3(
                leftControllerAnchor.position.x,
                leftControllerAnchor.position.y + distToHand,
                leftControllerAnchor.position.z
            );

            Vector3 directionToPlayer = centerEyeAnchor.position - leftControllerAnchor.position;
            directionToPlayer.y = 0f;
            // transform.LookAt(transform.position + directionToPlayer);
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
            foreach (GameObject card in list_cards)
            {
                if (!card.GetComponent<Grabbable>().isGrabbed)
                {
                    card.GetComponentInChildren<Canvas>().enabled = false;
                    card.GetComponent<Renderer>().enabled = false;
                }
            }

        }


    }


    public void RemoveCard(GameObject card)
    {
        list_cards.Remove(card);
    }
}

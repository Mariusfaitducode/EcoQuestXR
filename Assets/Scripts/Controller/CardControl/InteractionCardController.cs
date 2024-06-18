using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionCardController : MonoBehaviour
{
    internal CardManager cardManager;
    internal Transform initialPlaceTransform;
    internal GameObject depotZone;

    internal ControlMode controlMode;

    
    
    //public GameObject deck;
    public float depotThreshold = 0.1f;
    public float returnSpeed = 2.0f; // Vitesse de retour




    // attributs privés 
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Grabbable grabbable;
    private bool isNearDepot;


    public void InitializeInteraction()
    {
        if (controlMode == ControlMode.keyboard) return;
        
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null)
        {
            Debug.LogError("OVRGrabbable component is missing on this object.");
        }

    }

    void Update()
    {
        
        if (controlMode == ControlMode.keyboard) return;

        // Enregistrer la position et la rotation initiales
        initialPosition = initialPlaceTransform.position;
        initialRotation = initialPlaceTransform.rotation;


        if (!grabbable.isGrabbed)
        {
            if (Vector3.Distance(transform.position, initialPosition) > 0.01f || Quaternion.Angle(transform.rotation, initialRotation) > 1.0f)
            {
                if (isNearDepot)
                {
                    PlayCard();
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * returnSpeed);
                    transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * returnSpeed);
                }
            }
            else
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
            }


            cardManager.gameManager.cloudController.is_moving = false;

        }
        else
        {
            foreach (Canvas canva in GetComponentsInChildren<Canvas>())
            {
                canva.enabled = true;
            }

            cardManager.gameManager.cloudController.is_moving = true;
        }

    }

    private void PlayCard()
    {

        

        if (cardManager.CanPlayCard(cardManager.gameManager.statManager.globalStats.currentMoneyInBank, float.Parse(this.GetComponentInChildren<DisplayCard>()._price.text))) {

            isNearDepot = false;
            System.Random random = new System.Random();

            cardManager.gameManager.cloudController.ChangeText("notEnoughMoney" + random.Next(1, 3));

        } 
        else 
        {
            cardManager.PlayEvent(this.gameObject);

            // Add little animation here
            Destroy(this);
        }
    }

    #region collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == depotZone)
        {
            isNearDepot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == depotZone)
        {
            isNearDepot = false;
        }
    }
    #endregion 


}

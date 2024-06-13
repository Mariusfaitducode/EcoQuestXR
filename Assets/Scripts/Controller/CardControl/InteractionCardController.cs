using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionCardController : MonoBehaviour
{
    // initialement dans le prefab 
    internal CardManager cardManager;
    internal Transform initialPlaceTransform;
    internal GameObject depotZone;

    internal ControlMode controlMode;

    
    
    //public GameObject deck;
    public float depotThreshold = 0.1f;
    public float returnSpeed = 2.0f; // Vitesse de retour


    // paramétrer par CardManager via Display Canva


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
        }

    }

    private void PlayCard()
    {

        cardManager.PlayEvent(this.gameObject);

        // Add little animation here
        Destroy(this);

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

using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCardController : MonoBehaviour
{
    // initialement dans le prefab 
    public CardManager CardManager;
    public GameObject deck;
    public float depotThreshold = 0.1f;
    public float returnSpeed = 2.0f; // Vitesse de retour


    // paramétrer par CardManager via Display Canva
    public GameObject depot_zone;
    public Transform initialPlaceTransform;


    // attributs privés 
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Grabbable grabbable;
    private bool isNearDepot;


    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null)
        {
            Debug.LogError("OVRGrabbable component is missing on this object.");
        }

    }

    void Update()
    {

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

        CardManager.PlayEvent(this.gameObject);

        // Add little animation here
        Destroy(this);

    }

    #region collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == depot_zone)
        {
            isNearDepot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == depot_zone)
        {
            isNearDepot = false;
        }
    }
    #endregion 


}

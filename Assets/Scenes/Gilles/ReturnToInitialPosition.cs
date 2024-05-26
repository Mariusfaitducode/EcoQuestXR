using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToInitialPosition : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Grabbable grabbable;
    private bool isReturning = false;
    public float returnSpeed = 2.0f; // Vitesse de retour

    public Transform placeTransform;

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
        initialPosition = placeTransform.position;
        initialRotation = placeTransform.rotation;


        // Vérifier si l'objet est lâché
        if (!grabbable.isGrabbed && !isReturning)
        {
            isReturning = true;
        }

        // Si l'objet est en train de revenir
        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * returnSpeed);

            // Arrêter l'animation de retour quand l'objet est suffisamment proche de la position initiale
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f && Quaternion.Angle(transform.rotation, initialRotation) < 1.0f)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                isReturning = false;
            }
        }
    }
}

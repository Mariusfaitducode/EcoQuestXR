using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCardController : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Grabbable grabbable;
    private bool isReturning = false;
    public float returnSpeed = 2.0f; // Vitesse de retour

    public Transform placeTransform;

    public GameObject depot_zone;
    public float depotThreshold = 0.1f;

    public GameObject deck;

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

            // Vérifier la distance par rapport à depot_zone si la carte a été choisie
            if (Vector3.Distance(transform.position, initialPosition) > 0.05f && Vector3.Distance(transform.position, depot_zone.transform.position) < depotThreshold)
            {

                // Si la carte est proche de depot_zone, la rendre enfant de depot_zone et l'aligner
                AlignWithDepotZone();
                isReturning = false;
            }
            else
            {
                // Si la carte n'est pas proche de depot_zone, la faire revenir à sa position initiale
                isReturning = true;
            }
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

    private void AlignWithDepotZone()
    {
        transform.SetParent(depot_zone.transform);
        transform.localPosition = new Vector3(depot_zone.transform.position.x, depot_zone.transform.position.y + 0.22f, depot_zone.transform.position.z); // Réinitialiser la position locale pour aligner les positions
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f); ; // Réinitialiser la rotation locale pour aligner les rotations

        deck.GetComponent<DeckController>().RemoveCard(this.gameObject);
    }
}

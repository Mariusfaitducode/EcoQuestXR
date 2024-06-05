using System.Collections;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    // Variable publique pour le temps d'attente en secondes
    public float waitTime = 5.0f;


    public Transform DrawPileTransform;
    public Transform DashboardTransform;


    private GameObject table;
    private bool tableFound = false;


    // Start est appelé avant le premier frame update
    void Start()
    {
        // Démarrer la coroutine qui attend et affiche un message
        StartCoroutine(WaitAndPlaceObj());
    }

    // Coroutine pour attendre et afficher un message
    IEnumerator WaitAndPlaceObj()
    {

        while (!tableFound)
        {
            // Attendre pendant waitTime secondes
            yield return new WaitForSeconds(waitTime);

            FindTable();
        }

        SetObjPosition();

    }

    void FindTable()
    {
        table = GameObject.FindGameObjectWithTag("Table");
        if (table != null)
        {
            Debug.Log("Table found");
            tableFound = true;
        }

    }


    // While the player has not interacted with map, we continue checking for the table position.
    void SetObjPosition()
    {
        Vector3 initialPosition = table.transform.position;
        

        DrawPileTransform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + 5);
        DashboardTransform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + 5);




        Debug.Log("hi mom");

        //updateTerrainRenderer.UpdateLimitTerrainCenter();

        //if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick) != Vector2.zero || OVRInput.Get(OVRInput.RawAxis2D.LThumbstick) != Vector2.zero)
        //{
        //    playerHasMoved = true;
        //}
    }


    // Update est appelé une fois par frame
    void Update()
    {
       

    }
}

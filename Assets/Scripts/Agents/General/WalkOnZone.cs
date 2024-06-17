using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnZone : MonoBehaviour
{
    internal AgentManager agentManager;
    internal bool initialized = false;

    internal GameObject place;

    internal float speed;
    internal float turnSpeed;
    
    public float speedFactor = 0.2f;
    public float turnSpeedFactor = 50.0f;
    
    private float timer;
    private float newDirectionTimer = 2.0f;
    
    internal float sizePlace;
    internal float uniformScale = 1f;

    
    void Start()
    {
        agentManager = FindObjectOfType<AgentManager>();
        
        // Initialize();
    }
    
    void Update()
    {
        
        if (agentManager != null && !initialized)
        {
            
            initialized = true;
        }

        if (initialized && !agentManager.timer.isTimePaused)
        {
            
            speed = speedFactor * sizePlace * agentManager.objectManager.mesh.transform.localScale.x;
            turnSpeed = turnSpeedFactor * sizePlace * agentManager.objectManager.mesh.transform.localScale.x;
            
            timer += Time.deltaTime;

            // Changer de direction à intervalles réguliers
            if (timer > newDirectionTimer)
            {
                timer = 0;
                newDirectionTimer = Random.Range(1.0f, 4.0f);  // Changer de direction toutes les 1 à 4 secondes
                ChangeDirection();
            }
        
            
            
            
            float radius = sizePlace * agentManager.objectManager.mesh.transform.localScale.x;
            
        
        

            // Vector3 lastPosition = transform.position;
            // Mouvement en avant
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
            Vector3 localPosition = transform.position - place.transform.position;
            
            // Debug.Log("Radius :" + radius + " LocalPosition : " + localPosition + " Position : " + transform.position);


            if (localPosition.x > radius ||localPosition.x < -radius)
            {
                localPosition.x = 0;
                transform.Rotate(Vector3.up, 180);
            }
            else if (localPosition.z > radius || localPosition.z < -radius)
            {
                localPosition.z = 0;
                transform.Rotate(Vector3.up, 180);
            }

            // transform.localPosition = localPosition;
        
            // Vérifier si le nouvel emplacement est toujours dans la zone
        }
        
        
    }

    void ChangeDirection()
    {
        float turnAngle = Random.Range(-90, 90); // Choisir un nouvel angle au hasard entre -90 et 90 degrés
        transform.Rotate(Vector3.up, turnAngle);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Inverser la direction en cas de collision
        transform.Rotate(Vector3.up, 180);
    }
}

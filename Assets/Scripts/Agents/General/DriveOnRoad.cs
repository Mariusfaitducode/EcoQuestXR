using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveOnRoad : MonoBehaviour
{
    internal AgentManager agentManager;
    // public AreaType areaType;

    public float speed = 3f;
    public float treshold = 1f;
    
    public float roadStep = 0.5f;
    
    // internal AreaCell[,] areaGrid;
    // internal float mapScale;
    
    // internal AreaCell actualCell;
    // internal AreaCell nextCell;
    // internal AreaCell lastCell;

    public bool stopped;
    internal bool initialized = false;
    
    
    // List<FindPath.PathPoint> bigRoadPath;
    
    internal List<List<FindPath.PathPoint>> listRoads;
    internal List<FindPath.PathPoint> choosedRoad;
    
    internal FindPath.PathPoint target;

    internal FindPath.PathPoint lastPoint;
    internal FindPath.PathPoint actualPoint;
    internal int actualRoadIndex;

    internal FindPath.PathPoint nextPoint;
    internal int nextRoadIndex;

    
    void Start()
    {
        agentManager = FindObjectOfType<AgentManager>();
        listRoads = agentManager.listRoads;
    }

    void Update()
    {
        if (agentManager != null && agentManager.areas != null && agentManager.areas.Count > 0 && !initialized)
        {
            // speed = agentManager.mapScale * speed;
            // treshold = agentManager.mapScale * treshold;
            // roadStep = agentManager.mapScale * roadStep;
            
            initialized = Initialize();
        }


        if (initialized && !agentManager.timer.isTimePaused)
        {
            Vector3 direction = (nextPoint.position) - (actualPoint.position);
            
            speed = direction.magnitude * 2;
            treshold = direction.magnitude / 2;
            roadStep = direction.magnitude / 4;
            
            this.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            
            
            if (Vector3.Distance(this.transform.position, (nextPoint.position)) < treshold)
            {
                ChangeTarget();
            }
        }
    }
    
    
    bool Initialize()
    {
        // TODO
        
        // Find target
        
        
        // Random road en fonction de la longeur
        choosedRoad = listRoads[Random.Range(0, listRoads.Count)];
        
        if (Random.Range(0, 2) == 0)
        {
            target = choosedRoad[0];
        }
        else
        {
            target = choosedRoad[choosedRoad.Count - 1];
        }

        // Find firstPoint
        
        
        actualRoadIndex = Random.Range(0, choosedRoad.Count);
        
        actualPoint = choosedRoad[actualRoadIndex];
        lastPoint = actualPoint;
        
        // Find nextPoint
        
        
        
        SearchNeighbour();
        
        // Find good position
        
        // Take uniformScale into account

        this.transform.position = actualPoint.position;
        
        // Determine nextPoint
        
        
        
        Vector3 direction = (nextPoint.position) - (actualPoint.position);
        
        Vector3 directionRight = new Vector3(direction.z, 0f, -direction.x);
        
        this.transform.LookAt(nextPoint.position + directionRight * roadStep);

        return true;
    }
    
    
    
    void ChangeTarget()
    {
     
        // Arrived at destination
        if (nextRoadIndex == 0 || nextRoadIndex == choosedRoad.Count - 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                target = choosedRoad[0];
            }
            else
            {
                target = choosedRoad[choosedRoad.Count - 1];
            }
            return;
        }
        
        // Not arrived at destination
        lastPoint = actualPoint;
        actualPoint = nextPoint;
        
        actualRoadIndex = nextRoadIndex;
        
        SearchNeighbour();
        
        Vector3 direction = (nextPoint.position) - (actualPoint.position);
        
        Vector3 directionRight = new Vector3(direction.z, 0f, -direction.x);
        
        this.transform.LookAt(nextPoint.position + directionRight.normalized * roadStep);


    }
    
    void SearchNeighbour()
    {
        nextPoint = null;
        
        if (actualRoadIndex - 1 >= 0)
        {
            nextPoint = choosedRoad[actualRoadIndex - 1];
            nextRoadIndex = actualRoadIndex - 1;
        }
        if (actualRoadIndex + 1 < choosedRoad.Count)
        {
            
            if (nextPoint == null)
            {
                nextPoint = choosedRoad[actualRoadIndex + 1];
                nextRoadIndex = actualRoadIndex + 1;
                return;
            }
            
            float distanceMinus = Vector3.Distance(nextPoint.position, target.position);
            float distancePlus = Vector3.Distance(choosedRoad[actualRoadIndex + 1].position, target.position);
                
            if (distancePlus < distanceMinus)
            {
                nextPoint = choosedRoad[actualRoadIndex + 1];
                nextRoadIndex = actualRoadIndex + 1;
                return;
            }
        }
        if ((actualRoadIndex + 1 >= choosedRoad.Count) && (actualRoadIndex - 1 < 0))
        {
            Debug.LogError("Error : No next point found. Destroying object.");
            DestroyImmediate(this.gameObject);
            // return false;
        }
    }
}

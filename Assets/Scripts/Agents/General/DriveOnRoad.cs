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

    internal int direction; // 1 : right, -1 : left
    
    // List<FindPath.PathPoint> bigRoadPath;
    
    internal List<List<FindPath.PathPoint>> listRoads;
    internal List<FindPath.PathPoint> choosedRoad;
    
    internal FindPath.PathPoint target;
    internal bool targetEnd = false;

    // internal FindPath.PathPoint lastPoint;
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
            Vector3 direction = (nextPoint.pointPosition.transform.position) - (actualPoint.pointPosition.transform.position);
            
            speed = direction.magnitude * 2;
            treshold = direction.magnitude / 2;
            roadStep = direction.magnitude / 6;
            
            this.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            
            
            if (Vector3.Distance(this.transform.position, (nextPoint.pointPosition.transform.position)) < treshold)
            {
                ChangeTarget();
            }
        }
    }
    
    
    bool Initialize()
    {
        // TODO
        
        // Find target

        int totalPoints = 0;
        
        foreach (List<FindPath.PathPoint> road in listRoads)
        {
            totalPoints += road.Count;
        }
        
        int randomIndex = Random.Range(0, totalPoints);
        
        int index = 0;
        
        foreach (List<FindPath.PathPoint> road in listRoads)
        {
            if (index + road.Count > randomIndex)
            {
                choosedRoad = road;
                break;
            }
            index += road.Count;
        }
        
        // Random road en fonction de la longeur
        // choosedRoad = listRoads[0];
        
        // if (Random.Range(0, 2) == 0)
        // {
        //     target = choosedRoad[0];
        //     targetEnd = false;
        // }
        // else
        // {
        //     target = choosedRoad[choosedRoad.Count - 1];
        //     targetEnd = true;
        // }

        // Find firstPoint
        
        
        actualRoadIndex = Random.Range(0, choosedRoad.Count);
        
        actualPoint = choosedRoad[actualRoadIndex];
        // lastPoint = actualPoint;
        
        // Find nextPoint
        
        
        this.direction = Random.Range(0, 2) == 0 ? 1 : -1;
        
        Advance();
        
        
        // SearchNeighbour();
        
        // Find good position
        
        // Take uniformScale into account
        
        // TODO : Get good position

        this.transform.position = actualPoint.pointPosition.transform.position;
        
        // Determine nextPoint
        
        
        
        Vector3 newDirection = (nextPoint.pointPosition.transform.position) - (actualPoint.pointPosition.transform.position);
        
        Vector3 directionRight = new Vector3(newDirection.z, 0f, -newDirection.x);
        
        this.transform.LookAt(nextPoint.pointPosition.transform.position + directionRight * roadStep);

        return true;
    }
    
    
    
    void ChangeTarget()
    {
     
        // Arrived at destination
        if (nextRoadIndex == 0 || nextRoadIndex == choosedRoad.Count - 1)
        {
            // Debug.Log("Arrived at destination !!!!!!!!!!!!!!!!!");

            this.direction *= -1;
            
            // if (targetEnd)
            // {
            //     target = choosedRoad[0];
            //     targetEnd = false;
            // }
            // else
            // {
            //     target = choosedRoad[choosedRoad.Count - 1];
            //     targetEnd = true;
            // }
        }
        
        // Not arrived at destination
        // lastPoint = actualPoint;
        actualPoint = nextPoint;
        
        actualRoadIndex = nextRoadIndex;
        
        // SearchNeighbour();
        Advance();
        
        // Debug.Log("Next Road Index : " + nextRoadIndex + " Actual Road Index : " + actualRoadIndex + "TargetEnd : "+ targetEnd + " TargetIndexEnd : " + (choosedRoad.Count - 1) + " TargetIndexStart : " + 0 );
        
        // Debug.Log(0);

        if (nextRoadIndex == actualRoadIndex)
        {
            Debug.LogError("Error : No next point found. car is blocked");
        }
        
        Vector3 direction = (nextPoint.pointPosition.transform.position) - (actualPoint.pointPosition.transform.position);
        
        Vector3 directionRight = new Vector3(direction.z, 0f, -direction.x);
        
        this.transform.LookAt(nextPoint.pointPosition.transform.position + directionRight.normalized * roadStep);


    }
    
    
    void Advance()
    {
        
        if (actualRoadIndex + direction >= 0 && actualRoadIndex + direction < choosedRoad.Count)
        {
            nextPoint = choosedRoad[actualRoadIndex + this.direction];
            nextRoadIndex = actualRoadIndex + this.direction;
        }
        else
        {
            direction *= -1;
            nextPoint = choosedRoad[actualRoadIndex + this.direction];
            nextRoadIndex = actualRoadIndex + this.direction;
        }
        
        
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
            
            float distanceMinus = Vector3.Distance(nextPoint.pointPosition.transform.position, target.pointPosition.transform.position);
            float distancePlus = Vector3.Distance(choosedRoad[actualRoadIndex + 1].pointPosition.transform.position, target.pointPosition.transform.position);
                
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct DriveSettings
{
    internal float speed ;
    internal float treshold ;
    
    internal float roadStep ;
    
    public float speedFactor;
    // public float tresholdFactor ;
    public float roadStepFactor ;
}


public class DriveInArea : MonoBehaviour
{
    internal AgentManager agentManager;
    public AreaType areaType;

    public DriveSettings driveSettings;
    
    internal AreaCell[,] areaGrid;
    // internal float mapScale;
    
    internal AreaCell actualCell;

    internal AreaCell nextCell;
    internal AreaCell lastCell;
    
    
    internal float floorHeight = 0.04f; 

    public bool stopped;

    internal bool initialized = false;

    
    void Start()
    {
        agentManager = FindObjectOfType<AgentManager>();
        
        // Initialize();
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
            Vector3 direction = (nextCell.cellPosition.transform.position) - (actualCell.cellPosition.transform.position);
        
            driveSettings.speed = direction.magnitude * 2 * driveSettings.speedFactor;
            driveSettings.treshold = direction.magnitude / 2;
            driveSettings.roadStep = direction.magnitude / 4 * driveSettings.roadStepFactor;
            
            this.transform.Translate(Vector3.forward * (driveSettings.speed * Time.deltaTime));
            
            float height = floorHeight * agentManager.objectManager.mesh.transform.localScale.y;
            
            this.transform.position = new Vector3(this.transform.position.x, actualCell.cellPosition.transform.position.y + height, this.transform.position.z);
            

            if (Vector3.Distance(this.transform.position, (nextCell.cellPosition.transform.position)) < driveSettings.treshold)
            {
                ChangeTarget();
            }
        }
    }

    
    bool Initialize()
    {
        areaGrid = agentManager.areas.Find(a => a.data.type == areaType).areaGrid;

        float minDistance = float.MaxValue;
        
        foreach (AreaCell cell in areaGrid)
        {
            float distance = Vector3.Distance(this.transform.position, cell.cellPosition.transform.position);
            
            if (cell.type == CellType.Road && distance < minDistance)
            {
                minDistance = distance;
                actualCell = cell;
            }
        }

        this.transform.position = actualCell.cellPosition.transform.position ;


        SearchNeighbour();
        
        if (nextCell == null || nextCell.cellPosition == null)
        {
            // nextCell = actualCell;
            
            Debug.LogError("Error : No next cell found. Destroying object.");
            DestroyImmediate(this.gameObject);
            return false;
        }
        
        Vector3 direction = (nextCell.cellPosition.transform.position) - (actualCell.cellPosition.transform.position);
        
        Vector3 directionRight = new Vector3(direction.z, 0f, -direction.x);
        
        this.transform.LookAt(nextCell.cellPosition.transform.position + directionRight * driveSettings.roadStep);

        return true;
    }

    void ChangeTarget()
    {
        
        lastCell = actualCell;
        actualCell = nextCell;
        
        SearchNeighbour();
        
        Vector3 direction = (nextCell.cellPosition.transform.position) - (actualCell.cellPosition.transform.position);
        
        Vector3 directionRight = new Vector3(direction.z, 0f, -direction.x);
        
        this.transform.LookAt(nextCell.cellPosition.transform.position + directionRight.normalized * driveSettings.roadStep);


    }

    void SearchNeighbour()
    {
        int x = actualCell.gridPosition.x;
        int y = actualCell.gridPosition.y;
        int size = areaGrid.GetLength(0);
        
        // Debug.Log(size);
        
        List<AreaCell> validCells = new List<AreaCell>();

        for (int i = -1; i <= 1; i += 2)
        {
            AreaCell horizontalCell = null;

            if (0 < x + i && x + i < size-1)
            {
                horizontalCell = areaGrid[x + i, y];

                if (horizontalCell != lastCell && horizontalCell.type == CellType.Road)
                {
                    validCells.Add(horizontalCell);
                }
            }

            AreaCell verticalCell = null;
            
            if (0 < y + i && y + i < size-1)
            {
                verticalCell = areaGrid[x, y + i];
                
                if (verticalCell != lastCell && verticalCell.type == CellType.Road)
                {
                    validCells.Add(verticalCell);
                }
            }
        }

        if (validCells.Count == 0)
        {
            nextCell = lastCell;
        }
        else
        {
            nextCell = validCells[Random.Range(0, validCells.Count)];
        }
        
        
        
    }
}

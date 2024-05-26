using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveInArea : MonoBehaviour
{
    internal AgentManager agentManager;
    public AreaType areaType;

    public float speed = 5f;
    public float treshold = 0.05f;
    
    internal AreaCell[,] areaGrid;
    // internal float mapScale;
    
    internal AreaCell actualCell;

    internal AreaCell nextCell;
    internal AreaCell lastCell;

    public bool stopped;

    internal bool initialized = false;

    
    // Start is called before the first frame update
    void Start()
    {
        agentManager = FindObjectOfType<AgentManager>();
        
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

        // if (agentManager != null && agentManager.areas != null && agentManager.areas.Count > 0 && !initialized)
        // {
        //     initialized = true;
        // }


        if (!agentManager.timer.isTimePaused)
        {
            Vector3 direction = (nextCell.cellPosition.transform.position) - (actualCell.cellPosition.transform.position);
        
            this.transform.Translate(direction * (speed * Time.deltaTime));
            // this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);

            if (Vector3.Distance(this.transform.position, (nextCell.cellPosition.transform.position)) < treshold)
            {
                ChangeTarget();
            }
        }
    }

    
    void Initialize()
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

        this.transform.position = actualCell.cellPosition.transform.position;
        
        // this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);

        SearchNeighbour();
    }

    void ChangeTarget()
    {
        lastCell = actualCell;
        actualCell = nextCell;
        
        SearchNeighbour();
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

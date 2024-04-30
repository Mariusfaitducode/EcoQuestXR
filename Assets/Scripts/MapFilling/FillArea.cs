using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FillArea
{


    public static void GenerateAreaContent(Area area, int[,] roads)
    {
        Debug.Log("Generate Area Content : " + area.data.type);

        int count = 0;


        for (int i = 0; i < area.areaGrid.GetLength(0); i++)
        {
            for (int j = 0; j < area.areaGrid.GetLength(0); j++)
            {
                Vector3 newPosition = area.areaGrid[i, j].position;

                

                // Debug.Log("ROAD");
                // GameObject cube = GameObject.Instantiate(area.data.prefabs, newPosition, Quaternion.identity);
                // cube.transform.parent = area.sphere.transform;
                // count++;

                if (roads[i, j] == 1)
                {
                    if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                            area.uniformStartSize))
                    {
                        GameObject cube = GameObject.Instantiate(area.data.prefabs, newPosition, Quaternion.identity);
                        cube.transform.parent = area.sphere.transform;
                
                        count++;
                    }
                }
                else
                {
                    Debug.Log("NO ROAD");
                }


            }
        }



        // for (int i = 0; i < startVertices.Count; i++)
        // {
        //         
        //     count++;
        //     Vector3 newPosition = startVertices[i];
        //     newPosition.y = FillMapUtils.GetHeightFromRaycast(newPosition);
        //             
        //     Debug.Log(newPosition.y);
        //         
        //     GameObject cube = GameObject.Instantiate(area.data.prefabs, newPosition, Quaternion.identity);
        //     cube.transform.parent = area.sphere.transform;
        //         
        // }
        // Debug.Log(count + " prefabs instantiated");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FillArea
{


    public static void GenerateAreaContent(Area area, float scale)
    {
        Debug.Log("Generate Area Content : " + area.data.type);
        
        int count = 0;
        
        for (int i = 0; i < area.vertices.Count; i++)
        {
            if (Random.Range(0, 100) < 5)
            {
                count++;
                Vector3 newPosition = area.vertices[i] * scale;

                newPosition.y = FillMapUtils.GetHeightFromRaycast(newPosition);
                
                GameObject cube = GameObject.Instantiate(area.data.prefabs, newPosition, Quaternion.identity);
                cube.transform.parent = area.sphere.transform;
            }
        }
        Debug.Log(count + " prefabs instantiated");
    }
}

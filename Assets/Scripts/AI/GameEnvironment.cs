using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    //Reference to AI path
    private List<GameObject> waypoints = new();

    public List<GameObject> Waypoints { get { return waypoints; } }
    public static GameEnvironment Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new GameEnvironment();
                instance.Waypoints.AddRange(GameObject.FindGameObjectsWithTag("waypoint"));
            }
            return instance;
        }
    }

}

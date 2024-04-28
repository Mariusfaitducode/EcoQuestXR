using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnTable : MonoBehaviour
{
    private GameObject _table;
    private bool _haveTable = false;

    void Update()
    {
        if (!_haveTable)
        {
            _table = GameObject.FindGameObjectWithTag("Table");

            if (_table != null)
            {
                _haveTable = true;
                Vector3 tablePosition = _table.transform.position;
                transform.position = new Vector3(tablePosition.x, tablePosition.y, tablePosition.z);
                Debug.Log("Table found");
            }
            else
            {
                Debug.Log("Table not found");
            }
        }
        
    }

   
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectsInitialization
{
    public static List<ObjectProperties> InitializeObjectsProperties(string pathCSV)
    {
        List<ObjectProperties> objectProperties = new List<ObjectProperties>();
        
        DataCsv data = LoadDatas.ReadDataCSV(pathCSV);
        
        objectProperties = AffectDatasToObjectsProperties(data);
        
        // Debug.Log(objectProperties);
        
        return objectProperties;
    }
    
    
    public static List<ObjectProperties> AffectDatasToObjectsProperties(DataCsv data)
    {
        
        List<ObjectProperties> objectsProperties = new List<ObjectProperties>();


        foreach (string[] row in data.rows)
        {
            ObjectProperties objProps = new ObjectProperties();
            Stat stats = new Stat();

            // Object properties
            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();
                
                // Find SubObjectsIds
                if (data.header[i] == "subObjectsIds")
                {
                    row[i] = row[i].Trim();
                    
                    if (row[i] == "")
                    {
                        continue;
                    }
                    
                    string[] subObjectsIds = row[i].Split(';');
                    foreach (string subObjectId in subObjectsIds)
                    {
                        int id = int.Parse(subObjectId);
                        
                        if (id != null && id != -1)
                        {
                            objProps.subObjectsIds.Add(id);
                        }
                    }
                    continue;
                }


                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(ObjectProperties).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);
                    
                    propertyInfo.SetValue(objProps, value, null);
                }
            }
            
            //Stats
            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();
                
                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(Stat).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);
                    
                    propertyInfo.SetValue(stats, value, null);
                }
            }
            objProps.stats = stats;
            
            objectsProperties.Add(objProps);
        }

        return objectsProperties;
    }


    public static void InitializeObjectAlreadyOnArea(Area area, List<ObjectProperties> objectsProperties, GameManager gameManager)
    {

        foreach(GameObject areaObject in area.areaObjects)
        {
            ObjectScript objectScript = areaObject.GetComponent<ObjectScript>();
            
            // Debug.Log("Object name : " + areaObject.name);
            
            // string childName = areaObject.name.Replace("(Clone)", "").Trim();
            
            ObjectProperties objectProps = objectsProperties.Find(o => o.prefabName == areaObject.name);
            
            if (objectProps == null)
            {
                Debug.LogWarning("No object properties found for object : " + areaObject.name);
                continue;
            }
            
            objectScript.InitObjectScript(objectProps, gameManager);
        }

    }
    
    public static GameObject LoadPrefab(ObjectProperties objProps)
    {
        string path = "Prefabs/" + objProps.prefabPath + "/" + objProps.prefabName;
        GameObject prefab = Resources.Load<GameObject>(path);
        
        if (prefab == null)
        {
            Debug.LogWarning("No prefab found at : " + path);
        }
        return prefab;
    }
    
    
    public static List<SubObjects> ObjectsPropertiesToSubObjects(List<ObjectProperties> objectsProperties)
    {
        List<SubObjects> subObjectsProperties = new List<SubObjects>();
        
        foreach (ObjectProperties objProps in objectsProperties)
        {
            SubObjects subObject = new SubObjects();
            
            subObject.id = objProps.id;
            subObject.name = objProps.name;
            subObject.prefabName = objProps.prefabName;
            subObject.stats = objProps.stats;
            
            subObjectsProperties.Add(subObject);
        }
        
        return subObjectsProperties;
    }
    
    
    public static void LinkSubObjectsToObjects(List<ObjectProperties> objectsProperties, List<SubObjects> subObjectsProperties)
    {
        foreach (ObjectProperties objProps in objectsProperties)
        {
            foreach (int subObjectId in objProps.subObjectsIds)
            {
                SubObjects subObject = subObjectsProperties.Find(s => s.id == subObjectId);
                
                if (subObject == null)
                {
                    Debug.LogWarning("No subObject found for id : " + subObjectId);
                    continue;
                }
                objProps.subObjects.Add(subObject);
            }
        }
    }
    
}

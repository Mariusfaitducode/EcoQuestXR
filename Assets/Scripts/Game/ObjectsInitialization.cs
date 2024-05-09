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
        
        Debug.Log(objectProperties);
        
        return objectProperties;
    }
    
    
    public static List<ObjectProperties> AffectDatasToObjectsProperties(DataCsv data)
    {
        
        List<ObjectProperties> objectsProperties = new List<ObjectProperties>();


        foreach (string[] row in data.rows)
        {
            ObjectProperties objProps = new ObjectProperties();

            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();
                
                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(ObjectProperties).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);
                    
                    propertyInfo.SetValue(objProps, value, null);
                }
            }
            objectsProperties.Add(objProps);
        }

        return objectsProperties;
    }
    
    
    
}

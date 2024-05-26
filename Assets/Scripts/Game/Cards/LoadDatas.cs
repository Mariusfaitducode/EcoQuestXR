using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;


public struct DataCsv
{
    public string[] header;
    public List<string[]> rows;
        
    public DataCsv(string[] header, List<string[]> rows)
    {
        this.header = header;
        this.rows = rows;
    }
}

public class LoadDatas
{
    public static DataCsv ReadDataCSV(string pathCSV)
    {
        TextAsset csvData = Resources.Load<TextAsset>(pathCSV);

        if (csvData == null)
        {
            Debug.LogError("Le fichier CSV n'existe pas à l'emplacement spécifié : " + pathCSV);
            return new DataCsv(null, null);
        }
    
        // string[] rows = csvData.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] rows = csvData.text.Split('\n');
        Array.Resize(ref rows, rows.Length - 1);
        
        List<string[]> data = new List<string[]>();
        
        string[] header = rows[0].Split(',');

        for (int i = 1; i < rows.Length; i++)
        {
            string row = rows[i];
            string[] column = row.Split(',');
            
            data.Add(column);
        }

        return new DataCsv(header, data);
    }

    public static object MatchType(PropertyInfo propertyInfo, string[] row, string[] header, int index)
    {
        
        object value = null;
                
        if (propertyInfo.PropertyType.IsEnum)
        {
            value = Enum.Parse(propertyInfo.PropertyType, row[index], true);
        }
        else if (propertyInfo.PropertyType == typeof(int))
        {
            if (int.TryParse(row[index], out int parsedValue))
            {
                value = parsedValue;
            }
            else
            {
                Debug.LogError($"Failed to parse '{row[index]}' as int for property '{header[index]}'");
            }
        }
        else
        {
            value = Convert.ChangeType(row[index], propertyInfo.PropertyType);
        }
        // propertyInfo.SetValue(newCard, value, null);
        return value;
    }
}

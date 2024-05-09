using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



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
    
        string[] rows = csvData.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
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
}

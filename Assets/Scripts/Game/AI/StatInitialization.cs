using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;

public static class StatInitialization
{
    public static void LoadPopulationStatsFromCsv(string filePath, Probability healthProbs, Probability happinessProbs, Probability sensibilisationProbs, Probability distanceProbs, Probability salaryProbs)
    {
        // Load the CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);
        if (csvFile == null)
        {
            Debug.LogError($"File not found: {filePath}");
        }

        var lines = csvFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            switch (parts[0])
            {
                case "meanHealth":
                    healthProbs.mean = float.Parse(parts[1]);
                    break;
                case "stddevHealth":
                    healthProbs.stddev = float.Parse(parts[1]);
                    break;
                case "meanHappiness":
                    happinessProbs.mean = float.Parse(parts[1]);
                    break;
                case "stddevHappiness":
                    happinessProbs.stddev = float.Parse(parts[1]);
                    break;
                case "meanSensibilisation":
                    sensibilisationProbs.mean = float.Parse(parts[1]);
                    break;
                case "stddevSensibilisation":
                    sensibilisationProbs.stddev = float.Parse(parts[1]);
                    break;
                case "meanDistanceToWorkplace":
                    distanceProbs.mean = float.Parse(parts[1]);
                    break;
                case "stddevDistanceToWorkplace":
                    distanceProbs.stddev = float.Parse(parts[1]);
                    break;
                case "meanSalary":
                    salaryProbs.mean = float.Parse(parts[1]);
                    break;
                case "stddevSalary":
                    salaryProbs.stddev = float.Parse(parts[1]);
                    break;
                default:
                    Debug.LogError("Unknown probability type: " + parts[0]);
                    break;
            }
        }
    }

    public static void LoadTransportPonderationsFromCsv(string pathCSV, List<TransportMode> transportModes)
    {
        // Load CSV file
        DataCsv data = LoadDatas.ReadDataCSV(pathCSV);
        
        // Affect CSV data to TransportMode objects
        foreach (string[] row in data.rows)
        {
            TransportMode transportMode = new TransportMode();
            
            // Get id and name of the transport mode
            transportMode.id = int.Parse(row[0]);
            transportMode.name = row[1];
            
            // Create ponderation object
            
            Ponderation ponderation = new Ponderation();
            
            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();
                
                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(Ponderation).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);
                    
                    propertyInfo.SetValue(ponderation, value, null);
                }
                else
                {
                    if (data.header[i] != "name" && data.header[i] != "id")
                    {
                        Debug.LogError("Could not find the property " + data.header[i] + " in the ponderation object.");
                    }
                }
            }
            // Check errors
            if (transportMode.name == null)
            {
                Debug.LogError("Transport mode name is null");
            }
            transportMode.ponderation = ponderation;
            transportModes.Add(transportMode);
        }
        
        Debug.Log("Count of transport modes: " + transportModes.Count);
    }

    public static void LoadTransportStatsFromCsv(string pathCSV, List<TransportMode> transportModes)
    {
        // Load CSV file
        DataCsv data = LoadDatas.ReadDataCSV(pathCSV);

        // Affect CSV data to TransportMode objects
        foreach (string[] row in data.rows)
        {
            // Get string name of the stat to affect to the correposnding TransportMode
            string name = row[1];

            if (name == null)
            {
                Debug.LogError("In transport ponderations CSV, could not find the name of the transport mode in 2nd column of the row.");
            }

            // Get associated transport mode
            TransportMode transportMode = StatUtils.GetTransportModeByName(transportModes, name);
            
            Stat stats = new Stat();
            stats.Reset();

            // Affect stats to the transport mode
            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();

                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(Stat).GetProperty(data.header[i],
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);

                    propertyInfo.SetValue(stats, value, null);
                }
            }

            transportMode.stats = stats;
        }
    }
}

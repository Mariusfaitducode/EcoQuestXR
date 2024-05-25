using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;

public static class StatInitialization
{
    public static void LoadPopulationStatsFromCsv(string filePath, Probability healthProbs, Probability happinessProbs, Probability sensibilisationProbs, Probability distanceProbs, Probability salaryProbs)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split('\t');
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
            
            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();
                
                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(TransportMode).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);
                    
                    propertyInfo.SetValue(transportMode, value, null);
                }
            }
            // Check errors
            if (transportMode.name == null)
            {
                Debug.LogError("Transport mode name is null");
            }
            
            transportModes.Add(transportMode);
        }
    }

    public static void LoadTransportStatsFromCsv(string pathCSV, List<TransportMode> transportModes)
    {
        // Load CSV file
        DataCsv data = LoadDatas.ReadDataCSV(pathCSV);

        // Affect CSV data to TransportMode objects
        foreach (string[] row in data.rows)
        {
            Stat stats = new Stat();
            stats.Reset();

            // Get string name of the stat to affect to the correposnding TransportMode
            PropertyInfo nameInfo = typeof(TransportMode).GetProperty("name",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (nameInfo == null)
            {
                Debug.LogError("In transport ponderations CSV, could not find the name of the transport mode");
            }

            // Get associated transport mode
            TransportMode transportMode = StatUtils.GetTransportModeByName(transportModes, nameInfo.ToString());

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

            transportModes.Add(transportMode);
        }
    }
}

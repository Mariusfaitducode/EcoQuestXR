using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

public class CardsInitialization
{
    
    
    public static List<Card> InitializeCards(string pathCSV)
    {
        List<Card> cards = new List<Card>();
        
        DataCsv data = LoadDatas.ReadDataCSV(pathCSV);
        
        cards = AffectDatasToCards(data);
        
        Debug.Log(cards);
        
        return cards;
    }
    
    
    public static List<Card> AffectDatasToCards(DataCsv data)
    {
        
        List<Card> cards = new List<Card>();


        foreach (string[] row in data.rows)
        {
            Card newCard = new Card();

            for (int i = 0; i < data.header.Length; i++)
            {
                data.header[i] = data.header[i].Trim();
                
                PropertyInfo propertyInfo = typeof(Card).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = null;
                    
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        value = Enum.Parse(propertyInfo.PropertyType, row[i], true);
                    }
                    else if (propertyInfo.PropertyType == typeof(int))
                    {
                        if (int.TryParse(row[i], out int parsedValue))
                        {
                            value = parsedValue;
                        }
                        else
                        {
                            Debug.LogError($"Failed to parse '{row[i]}' as int for property '{data.header[i]}'");
                            continue;
                        }
                    }
                    else
                    {
                        value = Convert.ChangeType(row[i], propertyInfo.PropertyType);
                    }
                    propertyInfo.SetValue(newCard, value, null);
                }
            }
            cards.Add(newCard);
        }

        return cards;
    }



    
    
}

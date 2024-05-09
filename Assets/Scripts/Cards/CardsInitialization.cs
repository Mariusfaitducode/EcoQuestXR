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
        
        return AffectDatasToCards(data);
    }
    
    
    public static List<Card> AffectDatasToCards(DataCsv data)
    {
        
        List<Card> cards = new List<Card>();


        foreach (string[] row in data.rows)
        {
            Card newCard = new Card();

            for (int i = 0; i < data.header.Length; i++)
            {
                PropertyInfo propertyInfo = typeof(Card).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    propertyInfo.SetValue(newCard, Convert.ChangeType(row[i], propertyInfo.PropertyType), null);
                }
            }
            cards.Add(newCard);
        }

        return cards;
    }



    
    
}

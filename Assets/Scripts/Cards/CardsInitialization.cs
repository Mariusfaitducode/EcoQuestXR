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
                
                // Match Card class to Csv header
                PropertyInfo propertyInfo = typeof(Card).GetProperty(data.header[i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (propertyInfo != null && row.Length > i)
                {
                    object value = LoadDatas.MatchType(propertyInfo, row, data.header, i);
                    
                    propertyInfo.SetValue(newCard, value, null);
                }
            }
            cards.Add(newCard);
        }

        return cards;
    }


    public static void MatchCardWithObjectProperties(List<Card> cards, List<ObjectProperties> objectsProperties)
    {
        
        // Set objectProperties to cards
        
        foreach (Card card in cards)
        {
            foreach (ObjectProperties objectProperties in objectsProperties)
            {
                if (card.idObject1 == objectProperties.id)
                {
                    card.objectProperties1 = objectProperties;
                }
                if (card.idObject2 == objectProperties.id)
                {
                    card.objectProperties2 = objectProperties;
                }
            }
        }
        
        // Calculate CardProperties
        
        // To change for a good calculation
        foreach (Card card in cards)
        {
            if (card.objectProperties1 != null)
            {
                card.cardProperties.price = card.objectProperties1.price * card.quantityObject1;
                card.cardProperties.ecology = card.objectProperties1.ecology * card.quantityObject1;
                card.cardProperties.population = card.objectProperties1.population * card.quantityObject1;
                card.cardProperties.energy = card.objectProperties1.energy * card.quantityObject1;
                card.cardProperties.pollution = card.objectProperties1.pollution * card.quantityObject1;
            }
            else if (card.objectProperties2 != null)
            {
                card.cardProperties.price -= card.objectProperties2.price * card.quantityObject2;
                card.cardProperties.ecology -= card.objectProperties2.ecology * card.quantityObject2;
                card.cardProperties.population -= card.objectProperties2.population * card.quantityObject2;
                card.cardProperties.energy -= card.objectProperties2.energy * card.quantityObject2;
                card.cardProperties.pollution -= card.objectProperties2.pollution * card.quantityObject2;
            }
        }
    }
}

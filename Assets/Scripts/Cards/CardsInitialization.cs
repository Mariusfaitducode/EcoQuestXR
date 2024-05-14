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
        
        PrintCards(cards);
        
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


    public static void MatchCardWithObjectProperties(List<Card> cards, List<ObjectProperties> objectsProperties, float factorCostReductionDestruction)
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
        
        // Set areaType to cards
        foreach (Card card in cards)
        {
            card.areaType = card.objectProperties1.areaType;
        }

        // Calculate CardProperties
        // TODO :  Improve card stats calculation
        foreach (Card card in cards)
        {
            if (card.cardType == CardType.Construction)
            {
                card.cardProperties.price = card.objectProperties1.price * card.quantityObject1;
                card.cardProperties.ecology = card.objectProperties1.ecology * card.quantityObject1;
                card.cardProperties.population = card.objectProperties1.population * card.quantityObject1;
                card.cardProperties.energy = card.objectProperties1.energy * card.quantityObject1;
                card.cardProperties.pollution = card.objectProperties1.pollution * card.quantityObject1;
            }
            else if (card.cardType == CardType.Destruction)
            {
                card.cardProperties.price = (int)(card.objectProperties1.price * card.quantityObject1 * factorCostReductionDestruction);
                card.cardProperties.ecology = - card.objectProperties1.ecology * card.quantityObject1;
                card.cardProperties.population = - card.objectProperties1.population * card.quantityObject1;
                card.cardProperties.energy = - card.objectProperties1.energy * card.quantityObject1;
                card.cardProperties.pollution = - card.objectProperties1.pollution * card.quantityObject1;
            }
            else
            {
                Debug.LogError("Card type " + card.cardType + " not implemented");
            }
        }
    }
    
    public static void PrintCards(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            Debug.Log(card.areaType);
        }
    }
}

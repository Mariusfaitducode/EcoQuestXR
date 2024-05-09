using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType {
    Construction,
    Destruction,
    Upgrade,
    Rules,
    Event
}

public class Card
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    
    public CardType cardType { get; set; }
    
    public string id_object1 { get; set; }
    public string id_object2 { get; set; }
    public string quantity_object1 { get; set; }
    public string quantity_object2 { get; set; }
    
    
    // public AreaType areaType;
    // public CardAction action;
    //
    // public string energy;
    // public string ecology;
    // public string population;
    // public string pollution;
    
    // public string price;



    public Card(string title, string price, string description, AreaType areaType, ActionType action, string energy, string ecology, string population, string pollution)
    {
        this.title = title;
        this.price = price;
        this.description = description;
        this.areaType = areaType;
        this.action = action;
        this.energy = energy;
        this.ecology = ecology;
        this.population = population;
        this.pollution = pollution;
    }

    public class CardProperties
    {
        
    }
    

}

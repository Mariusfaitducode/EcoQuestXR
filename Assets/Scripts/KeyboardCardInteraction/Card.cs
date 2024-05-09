using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardAction {
    Construction,
    Destruction,
    Upgrade,
    Rules,
    Event
}

public class Card
{ 
    public string title;
    public string price;
    public string description;
    
    public AreaType areaType;
    public CardAction action;
    
    public string energy;
    public string ecology;
    public string population;
    public string pollution;


    // public Card(string title, string price, string description, AreaType areaType, CardAction action, string energy, string ecology, string population, string pollution)
    // {
    //     this.title = title;
    //     this.price = price;
    //     this.description = description;
    //     this.areaType = areaType;
    //     this.action = action;
    //     this.energy = energy;
    //     this.ecology = ecology;
    //     this.population = population;
    //     this.pollution = pollution;
    // }

    public class CardProperties
    {
        
    }
    

}

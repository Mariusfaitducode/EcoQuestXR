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
    
    
    public AreaType areaType;
    // public CardAction action;
    //
    // public string energy;
    // public string ecology;
    // public string population;
    // public string pollution;
    
    // public string price;
    

    public class CardProperties
    {
        
    }
    

}

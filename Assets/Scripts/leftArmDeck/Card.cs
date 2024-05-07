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
    
    public AreaType areaType;
    public CardAction action;
    

    public int id;
    public string description;
    public string area;


    public Card()
    {
        
    }
    
    

}

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
    public AreaType areaType;
    
    public int idObject1 { get; set; }
    public int idObject2 { get; set; }
    public int quantityObject1 { get; set; }
    public int quantityObject2 { get; set; }
    public ObjectProperties objectProperties1;
    public ObjectProperties objectProperties2;
    
    public GameStats cardProperties = new GameStats();
    
    public GameObject cardObject;
    
    public void SetCardObject(GameObject cardObject)
    {
        this.cardObject = cardObject;
    }
    public GameObject GetCardObject()
    {
        return cardObject;
    }
    

}

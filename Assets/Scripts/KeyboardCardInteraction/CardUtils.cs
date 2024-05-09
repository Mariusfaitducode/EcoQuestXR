using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardUtils
{
    public static List<GameObject> GetPanelsLocation(Canvas canvas)
    {
        // Get all panels
        List<GameObject> cardLocationPanels = new List<GameObject>();
        foreach (Transform child in canvas.transform)
        {
            if (child.CompareTag("Location"))
            {
                cardLocationPanels.Add(child.gameObject);
            }
        }
        if (cardLocationPanels.Count == 0)
        {
            Debug.LogError("No location cards found in draw pile : you need to set it up in the scene");
        }
        return cardLocationPanels;
    }
    
    public static void DeleteCards(List<GameObject> cardLocationPanels)
    {
        foreach (GameObject panel in cardLocationPanels)
        {
            foreach (Transform child in panel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
    
}

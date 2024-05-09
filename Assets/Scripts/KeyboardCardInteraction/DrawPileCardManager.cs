using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawPileCardManager : MonoBehaviour
{
    private List<Card> _cardsPile;
    private List<GameObject> _cardLocationPanels;
    private List<Card> _selectedCards;
    
    
    public Canvas canvas;
    public GameObject cardPrefab;
    public GameObject counterTextObject;
    public LeftArmDeckCardManager leftArmDeckCardManager;
    private TextMeshProUGUI counterText;
    
    public int nbrMaxSelectedCards = 3;
    private int _nbrSelectedCards = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        GetCounterText();
        CreateSomeCards();
        _cardLocationPanels = CardUtils.GetPanelsLocation(canvas);
        CardUtils.DeleteCards(_cardLocationPanels);
        DrawCards();
        UpdateCounter();
    }
    private void GetCounterText()
    {
        counterText = counterTextObject.GetComponent<TextMeshProUGUI>();
    }
    public bool CanSelectCard()
    {
        return _nbrSelectedCards < nbrMaxSelectedCards;
    }

    public void Validate()
    {
        Debug.Log("Validate");
        _selectedCards = new List<Card>();
        foreach (GameObject panel in _cardLocationPanels)
        {
            DisplayCard displayCard =  panel.transform.GetComponentInChildren<DisplayCard>();
            if (displayCard.IsSelected())
                _selectedCards.Add(displayCard.GetCard());
        }
        leftArmDeckCardManager.GetDraftedCards(_selectedCards);
        this.gameObject.SetActive(false);
    }
    public void AddSelectedCard()
    {
        _nbrSelectedCards++;
        UpdateCounter();
    }
    public void RemoveSelectedCard()
    {
        _nbrSelectedCards--;
        UpdateCounter();
    }
    private void CreateSomeCards()
    {
        _cardsPile = new List<Card>();
        
        // _cardsPile.Add(new Card(
        //     "Création d'un quartier moderne", 
        //     "1.4M", 
        //     "Construction de 4 maisons modernes et isolées",
        //     AreaType.City,
        //     CardType.Construction,
        //     "-200/mois",
        //     "-100",
        //     "+16",
        //     "+100"));
        //
        // _cardsPile.Add(new Card(
        //     "Destruction d'un HLM", 
        //     "250K", 
        //     "Destruction de 1 HLM de 34 logements",
        //     AreaType.City,
        //     CardType.Destruction,
        //     "+500/mois",
        //     "+200",
        //     "-100",
        //     "-300"));
        //
        // _cardsPile.Add(new Card(
        //     "Contruction d'éoliennes", 
        //     "2M", 
        //     "Construction d'un parc de 5 éoliennes",
        //     AreaType.Energy,
        //     CardType.Construction,
        //     "+1000/mois",
        //     "-50",
        //     "+0",
        //     "+50"));
        //
        // _cardsPile.Add(new Card(
        //     "Destruction d'un centrale à charbon", 
        //     "600K", 
        //     "Destruction d'un centrale à charbon",
        //     AreaType.Energy,
        //     CardType.Destruction,
        //     "-3000/mois",
        //     "+500",
        //     "+0",
        //     "-300"));
    }
    
    private void DrawCards()
    {
        int idx = 0;
        foreach (GameObject panel in _cardLocationPanels)
        {
            GameObject card = Instantiate(cardPrefab, panel.transform);
            DisplayCard displayCard =  card.GetComponent<DisplayCard>();
            displayCard.SetCard(_cardsPile[idx]);
            displayCard.SetDrawPileCardManager(this);
            
            // card.GetComponent<DisplayCard>().SetCard(_cardsPile[Random.Range(0, _cardsPile.Count)]);
            idx++;
        }
    }
    private void UpdateCounter()
    {
        counterText.text = _nbrSelectedCards + " / " + nbrMaxSelectedCards;
    }
}

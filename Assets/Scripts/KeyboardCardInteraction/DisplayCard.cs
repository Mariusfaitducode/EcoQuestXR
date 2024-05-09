using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DisplayCard : MonoBehaviour
{
    public string folderActionImage = "Images/ActionImage/";
    public string folderActionLogo = "Images/ActionLogo/";
    public string folderAreaBackground = "Images/AreaBackground/";
    public string folderAreaLogo = "Images/AreaLogo/";
    
    private Card _card;
    private bool isSelected = false;
    private DrawPileCardManager _drawPileCardManager;
    private LeftArmDeckCardManager _leftArmDeckCardManager;

    private TextMeshProUGUI _title;
    private TextMeshProUGUI _description;
    private TextMeshProUGUI _price;
    private RawImage _image;

    private RectTransform _scale;
    private Button _button;
    private RawImage _shadow;
    private RawImage _background;
    private RawImage _logoArea;
    private RawImage _logoAction;

    private TextMeshProUGUI _energy;
    private TextMeshProUGUI _ecology;
    private TextMeshProUGUI _population;
    private TextMeshProUGUI _pollution;
    
    // Start is called before the first frame update
    void Start()
    {
        GetChilds();
        SetButtonListener();
        GetCardInformations();
    }
    public void SelectUnselectFromDrawPileCardManager()
    {
        if (_drawPileCardManager.CanSelectCard() || isSelected)
        {
            SelectUnselect();
            if (isSelected)
            {
                _drawPileCardManager.AddSelectedCard();
            }
            else
            {
                _drawPileCardManager.RemoveSelectedCard();
            }
        }
    }
    public void SelectUnselectFromLeftArmDeckManager()
    {
        _leftArmDeckCardManager.UnSelectAll();
        SelectUnselect();
    }
    private void SelectUnselect()
    {
        isSelected =! isSelected;
        if (isSelected)
        {
            _shadow.enabled = true;
            _scale.localScale = new Vector3(1.12f, 1.12f, 1.12f);
        }
        else
        {
            _shadow.enabled = false;
            _scale.localScale = new Vector3(1f, 1f, 1f);
        }

    }
    public void Unselect()
    {
        isSelected = false;
        _shadow.enabled = false;
        _scale.localScale = new Vector3(1f, 1f, 1f);
    }
    public void SetDrawPileCardManager(DrawPileCardManager drawPileCardManager)
    {
        _drawPileCardManager = drawPileCardManager;
    }
    private void SetButtonListener()
    {
        if (_drawPileCardManager != null)
        {
            _button.onClick.AddListener(SelectUnselectFromDrawPileCardManager);
        }
        else
        {
            _button.onClick.AddListener(SelectUnselectFromLeftArmDeckManager);
        }
    }
    public void SetLeftArmDeckCardManager(LeftArmDeckCardManager leftArmDeckCardManager)
    {
        _leftArmDeckCardManager = leftArmDeckCardManager;
    }

    public Card GetCard()
    {
        return _card;
    }
    private void GetChilds()
    {
        _title = FindWithTag<TextMeshProUGUI>("TitleText");
        _description = FindWithTag<TextMeshProUGUI>("DescriptionText");
        _price = FindWithTag<TextMeshProUGUI>("PriceText");
        _image = FindWithTag<RawImage>("ImageRawImage");
        
        
        _shadow = GetComponent<RawImage>();
        _button = FindWithTag<Button>("BackgroundRawImage");
        _scale = FindWithTag<RectTransform>("BackgroundRawImage");
        _background = FindWithTag<RawImage>("BackgroundRawImage");
        _logoArea = FindWithTag<RawImage>("LogoAreaRawImage");
        _logoAction = FindWithTag<RawImage>("LogoActionRawImage");
        
        _energy = FindWithTag<TextMeshProUGUI>("EnergyText");
        _ecology = FindWithTag<TextMeshProUGUI>("EcologyText");
        _population = FindWithTag<TextMeshProUGUI>("PopulationText");
        _pollution = FindWithTag<TextMeshProUGUI>("PollutionText");
    }
    private void GetCardInformations()
    {
        if (_card != null)
        {
            _title.text = _card.title;
            _description.text = _card.description;
            _price.text = _card.price;
            _image.texture = GetTexture(folderActionImage, _card.action.ToString());
        
        
            _background.texture = GetTexture(folderAreaBackground, _card.areaType.ToString());
            _logoArea.texture = GetTexture(folderAreaLogo, _card.areaType.ToString());
            _logoAction.texture = GetTexture(folderActionLogo, _card.action.ToString());
        
            _energy.text = _card.energy;
            _ecology.text = _card.ecology;
            _population.text = _card.population;
            _pollution.text = _card.pollution;
        }
    }
    public void SetCard(Card card)
    {
        _card = card;
    }
    public bool IsSelected()
    {
        return isSelected;
    }
    private Texture2D GetTexture(string folder, string type)
    {
        string path = folder + type;
        Texture2D texture2D = Resources.Load<Texture2D>(path);
        if (texture2D == null)
        {
            Debug.LogError("Could not find texture2D in the folder Resources at path : " + path);
            return null;
        }
        return texture2D;
    }
    private T FindWithTag<T>(string tag) where T : Component
    {
        GameObject obj =  FindWithTagRecursive(transform, tag);
        if (obj == null)
        {
            Debug.LogError("Could not find object with tag " + tag);
            return null;
        }
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Could not find component of type " + typeof(T) + " on object with tag " + tag);
            return null;
        }
        return component;
    }
    GameObject FindWithTagRecursive(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            GameObject obj = FindWithTagRecursive(child, tag);
            if (obj != null)
            {
                return obj;
            }
        }

        return null;
    }
}

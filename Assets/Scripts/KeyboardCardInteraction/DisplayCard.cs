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
    private Canvas _parentCanvas;
    // private PileManager _pileManager;
    // private DisplayCanvas _displayCanvas;

    public TextMeshProUGUI _title;
    public TextMeshProUGUI _description;
    public TextMeshProUGUI _price;
    public RawImage _image;

    public RectTransform _scale;
    public Button _button;
    public RawImage _shadow;
    public RawImage _background;
    public RawImage _logoArea;
    public RawImage _logoAction;

    public TextMeshProUGUI _energy;
    public TextMeshProUGUI _ecology;
    public TextMeshProUGUI _population;
    public TextMeshProUGUI _pollution;
    
    // Start is called before the first frame update
    
    public void Select()
    {
        isSelected = true;
        _shadow.enabled = true;
        _scale.localScale = new Vector3(1.12f, 1.12f, 1.12f);
    }

    public Canvas GetParentCanvas()
    {
        return _parentCanvas;
    }
    public void SetCanvas(Canvas canvas)
    {
        _parentCanvas = canvas;
    }
    public void Unselect()
    {
        isSelected = false;
        _shadow.enabled = false;
        _scale.localScale = new Vector3(1f, 1f, 1f);
    }
    // public void SetDrawPileCardManager(PileManager pileManager)
    // {
    //     _pileManager = pileManager;
    // }
    // private void SetButtonListener()
    // {
    //     if (_pileManager != null)
    //     {
    //         _button.onClick.AddListener(SelectUnselectFromDrawPileCardManager);
    //     }
    //     else
    //     {
    //         _button.onClick.AddListener(SelectUnselectFromLeftArmDeckManager);
    //     }
    // }
    // public void SetLeftArmDeckCardManager(DisplayCanvas displayCanvas)
    // {
    //     _displayCanvas = displayCanvas;
    // }

    public Button GetButton()
    {
        return _button;
    }
    public Card GetCard()
    {
        return _card;
    }
    
    private void SetCardInformations()
    {
        if (_card != null)
        {
            _title.text = _card.title;
            _description.text = _card.description;
            _price.text = _card.cardProperties.price.ToString();
            _image.texture = GetTexture(folderActionImage, _card.cardType.ToString());
            
            
            _background.texture = GetTexture(folderAreaBackground, _card.areaType.ToString());
            _logoArea.texture = GetTexture(folderAreaLogo, _card.areaType.ToString());
            _logoAction.texture = GetTexture(folderActionLogo, _card.cardType.ToString());
            
            _energy.text = _card.cardProperties.energy.ToString();
            _ecology.text = _card.cardProperties.ecology.ToString();
            _population.text = _card.cardProperties.population.ToString();
            _pollution.text = _card.cardProperties.pollution.ToString();
        }
    }
    public void SetCard(Card card)
    {
        _card = card;
        SetCardInformations();
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

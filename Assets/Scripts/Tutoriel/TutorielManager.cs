using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorielManager : MonoBehaviour
{

    private int index_slide = 0;
    private Image image;

    private Transform prev_button; 
    private Transform next_button; 
    
    public List<Sprite> list_slides = new List<Sprite>();


    // Start is called before the first frame update
    void Start()
    {

        image = this.gameObject.transform.GetChild(0).GetComponent<Image>(); 
        prev_button = this.gameObject.transform.GetChild(2);
        next_button = this.gameObject.transform.GetChild(1);

        prev_button.gameObject.SetActive(false);

        this.transform.position = Camera.main.transform.position + new Vector3(0, 0.5f, 3);
    }

    // Update is called once per frame
    void Update()
    {
        



    }


    public void next_slide() { 
        
        if (index_slide < list_slides.Count - 1) index_slide++; // double vérif

        if (index_slide == 1) prev_button.gameObject.SetActive(true);
        if (index_slide == list_slides.Count - 1) next_button.gameObject.SetActive(false);

        image.sprite = list_slides[index_slide];  

    }


    public void prev_slide()
    {

        if (index_slide > 0) index_slide--; // double vérif

        if (index_slide == 0) prev_button.gameObject.SetActive(false);
        if (index_slide == list_slides.Count - 2) next_button.gameObject.SetActive(true);


        image.sprite = list_slides[index_slide];

    }

}

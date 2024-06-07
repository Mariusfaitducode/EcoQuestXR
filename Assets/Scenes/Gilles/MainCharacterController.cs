using Oculus.Interaction.DebugTree;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{

    private float intensity_color = 0;

    private Material character_mat = null;

    private Color maxValColor = Color.white;
    private Color minValColor = new Color(0.3f, 0.3f, 0.3f);

    private Dictionary<int, string> list_text = new Dictionary<int, string>();

    private TextMeshProUGUI text; 

    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {

        character_mat = this.GetComponentInChildren<SkinnedMeshRenderer>().material;
        text = this.GetComponentInChildren<TextMeshProUGUI>();
        audioSource = this.GetComponent<AudioSource>();

        list_text.Add(0, "Bienvenu dans EcoQuest, laisse moi te guider pour tes premiers pas");
        list_text.Add(1, "Est-ce que tu veux bien m'aider à survivre ?");
        list_text.Add(2, "SAUVE MOI ET SAUVE LA VILLE");
        list_text.Add(3, "");
        list_text.Add(4, "");

        text.text = list_text[0];

    }

    // Update is called once per frame
    void Update()
    {


    }

    
    public void ChangeValue(float new_val)
    {

        if (new_val < 100 && new_val > 0)
        {
            intensity_color = new_val;
            character_mat.SetColor("_BaseColor", Color.Lerp(minValColor, maxValColor, intensity_color / 100f));
        }

    }



    public void ChangeText(int new_text_id)
    {
        if (list_text.ContainsKey(new_text_id))
        {
            text.text = list_text[new_text_id];
            audioSource.Play();
        }
    }

}

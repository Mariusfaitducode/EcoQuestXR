using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CloudController : MonoBehaviour
{
    public ObjectPose ovrPose;
    
    internal Transform meshTransform;
    internal UpdateTerrainRenderer updateTerrainRenderer;
    internal Transform centerEyeAnchorTransform;

    private float intensity_color = 0;

    private Material character_mat = null;

    private Color maxValColor = Color.white;
    private Color minValColor = new Color(0.3f, 0.3f, 0.3f);

    private Dictionary<string, string> list_text = new Dictionary<string, string>();

    private TextMeshProUGUI text; 
    private Canvas canva; 

    private AudioSource audioSource;

    public List<AudioClip> clipList = new List<AudioClip>();

    public void InitializeCloudController()
    {
        transform.rotation = ovrPose.rotation;
        transform.localScale = new Vector3(ovrPose.scale, ovrPose.scale, ovrPose.scale);
    
    
        character_mat = this.GetComponentInChildren<SkinnedMeshRenderer>().material;
        text = this.GetComponentInChildren<TextMeshProUGUI>();
        canva = this.GetComponentInChildren<Canvas>();
        audioSource = this.GetComponent<AudioSource>();

        list_text.Add("play_card3", "Tr�s bien je m'en occupe");
        list_text.Add("play_card2", "Tr�s bon choix !");
        list_text.Add("play_card1", "Un pas de plus vers le bonheur !");
        list_text.Add("random2", "N'h�site pas � aller voir le tutoriel");
        list_text.Add("random1", "Ca va ? Tu t'en sors ?");
        list_text.Add("win2", "Bravo ! Gr�ce � toi la ville est sauv�e !");
        list_text.Add("win1", "Je compte sur toi pour sauver ta vraie plan�te maintenant !");
        list_text.Add("lose2", "Oh non, j'ai l'impression que nous n'avons pas r�ussi � sauver la ville");
        list_text.Add("lose1", "Je compte sur toi pour r��ssayer tr�s bient�t");


        text.text = "";
    
    }


    
    void Update()
    {
        Vector3 shaderCenterMapPosition = updateTerrainRenderer.GetMapCenter();
        Vector3 centerMapPosition = new Vector3(shaderCenterMapPosition.x, meshTransform.position.y, shaderCenterMapPosition.z);
        transform.position = centerMapPosition + ovrPose.position;


        canva.enabled = audioSource.isPlaying;

    
        // TODO : use lookat
        // transform.rotation = meshTransform.rotation;
    }



        public void ChangeValue(float new_val)
    {

        if (new_val < 100 && new_val > 0)
        {
            intensity_color = new_val;
            character_mat.SetColor("_BaseColor", Color.Lerp(minValColor, maxValColor, intensity_color / 100f));
        }

    }



    public void ChangeText(string new_text_id)
    {
        if (list_text.ContainsKey(new_text_id))
        {
            canva.enabled = true;
            text.text = list_text[new_text_id];

            // Rechercher l'audio clip correspondant
            AudioClip matchingClip = null;
            foreach (AudioClip clip in clipList)
            {
                if (clip.name == new_text_id)
                {
                    matchingClip = clip;
                    break;
                }
            }

            // Si un audio clip correspondant est trouv�, le jouer
            if (matchingClip != null)
            {
                audioSource.clip = matchingClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Aucun audio clip correspondant trouv� pour " + new_text_id);
            }

        }
    }
}

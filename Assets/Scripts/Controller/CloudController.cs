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


    // variables pour le texte
    private Dictionary<string, string> list_text = new Dictionary<string, string>();

    private TextMeshProUGUI text; 
    private Canvas canva; 


    // variables pour le son
    private AudioSource audioSource;
    public List<AudioClip> clipList = new List<AudioClip>();


    // variables pour l'animation 
    private float moveSpeed = 1.0f;  // La vitesse de déplacement
    private float stopDistance = 0.05f;  // La distance minimale à laquelle la zone de dépôt doit s'arrêter

    private Vector3 movementOffset = Vector3.zero;  // Le vecteur de déplacement
    internal bool is_moving = false;


    public void InitializeCloudController()
    {
        transform.rotation = ovrPose.rotation;
        transform.localScale = new Vector3(ovrPose.scale, ovrPose.scale, ovrPose.scale);


        character_mat = this.GetComponentInChildren<SkinnedMeshRenderer>().material;
        text = this.GetComponentInChildren<TextMeshProUGUI>();
        canva = this.GetComponentInChildren<Canvas>();
        audioSource = this.GetComponent<AudioSource>();

        list_text.Add("play_card3", "Tres bien je m'en occupe");
        list_text.Add("play_card2", "Tres bon choix !");
        list_text.Add("play_card1", "Un pas de plus vers le bonheur !");

        list_text.Add("random2", "N'hesite pas a aller voir le tutoriel");
        list_text.Add("random1", "Ca va ? Tu t'en sors ?");

        list_text.Add("win2", "Bravo ! Grace a toi la ville est sauvee !");
        list_text.Add("win1", "Je compte sur toi pour sauver ta vraie planete maintenant !");

        list_text.Add("lose2", "Oh non, j'ai l'impression que nous n'avons pas reussi a sauver la ville");
        list_text.Add("lose1", "Je compte sur toi pour reessayer tres bientot");

        list_text.Add("notEnoughMoney1", "Desole mais tu n'as pas assez d'argent pour ça");
        list_text.Add("notEnoughMoney2", "Tu devrais attendre de gagner un peu plus avant de jouer");

        list_text.Add("help1", "Tu peux attraper la carte pour la deplacer avec le bouton de grip");
        list_text.Add("help2", "Utilises les joysticks pour te deplacer sur la carte");

        text.text = "";

    }


    
    void Update()
    {

        canva.enabled = audioSource.isPlaying;

        Vector3 shaderCenterMapPosition = updateTerrainRenderer.GetMapCenter();
        Vector3 centerMapPosition = new Vector3(shaderCenterMapPosition.x, meshTransform.position.y, shaderCenterMapPosition.z);
        transform.position = centerMapPosition + ovrPose.position + (movementOffset * 2f);


        if (is_moving)
        {
            MoveToDestination();
        } 
        else
        {
            ReturnToPosition();
        }

    
        // TODO : use lookat
        this.transform.LookAt(centerEyeAnchorTransform);
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

    public void MoveToDestination()
    {
        // Calculer la distance entre la zone de dépôt et la position cible
        float distance = Vector3.Distance(this.transform.position + movementOffset, centerEyeAnchorTransform.position);

        // Si la distance est supérieure à la distance d'arrêt, calculer le vecteur de déplacement
        if (distance > stopDistance)
        {
            Vector3 direction = (centerEyeAnchorTransform.position - (this.transform.position + movementOffset)).normalized;
            movementOffset += direction * moveSpeed * Time.deltaTime;
        }
    }

    public void ReturnToPosition()
    {
        // Vérifier si la magnitude du vecteur de déplacement est supérieure à une petite valeur
        if (movementOffset.magnitude > 0.01f)
        {
            Vector3 direction = (-movementOffset).normalized;
            movementOffset += direction * moveSpeed * Time.deltaTime;
        } 
        else
        {
            movementOffset = Vector3.zero;
        }
    }


}

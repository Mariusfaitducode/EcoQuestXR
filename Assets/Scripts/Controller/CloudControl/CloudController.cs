using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum CloudEvent
{
    playCard,
    firstPlay,
    random,
    lose,
    notEnoughMoney,
    help,
    loseMoney, 
    loseEnergy, 
    loseSociety,
    winEcology, 
    warningMoney,
    warningEnergy,
    warningSociety,
    warningEcology,
    encouragementEcology
}

[Serializable]
public struct TextAudio
{
    public string text;
    public AudioClip audioClip;
}

[Serializable]
public struct CloudText
{
    public CloudEvent cloudEvent;
    
    public List<TextAudio> textsAudios;
}

public class CloudDone
{
    public CloudEvent cloudEvent;
    public bool done;
}


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
    internal List<CloudDone> cloudDones = new List<CloudDone>();
    private bool isDisplayingText = false;

    // variables pour le son
    private AudioSource audioSource;
    // public List<AudioClip> clipList = new List<AudioClip>();

    public CloudDatas cloudDatas;
    

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

        text.text = "";
        
        // Set cloud dones to false for all events
        foreach (CloudEvent cloudEvent in Enum.GetValues(typeof(CloudEvent)))
        {
            cloudDones.Add(new CloudDone { cloudEvent = cloudEvent, done = false });
        }
    }


    
    void Update()
    {

        canva.enabled = audioSource.isPlaying;

        Vector3 shaderCenterMapPosition = updateTerrainRenderer.GetMapCenter();
        Vector3 centerMapPosition = new Vector3(shaderCenterMapPosition.x, meshTransform.position.y, shaderCenterMapPosition.z);
        transform.position = centerMapPosition + ovrPose.position + (movementOffset * 2.5f);


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

        if (new_val < 1 && new_val > 0)
        {
            intensity_color = new_val;
            character_mat.SetColor("_BaseColor", Color.Lerp(minValColor, maxValColor, intensity_color));
        }

    }


    // Display new text 
    public float DisplayNewText(CloudEvent cloudEvent)
    {
        
        CloudText cloudText = cloudDatas.cloudTexts.Find(x => x.cloudEvent == cloudEvent);
        
        TextAudio textAudio = cloudText.textsAudios[UnityEngine.Random.Range(0, cloudText.textsAudios.Count)];
        
        cloudDones.Find(x => x.cloudEvent == cloudEvent).done = true;
        
        if (true)
        {
            canva.enabled = true;
            
            text.text = textAudio.text;

            // Si un audio clip correspondant est trouvé, le jouer
            // et attendre la fin de la lecture avant de lire un autre texte
            if (textAudio.audioClip != null && !isDisplayingText)
            {
                audioSource.clip = textAudio.audioClip;
                audioSource.Play();
                isDisplayingText = true;
                float duration = textAudio.audioClip.length;
                StartCoroutine(WaitForAudio(duration));
                return duration;
            }
            else
            {
                Debug.LogWarning("Aucun audio clip correspondant trouvé pour " + cloudEvent.ToString());
            }

        }

        return 0;
    }
    public IEnumerator WaitForAudio(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDisplayingText = false;
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

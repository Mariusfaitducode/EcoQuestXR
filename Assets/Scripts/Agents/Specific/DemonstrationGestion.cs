using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonstrationGestion : MonoBehaviour
{
    
    internal GameManager gameManager;
    internal AgentManager agentManager;

    
    public List<GameObject> characterPrefabs = new List<GameObject>();
    // public AnimatorController animator;

    public float sizePlace = 3f;
    public float floorHeight = 1f;
    
    internal List<GameObject> characters = new List<GameObject>();
    
    internal float prefabScale = 1f;
    internal float uniformScale = 1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        

    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager != null)
        {
            // currentTime = gameManager.timer.currentTime;
            prefabScale = gameManager.objectManager.prefabScale;
            uniformScale = gameManager.objectManager.mesh.transform.localScale.x;
        }

        // if (Input.GetKey(KeyCode.Space))
        // {
        //     // Init demonstration place
        //     
        //     StartDemonstration(10);
        //
        // }
    }


    public void StartDemonstration(int quantity)
    {
        // int quantity = 10;


        for (int i = 0; i < quantity; i++)
        {
                
                
            Vector3 centerPlace = this.transform.position;
                
            centerPlace.y = centerPlace.y + floorHeight * uniformScale;
                
            float radius = sizePlace * uniformScale;
                
                
                
                
            GameObject character = characterPrefabs[Random.Range(0, characterPrefabs.Count)];
                
                
            GameObject newCharacter = FillMapUtils.InstantiateObjectWithScale(character, this.transform, centerPlace, this.transform.rotation, Vector3.one * uniformScale);
                
            Vector3 randomPos = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
                
            newCharacter.transform.Translate( randomPos, Space.Self);
                
            // newCharacter.transform.position = randomPos;

            Animator charAnim = newCharacter.GetComponent<Animator>();
                
            // charAnim.runtimeAnimatorController = animator;
                    
            charAnim.Play("Protest");
                
                
            newCharacter.GetComponent<WalkOnZone>().sizePlace = sizePlace;
            newCharacter.GetComponent<WalkOnZone>().place = this.gameObject;
                
            // Init character animation
                
            characters.Add(newCharacter);
        }
    }
}

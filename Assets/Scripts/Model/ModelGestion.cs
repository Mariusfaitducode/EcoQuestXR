using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct RendererMaterials
{
    public MeshRenderer renderer;
    public List<Material> materials;
}


public struct SubObject
{
    public GameObject gameObject;
    
}


public class ModelGestion : MonoBehaviour
{
    // TODO : set at initialization
    public ObjectScript objectScript;
    
    
    // Apparence
    
    public List<RendererMaterials> rendererSettings = new List<RendererMaterials>();
    public List<GameObject> subObjects = new List<GameObject>();
    // public List<Animation> animations = new List<Animation>();
    // public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    // public List<AudioClip> audioClips = new List<AudioClip>();



    // Start is called before the first frame update
    void Start()
    {
        foreach (RendererMaterials rendererSetting in rendererSettings)
        {
            
            MeshRenderer renderer = rendererSetting.renderer;
            Material[] newMaterials = new Material[renderer.sharedMaterials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = new Material(renderer.sharedMaterials[i]);
                
                foreach (Material material in rendererSetting.materials)
                {
                    
                    if (material.name == renderer.sharedMaterials[i].name)
                    {
                        Debug.Log("Material found : " + material.name);
                        newMaterials[i] = new Material(material);
                        newMaterials[i].color = Random.ColorHSV();
                        break;
                    }
                }
                
                // newMaterials[i].color = FillMapUtils.GetRandomColorVariation(newMaterials[i].color, Random.Range(0, 0.1f));
            }
            renderer.materials = newMaterials;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitialisationModel()
    {
        // Initialize with the objectScript properties
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct RendererMaterials
{
    public List<MeshRenderer> renderers;
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
        InitialisationMaterials();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitialisationMaterials()
    {
        foreach (RendererMaterials rendererSetting in rendererSettings)
        {
            List<Material> newMaterials = new List<Material>();
            
            foreach (Material material in rendererSetting.materials)
            {
                Material newMaterial = new Material(material);
                // newMaterial.color = Random.ColorHSV();
                newMaterial.color = FillMapUtils.GetRandomColorVariation(newMaterial.color, Random.Range(0, 0.1f));

                newMaterials.Add(newMaterial);
            }

            foreach (MeshRenderer renderer in rendererSetting.renderers)
            {
                Material[] finalMaterials = new Material[renderer.sharedMaterials.Length];

                for (int i = 0; i < finalMaterials.Length; i++)
                {
                    finalMaterials[i] = new Material(renderer.sharedMaterials[i]);
                
                    foreach (Material material in newMaterials)
                    {
                    
                        if (material.name == renderer.sharedMaterials[i].name)
                        {
                            // Debug.Log("Material found : " + material.name);
                            finalMaterials[i] = new Material(material);
                            break;
                        }
                    }
                
                    // newMaterials[i].color = FillMapUtils.GetRandomColorVariation(newMaterials[i].color, Random.Range(0, 0.1f));
                }
                renderer.materials = finalMaterials;

            }
        }
    }

    public void InitialisationModel()
    {
        // Initialize with the objectScript properties
    }
}

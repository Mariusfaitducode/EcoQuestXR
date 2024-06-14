using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{

    internal Timer timer; 


    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void open_settings()
    {
        timer.PauseTime(); 
        this.enabled = true;    
    }


    public void close_settings()
    {
        timer.ResumeTime();
        this.enabled = false;
    }


}

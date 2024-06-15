using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsController : MonoBehaviour
{

    internal GameManager gameManager; 


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponentInChildren<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void open_settings()
    {
        gameManager.timer.PauseTime(); 
        this.GetComponentInChildren<Canvas>().enabled = true;    
    }


    public void close_settings()
    {
        if (!gameManager.eventsGestion.GetEventByName("DraftEvent").isEventActive) gameManager.timer.ResumeTime();
        this.GetComponentInChildren<Canvas>().enabled = false;
    }

    public void exit_game()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }




}

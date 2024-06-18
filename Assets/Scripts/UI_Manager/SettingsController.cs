using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsController : MonoBehaviour
{

    internal GameManager gameManager;

    private GameObject panel_settings;
    private GameObject panel_gameOver;

    private bool is_gameOver = false; 


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponentInChildren<Canvas>().enabled = false;
        panel_settings = this.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        panel_gameOver = this.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;

        panel_gameOver.SetActive(false);

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
        if (!gameManager.eventsGestion.GetEventByName("DraftEvent").isEventActive && !is_gameOver) gameManager.timer.ResumeTime();
        this.GetComponentInChildren<Canvas>().enabled = false;
    }

    public void exit_game()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void end_game()
    {
        Debug.Log("end_game");
        is_gameOver = true; 
        panel_settings.SetActive(false);
        panel_gameOver.SetActive(true); 

        open_settings();
    }







}

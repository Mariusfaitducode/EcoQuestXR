using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Camera.main.transform.position + new Vector3(0, 0.3f, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void run_tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }


    public void run_game()
    {
        SceneManager.LoadScene("Main");
    }


    public void quit_game()
    {

        // Quitter l'application
        Application.Quit();

        // Si tu es dans l'�diteur Unity, il est utile d'arr�ter aussi l'�diteur
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif


    }



}

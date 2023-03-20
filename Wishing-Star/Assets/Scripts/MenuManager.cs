using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]GameObject settingsMenu;
    [SerializeField]GameObject mainMenu;

    public GameObject titleArt;
    public GameObject startButton;
    private float buttonTimer = 0;
    private float buttonCooldownTime = 0.8f;

    private Scene currentScene;
    public int activeGame = 1;
    private bool started = false;

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == "Menu" && !started)
        {
            if (startButton.activeSelf)
            {
                if (buttonTimer < buttonCooldownTime)
                {
                    buttonTimer += Time.deltaTime;
                    //Debug.LogError(deathTimer);
                }
                else
                {
                    buttonTimer = 0;
                    startButton.SetActive(false);
                }
            }

            else if (!startButton.activeSelf)
            {
                if (buttonTimer < buttonCooldownTime)
                {
                    buttonTimer += Time.deltaTime;
                    //Debug.LogError(deathTimer);
                }
                else
                {
                    buttonTimer = 0;
                    startButton.SetActive(true);
                }
            }
        }
        
    }

    public void StartGame()
    {
        //Hardware to software ???
        started = true;
        startButton.SetActive(false);
        titleArt.SetActive(false);
    }

    /*
    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    public void Back()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    */

}

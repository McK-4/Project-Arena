using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //[SerializeField]GameObject settingsMenu;
    //[SerializeField]GameObject mainMenu;

    //public GameObject titleArt;
    public GameObject startButton;
    private float buttonTimer = 0;
    private float buttonCooldownTime = 0.8f;

    private Scene currentScene;
    public int activeGame = 1;
    private bool started = false;

    public List<Sprite> playerArtList;
    private Image playerArt;
    private Vector2 playerSelectInput;
    public int currentArtNum;

    private void Start()
    {
        currentArtNum = 0;
        playerArt = GameObject.Find("Player Art").GetComponent<Image>();
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        //Blinking Start Button
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


        if (playerSelectInput.y > 0)
        {
            if(currentArtNum <= 0)
                currentArtNum++;

            playerArt.sprite = playerArtList[currentArtNum];
        }
        else if (playerSelectInput.y < 0)
        {
            if (currentArtNum >= playerArtList.Count - 1)
                currentArtNum--;

            playerArt.sprite = playerArtList[currentArtNum];
        }

    }

    public void StartGame()
    {
        //Hardware to software ???
        started = true;
        startButton.SetActive(false);

        //Change this when we add title art
        //titleArt.SetActive(false);
    }

    public void PlayerSelect(InputAction.CallbackContext context)
    {
        playerSelectInput = context.ReadValue<Vector2>();
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

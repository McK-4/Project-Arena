using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //[SerializeField]GameObject settingsMenu;
    [SerializeField]GameObject mainMenu;

    //public GameObject titleArt;
    public GameObject startButton;
    private float buttonTimer = 0;
    private float buttonCooldownTime = 0.8f;

    private Scene currentScene;
    public int activeGame = 1;
    private bool started = false;
    
    //Player Select Menu Variables
    [SerializeField] GameObject playerSelectMenu;
    public List<Sprite> playerArtList;
    public Image playerArt;
    public Vector2 playerSelectInput;
    public int currentArtNum;
    private GameObject joinText_1;
    private GameObject joinText_2;
    private GameObject joinText_3;
    private GameObject joinText_4;

    //Map Select Menu Variables
    private GameObject mapSelectMenu;

    private GameObject mapCheckMark;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private GameObject player1Check;
    private GameObject player2Check;
    private GameObject player3Check;
    private GameObject player4Check;

    void Start()
    {

        mainMenu = GameObject.Find("Main Menu");

        //Player Select Menu
        playerSelectMenu = GameObject.Find("Player Select Menu");

        currentArtNum = 0;
        playerArt = GameObject.Find("Player Art").GetComponent<Image>();

        joinText_1 = GameObject.Find("Join Text_1");
        joinText_2 = GameObject.Find("Join Text_2");
        joinText_3 = GameObject.Find("Join Text_3");
        joinText_4 = GameObject.Find("Join Text_4");

        //Map Select Menu
        mapSelectMenu = GameObject.Find("Map Select");

        mapCheckMark = GameObject.Find("Map Selected Check");

        player1 = GameObject.Find("1_Player Mark");
        player2 = GameObject.Find("2_Player Mark");
        player3 = GameObject.Find("3_Player Mark");
        player4 = GameObject.Find("4_Player Mark");

        player1Check = GameObject.Find("1_Player Selected");
        player2Check = GameObject.Find("1_Player Selected");
        player3Check = GameObject.Find("1_Player Selected");
        player4Check = GameObject.Find("1_Player Selected");

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

        //Cycling Player Select Art
        if(playerSelectMenu)
        {
            if (playerSelectInput.y > 0)
            {
                if (currentArtNum <= 0)
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

        //Map Select Menu
        if (mapSelectMenu)
        {
            //If more than one maps, use playerSelectInput to move the player's mark
            

        }

    }

    public void StartGame()
    {
        //Hardware to software ???
        started = true;
        startButton.SetActive(false);

        //Change this when we add title art
        //titleArt.SetActive(false);

        mainMenu.SetActive(false);

        playerSelectMenu.SetActive(true);
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

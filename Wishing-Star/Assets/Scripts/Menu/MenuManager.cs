using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //[SerializeField]GameObject settingsMenu;
    [SerializeField]GameObject mainMenu;

    //public GameObject titleArt;
    public GameObject startButton;
    private float buttonTimer = 0;
    private float buttonCooldownTime = 0.8f;

    public int activeGame = 1;
    private bool started;

    public Vector2 playerSelectInput;

    //Player Select Menu Variables
    [SerializeField] GameObject playerSelectMenu;
    public int playersReady;
    [SerializeField]PlayerMenu player1Select;
    [SerializeField]PlayerMenu player2Select;
    [SerializeField]PlayerMenu player3Select;
    [SerializeField]PlayerMenu player4Select;
    bool player1Active;
    bool player2Active;
    bool player3Active;
    bool player4Active;
    [SerializeField]TextMeshProUGUI selectTimer;
    float startTimer;
    float waitTime = 5f;

    //Map Select Menu Variables
    [SerializeField] GameObject mapSelectMenu;

    void Start()
    {
    }

    void Update()
    {
        //Blinking Start Button
        if(!started)
        {
            if (startButton.activeSelf)
            {
                if (buttonTimer < buttonCooldownTime)
                {
                    buttonTimer += Time.deltaTime;
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
                }
                else
                {
                    buttonTimer = 0;
                    startButton.SetActive(true);
                }
            }
        }

        if (playersReady >= 2)
        {
            if (startTimer > 0)
            {
                startTimer -= Time.deltaTime;
                selectTimer.text = startTimer.ToString("f0");
            }
            else if (startTimer <= 0)
            {
                player1Active = player1Select.skinSelected;
                player2Active = player2Select.skinSelected;
                player3Active = player3Select.skinSelected;
                player4Active = player4Select.skinSelected;
                
                playerSelectMenu.SetActive(false);

                player1Select.playerSelectMenu = false;
                player2Select.playerSelectMenu = false;
                player3Select.playerSelectMenu = false;
                player4Select.playerSelectMenu = false;

                mapSelectMenu.SetActive(true);
                player1Select.mapSelectMenu = true;
                player2Select.mapSelectMenu = true;
                player3Select.mapSelectMenu = true;
                player4Select.mapSelectMenu = true;
            }
        }
        else
        {
            selectTimer.text = "";
            startTimer = waitTime;
        }
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            started = true;
            mainMenu.SetActive(false);

            playerSelectMenu.SetActive(true);
            player1Select.playerSelectMenu = true;
            player2Select.playerSelectMenu = true;
            player3Select.playerSelectMenu = true;
            player4Select.playerSelectMenu = true;
        }
    }
}

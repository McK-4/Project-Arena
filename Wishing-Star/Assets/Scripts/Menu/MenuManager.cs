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
    private PlayerMenu player1Check;
    private PlayerMenu player2Check;
    private PlayerMenu player3Check;
    private PlayerMenu player4Check;
    bool player1Active;
    bool player2Active;
    bool player3Active;
    bool player4Active;
    [SerializeField]TextMeshProUGUI selectTimer;
    float startTimer;
    float waitTime = 5f;

    [SerializeField] GameObject mapSelectMenu;

    [SerializeField] GameObject mapCheckMark;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    

    void Start()
    {
        //player1 = GameObject.Find("1_Player Mark");
        //player2 = GameObject.Find("2_Player Mark");
        //player3 = GameObject.Find("3_Player Mark");
        //player4 = GameObject.Find("4_Player Mark");

        player1Check = GameObject.Find("Player_1 Player Select").GetComponent<PlayerMenu>();
        player2Check = GameObject.Find("Player_2 Player Select").GetComponent<PlayerMenu>();
        player3Check = GameObject.Find("Player_3 Player Select").GetComponent<PlayerMenu>();
        player4Check = GameObject.Find("Player_4 Player Select").GetComponent<PlayerMenu>();
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
                player1Active = player1Check.skinSelected;
                player2Active = player2Check.skinSelected;
                player3Active = player3Check.skinSelected;
                player4Active = player4Check.skinSelected;
                
                playerSelectMenu.SetActive(false);
                mapSelectMenu.SetActive(true);
            }
        }
        else
        {
            selectTimer.text = "";
            startTimer = waitTime;
        }
    }

    public void StartGame()
    {
        //Hardware to software ???
        started = true;
        startButton.SetActive(false);

        mainMenu.SetActive(false);

        playerSelectMenu.SetActive(true);
    }
}

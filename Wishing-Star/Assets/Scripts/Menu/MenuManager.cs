using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    MasterManager master;
    [SerializeField] Animator fade;

    //[SerializeField]GameObject settingsMenu;
    [SerializeField]GameObject mainMenu;

    //public GameObject titleArt;
    public GameObject startButton;
    private float buttonTimer = 0;
    private float buttonCooldownTime = 0.8f;
    public AudioSource ButtonPressed;

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
    InputDevice player1Input;
    InputDevice player2Input;
    InputDevice player3Input;
    InputDevice player4Input;
    [SerializeField]TextMeshProUGUI selectTimer;
    float startTimer;
    float waitTime = 5f;

    //Map Select Menu Variables
    [SerializeField] GameObject mapSelectMenu;
    public int playersVoted;
    //int mostVotes;
    //int selectedMap;
    //[SerializeField]int[] maps;
    //Dictionary<int, int> mapVote;

    void Start()
    {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();
    }

    void Update()
    {
        //foreach (int map in maps)
       //{
            //mapVote.Add(map, 0);
        //}

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

                if (!player1Active)
                {
                    player1Select.gameObject.SetActive(false);
                }
                else if(player1Active)
                {
                    player1Input = player1Select.inputDevice.device;
                }
                if (!player2Active)
                {
                    player2Select.gameObject.SetActive(false);
                }
                else if (player2Active)
                {
                    player2Input = player2Select.inputDevice.device;
                }
                if (!player3Active)
                {
                    player3Select.gameObject.SetActive(false);
                }
                else if (player3Active)
                {
                    player3Input = player3Select.inputDevice.device;
                }
                if (!player4Active)
                {
                    player4Select.gameObject.SetActive(false);
                }
                else if (player4Active)
                {
                    player4Input = player4Select.inputDevice.device;
                }
                master.player1Input = player1Input;
                master.player2Input = player2Input;
                master.player3Input = player3Input;
                master.player4Input = player4Input;
                master.player1Active = player1Active;
                master.player2Active = player2Active;
                master.player3Active = player3Active;
                master.player4Active = player4Active;
                master.player1Skin = player1Select.currentArtNum;
                master.player2Skin = player2Select.currentArtNum;
                master.player3Skin = player3Select.currentArtNum;
                master.player4Skin = player4Select.currentArtNum;

                StartCoroutine("start");

                //mapSelectMenu.SetActive(true);
                //player1Select.mapSelectMenu = true;
                //player2Select.mapSelectMenu = true;
                //player3Select.mapSelectMenu = true;
                //player4Select.mapSelectMenu = true;
                //if (!player1Active)
                //{
                //    player1Select.mark.SetActive(false);
                //}
                //if (!player2Active)
                //{
                //    player2Select.mark.SetActive(false);
                //}
                //if (!player3Active)
                //{
                //    player3Select.mark.SetActive(false);
                //}
                //if (!player4Active)
                //{
                //    player4Select.mark.SetActive(false);
                //}
            }
        }
        else
        {
            selectTimer.text = "";
            startTimer = waitTime;
        }

        if (playersVoted > 0 && playersVoted == playersReady)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            started = true;
            mainMenu.SetActive(false);
            ButtonPressed.Play();

            playerSelectMenu.SetActive(true);
            player1Select.playerSelectMenu = true;
            player2Select.playerSelectMenu = true;
            player3Select.playerSelectMenu = true;
            player4Select.playerSelectMenu = true;
        }
    }
    IEnumerator start()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(Random.Range(1,2));
    }

    //public void MapVote(int votedMap, int change)
    //{
       //mapVote[votedMap] += change;
    //}

    //void LoadMap()
    //{
        //foreach (var map in maps)
        //{
            //if (mapVote[map] >= mostVotes)
            //{
                //selectedMap = map;
            //}
        //}

        //SceneManager.LoadScene(selectedMap + 1);
    //}
}

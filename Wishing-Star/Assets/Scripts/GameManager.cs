using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    Scene currentScene;

    [SerializeField]PlayerController player_1;
    [SerializeField]PlayerController player_2;
    [SerializeField]PlayerController player_3;
    [SerializeField]PlayerController player_4;

    public GameObject killer;
    PlayerController killerController;
    private bool awarded = false;

    public int totalGameTime = 300;
    public float gameTimeRemaining;
    public int minutesLeft;
    public int secondsLeft;
    [SerializeField]TextMeshProUGUI timeText;

    // Win Screen (UI Stuff)
    private PlayerManager playerManager;
    [SerializeField] List<GameObject> playerUI;
    [SerializeField] GameObject gameWinUI;
    public SpriteRenderer winPlayerArtMat;
    [SerializeField] Image winPlayerArt;
    [SerializeField] TextMeshProUGUI winPlayerNum;
    [SerializeField] GameObject contButton;
    private float buttonTimer = 0;
    private float buttonCooldownTime = 0.8f;
    public bool readyForScene = false;

    // Start is called before the first frame update
    void Start()
    {
        gameTimeRemaining = totalGameTime;

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        gameWinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if (player_1.died)
        {
            AwardPlayerKills(player_1.killerName);
        }
        if (player_2.died)
        {
            AwardPlayerKills(player_2.killerName);
        }
        if (player_3.died)
        {
            AwardPlayerKills(player_3.killerName);
        }
        if (player_4.died)
        {
            AwardPlayerKills(player_4.killerName);
        }

        //Change "Alex Test" to the name of the scene we are using for the game
        if (currentScene.buildIndex >= 1)
        {
            if (gameTimeRemaining > 0)
            {
                gameTimeRemaining -= Time.deltaTime;

                minutesLeft = Mathf.FloorToInt(gameTimeRemaining / 60);
                secondsLeft = Mathf.FloorToInt(gameTimeRemaining % 60);
            }

            if (minutesLeft >= 0)
            {
                timeText.text = minutesLeft.ToString() + ":" + secondsLeft.ToString();
            }
            

            if (gameTimeRemaining <= 0)
            {
                GameEnd();
            }
        }

    }

    public void AwardPlayerKills(string killer)
    {
        if (!awarded)
        {
            killerController = GameObject.Find(killer).GetComponent<PlayerController>();
            killerController.points++;
            awarded = true;
            //Debug.Log(killer.name + " got awarded a point for their kill!");
        }
    }

    void GameEnd()
    {
        readyForScene = true;
        //Turning Off Active Game UI and Turning On Game Win UI
        foreach (GameObject ui in playerUI)
        {
            ui.SetActive(false);
        }

        gameWinUI.SetActive(true);

        //Getting Winning Player Art
        //winPlayerArtMat = playerManager.winningPlayer.GetComponent<SpriteRenderer>();

        playerManager.GameWin();

        winPlayerArt.sprite = winPlayerArtMat.sprite;
        winPlayerArt.material = winPlayerArtMat.material;
        //Debug.Log(playerManager.winningPlayer.name);
        winPlayerNum.text = playerManager.winningPlayer.name.Substring(1,0);

        //Blinking Continue Button
        if (contButton.activeSelf)
        {
            if (buttonTimer < buttonCooldownTime)
            {
                buttonTimer += Time.deltaTime;
            }
            else
            {
                buttonTimer = 0;
                contButton.SetActive(false);
            }
        }

        else if (!contButton.activeSelf)
        {
            if (buttonTimer < buttonCooldownTime)
            {
                buttonTimer += Time.deltaTime;
            }
            else
            {
                buttonTimer = 0;
                contButton.SetActive(true);
            }
        }
    }
}

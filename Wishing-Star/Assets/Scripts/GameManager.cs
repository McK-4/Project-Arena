using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    // Start is called before the first frame update
    void Start()
    {
        gameTimeRemaining = totalGameTime;
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
        //Replace this with Win Screen
        SceneManager.LoadScene(0);
    }
}

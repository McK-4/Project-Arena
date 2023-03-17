using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Scene currentScene;

    PlayerController player_1_Controller;
    PlayerController player_2_Controller;
    PlayerController player_3_Controller;
    PlayerController player_4_Controller;

    public GameObject killer;
    PlayerController killerController;
    private bool awarded = false;

    public int totalGameTime = 300;
    public float gameTimeRemaining;
    public int minutesLeft;
    public int secondsLeft;

    // Start is called before the first frame update
    void Start()
    {
        player_1_Controller = GameObject.Find("1_Player").GetComponentInChildren<PlayerController>();
        player_2_Controller = GameObject.Find("2_Player").GetComponentInChildren<PlayerController>();
        player_3_Controller = GameObject.Find("3_Player").GetComponentInChildren<PlayerController>();
        player_4_Controller = GameObject.Find("4_Player").GetComponentInChildren<PlayerController>();

        gameTimeRemaining = totalGameTime;

    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if (player_1_Controller.died)
        {
            AwardPlayerKills();
        }
        else if (player_2_Controller.died)
        {
            AwardPlayerKills();
        }
        else if (player_3_Controller.died)
        {
            AwardPlayerKills();
        }
        else if (player_4_Controller.died)
        {
            AwardPlayerKills();
        }

        //Change "Alex Test" to the name of the scene we are using for the game
        if (currentScene.name == "Alex Test")
        {
            if (gameTimeRemaining > 0)
            {
                gameTimeRemaining -= Time.deltaTime;

                minutesLeft = Mathf.FloorToInt(gameTimeRemaining / 60);
                secondsLeft = Mathf.FloorToInt(gameTimeRemaining % 60);
            }

            if (gameTimeRemaining <= 0)
            {
                GameEnd();
            }
        }

    }

    public void AwardPlayerKills()
    {
        killer = GameObject.Find(player_1_Controller.killerName[0] + "_Player");
        killerController = killer.GetComponent<PlayerController>();

        if (!awarded)
        {
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

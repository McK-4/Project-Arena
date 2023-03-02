using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            for(int i = 0; i < players.Length; i++)
            {
                player.name = "Player_"+i;
            }
        }
    }


}

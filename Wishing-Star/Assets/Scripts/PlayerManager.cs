using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject[] players;

    public GameObject player_1;
    public GameObject player_2;
    public GameObject player_3;
    public GameObject player_4;

    //Anti-Player Camping Variable
    public Vector2 playerSpawn_1;
    public Vector2 playerSpawn_2;
    public Vector2 playerSpawn_3;
    public Vector2 playerSpawn_4;

    public LayerMask playerLayers;
    public float antiCampRange = 1;
    public bool playerCamping = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawn_1 = GameObject.Find("PlayerParent").transform.position;
        playerSpawn_2 = GameObject.Find("PlayerParent_1").transform.position;
        playerSpawn_3 = GameObject.Find("PlayerParent_2").transform.position;
        playerSpawn_4 = GameObject.Find("PlayerParent_3").transform.position;

        player_1 = GameObject.Find("Player_1");
        player_2 = GameObject.Find("Player_2");
        player_3 = GameObject.Find("Player_3");
        player_4 = GameObject.Find("Player_4");
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        /*
        foreach(GameObject player in players)
        {
            for(int i = 0; i < players.Length; i++)
            {
                player.name = "Player_"+i;
            }
        }
        */

        //Anti-Player Camping
        
        //Player1
        Collider2D[] hitPlayers_1 = Physics2D.OverlapCircleAll(playerSpawn_1, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_1)
        {
            //Physics2D.IgnoreCollision(player_1.GetComponent<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
            if (player.name == "Player_2" || player.name == "Player_3" || player.name == "Player_4")
            {
                playerCamping = true;
            }
        }

        //Player 2
        Collider2D[] hitPlayers_2 = Physics2D.OverlapCircleAll(playerSpawn_2, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_2)
        {
            Physics2D.IgnoreCollision(player_2.GetComponent<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
            if (player.name == "Player_1" || player.name == "Player_3" || player.name == "Player_4")
            {
                playerCamping = true;
            }
        }
        
        //Player 3
        Collider2D[] hitPlayers_3 = Physics2D.OverlapCircleAll(playerSpawn_3, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_3)
        {
            Physics2D.IgnoreCollision(player_3.GetComponent<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
            if (player.name == "Player_1" || player.name == "Player_2" || player.name == "Player_3")
            {
                playerCamping = true;
            }
        }
        
        //Player 4
        Collider2D[] hitPlayers_4 = Physics2D.OverlapCircleAll(playerSpawn_4, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_4)
        {
            Physics2D.IgnoreCollision(player_4.GetComponent<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
            if (player.name == "Player_1" || player.name == "Player_2" || player.name == "Player_3")
            {
                playerCamping = true;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (playerSpawn_1 == null)
            return;

        Gizmos.DrawWireSphere(playerSpawn_1, antiCampRange);
        Gizmos.DrawWireSphere(playerSpawn_2, antiCampRange);
        Gizmos.DrawWireSphere(playerSpawn_3, antiCampRange);
        Gizmos.DrawWireSphere(playerSpawn_4, antiCampRange);
    }


}

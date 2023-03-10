using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject[] players;

    private GameObject player_1;
    private GameObject player_2;
    private GameObject player_3;
    private GameObject player_4;

    //Anti-Player Camping Variable
    private Vector2 playerSpawn_1;
    private Vector2 playerSpawn_2;
    private Vector2 playerSpawn_3;
    private Vector2 playerSpawn_4;

    public LayerMask playerLayers;
    public float antiCampRange = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawn_1 = GameObject.Find("PlayerParent").transform.position;
        playerSpawn_2 = GameObject.Find("PlayerParent_1").transform.position;
        playerSpawn_3 = GameObject.Find("PlayerParent_2").transform.position;
        playerSpawn_4 = GameObject.Find("PlayerParent_3").transform.position;

        player_1 = GameObject.Find("PlayerParent");
        player_2 = GameObject.Find("PlayerParent_1");
        player_3 = GameObject.Find("PlayerParent_2");
        player_4 = GameObject.Find("PlayerParent_3");
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
            Physics2D.IgnoreCollision(player_1.GetComponentInChildren<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
        }

        //Player 2
        Collider2D[] hitPlayers_2 = Physics2D.OverlapCircleAll(playerSpawn_2, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_2)
        {
            Physics2D.IgnoreCollision(player_2.GetComponentInChildren<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
        }
        
        //Player 3
        Collider2D[] hitPlayers_3 = Physics2D.OverlapCircleAll(playerSpawn_3, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_3)
        {
            Physics2D.IgnoreCollision(player_3.GetComponentInChildren<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
        }
        
        //Player 4
        Collider2D[] hitPlayers_4 = Physics2D.OverlapCircleAll(playerSpawn_4, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_4)
        {
            Physics2D.IgnoreCollision(player_4.GetComponentInChildren<BoxCollider2D>(), player);
            Debug.Log("Hit " + player.name);
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

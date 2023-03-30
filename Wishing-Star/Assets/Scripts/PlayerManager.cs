using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    MasterManager master;

    //Players
    public GameObject player_1;
    public GameObject player_2;
    public GameObject player_3;
    public GameObject player_4;

    PlayerController player1;
    PlayerController player2;
    PlayerController player3;
    PlayerController player4;

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
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();

        player_1 = GameObject.Find("1_Player");
        player_2 = GameObject.Find("2_Player");
        player_3 = GameObject.Find("3_Player");
        player_4 = GameObject.Find("4_Player");

        player1 = player_1.GetComponent<PlayerController>();
        player2 = player_2.GetComponent<PlayerController>();
        player3 = player_3.GetComponent<PlayerController>();
        player4 = player_4.GetComponent<PlayerController>();

        playerSpawn_1 = player_1.transform.position;
        playerSpawn_2 = player_2.transform.position;
        playerSpawn_3 = player_3.transform.position;
        playerSpawn_4 = player_4.transform.position;

        player1.activeSkin = master.player1Skin;
        player2.activeSkin = master.player2Skin;
        player3.activeSkin = master.player3Skin;
        player4.activeSkin = master.player4Skin;

        if (!master.player1Active)
        {
            Destroy(player_1);
        }
        if (!master.player2Active)
        {
            Destroy(player_2);
        }
        if (!master.player3Active)
        {
            Destroy(player_3);
        }
        if (!master.player4Active)
        {
            Destroy(player_4);
        }

        if (master.player1Input != null)
        {
            player1.inputDevice = master.player1Input;
            player1.RePair();
        }
        if(master.player2Input != null)
        {
            player2.inputDevice = master.player1Input;
            player2.RePair();
        }
        if(master.player3Input != null)
        {
            player3.inputDevice = master.player3Input;
            player3.RePair();
        }
        if(master.player4Input != null)
        {
            player4.inputDevice = master.player4Input;
            player4.RePair();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Anti-Player Camping
        
        //Player1
        Collider2D[] hitPlayers_1 = Physics2D.OverlapCircleAll(playerSpawn_1, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_1)
        {
            Physics2D.IgnoreCollision(player_1.GetComponent<BoxCollider2D>(), player);

            //Debug.Log("Hit " + player.name);

            if (player.name == "2_Player" || player.name == "3_Player" || player.name == "4_Player")
            {
                playerCamping = true;
            }
        }

        //Player 2
        Collider2D[] hitPlayers_2 = Physics2D.OverlapCircleAll(playerSpawn_2, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_2)
        {
            Physics2D.IgnoreCollision(player_2.GetComponent<BoxCollider2D>(), player);

            //Debug.Log("Hit " + player.name);

            if (player.name == "1_Player" || player.name == "3_Player" || player.name == "4_Player")
            {
                playerCamping = true;
            }
        }
        
        //Player 3
        Collider2D[] hitPlayers_3 = Physics2D.OverlapCircleAll(playerSpawn_3, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_3)
        {
            Physics2D.IgnoreCollision(player_3.GetComponent<BoxCollider2D>(), player);

            //Debug.Log("Hit " + player.name);

            if (player.name == "1_Player" || player.name == "2_Player" || player.name == "4_Player")
            {
                playerCamping = true;
            }
        }
        
        //Player 4
        Collider2D[] hitPlayers_4 = Physics2D.OverlapCircleAll(playerSpawn_4, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_4)
        {
            Physics2D.IgnoreCollision(player_4.GetComponent<BoxCollider2D>(), player);

            //Debug.Log("Hit " + player.name);

            if (player.name == "1_Player" || player.name == "2_Player" || player.name == "3_Player")
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

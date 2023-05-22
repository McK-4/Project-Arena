using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Vector2[] playerSpawns = new Vector2[4];
    public int[] playerOrders = new int[4];

    public LayerMask playerLayers;
    public float antiCampRange = 1;
    public bool playerCamping = false;

    //Points
    public bool p1PointsRewarded = false;
    public bool p2PointsRewarded = false;
    public bool p3PointsRewarded = false;
    public bool p4PointsRewarded = false;

    private Vector2 winnerPos;
    private Vector2 secondPos;
    private Vector2 thirdPos;
    private Vector2 fourthPos;

    public GameObject winningPlayer;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();
        }
        catch
        {
            master = null;
        }

        player1 = player_1.GetComponent<PlayerController>();
        player2 = player_2.GetComponent<PlayerController>();
        player3 = player_3.GetComponent<PlayerController>();
        player4 = player_4.GetComponent<PlayerController>();

        playerSpawns[0] = player_1.transform.position;
        playerOrders[0] = player1.orderInLayer;
        playerSpawns[1] = player_2.transform.position;
        playerOrders[1] = player2.orderInLayer;
        playerSpawns[2] = player_3.transform.position;
        playerOrders[2] = player3.orderInLayer;
        playerSpawns[3] = player_4.transform.position;
        playerOrders[3] = player4.orderInLayer;
        /*
        Debug.Log("P1: " + playerOrders[0]);
        Debug.Log("P2: " + playerOrders[1]);
        Debug.Log("P3: " + playerOrders[2]);
        Debug.Log("P4: " + playerOrders[3]);
        */
        if (master != null)
        {
            player1.activeSkin = master.player1Skin;
            player2.activeSkin = master.player2Skin;
            player3.activeSkin = master.player3Skin;
            player4.activeSkin = master.player4Skin;

            if (!master.player1Active)
            {
                player_1.SetActive(false);
            }
            if (!master.player2Active)
            {
                player_2.SetActive(false);
            }
            if (!master.player3Active)
            {
                player_3.SetActive(false);
            }
            if (!master.player4Active)
            {
                player_4.SetActive(false);
            }

            if (master.player1Input != null)
            {
                player1.inputDevice = master.player1Input;
                player1.RePair();
            }
            if (master.player2Input != null)
            {
                player2.inputDevice = master.player2Input;
                player2.RePair();
            }
            if (master.player3Input != null)
            {
                player3.inputDevice = master.player3Input;
                player3.RePair();
            }
            if (master.player4Input != null)
            {
                player4.inputDevice = master.player4Input;
                player4.RePair();
            }
        }

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Player Leaderboard sorting:
        Player[] playerPoints = { new Player(player1.gameObject,player1.points), new Player(player2.gameObject, player2.points), new Player(player3.gameObject, player3.points), new Player(player4.gameObject, player4.points), };
        playerPoints = playerPoints.OrderByDescending(p => p.score).ToArray();

        //Setting the Player's win screen position
        //This needs to be changed to changing a UI image position, not the actual gameobject
        winningPlayer = playerPoints[0].playerGameObject;

        //Debug.Log(winningPlayer.name);
        /*
        playerPoints[0].playerGameObject.transform.position = winnerPos;
        playerPoints[1].playerGameObject.transform.position = secondPos;
        playerPoints[2].playerGameObject.transform.position = thirdPos;
        playerPoints[3].playerGameObject.transform.position = fourthPos;
        */
    }

    //Cast and check in sequential order for available spawns and return value of available spawn
    public void Respawn(PlayerController player)
    {
        if (Physics2D.OverlapCircle(playerSpawns[0], antiCampRange, playerLayers) == null)
        {
            player.Spawn(1);
        }
        else if (Physics2D.OverlapCircle(playerSpawns[1], antiCampRange, playerLayers) == null)
        {
            player.Spawn(2);
        }
        else if (Physics2D.OverlapCircle(playerSpawns[2], antiCampRange, playerLayers) == null)
        {
            player.Spawn(3);
        }
        else if (Physics2D.OverlapCircle(playerSpawns[3], antiCampRange, playerLayers) == null)
        {
            player.Spawn(4);
        }
    }

    public void GameWin()
    {
        gameManager.winPlayerArtMat = winningPlayer.GetComponent<SpriteRenderer>();
    }
    class Player
    {
        public GameObject playerGameObject;
        public int score;
        public Player(GameObject playerGameObject, int score)
        {
            this.playerGameObject = playerGameObject;
            this.score = score;
        }
    }
    //Anti-camping suff
    private void OnDrawGizmosSelected()
    {
        if (playerSpawns == null)
            return;

        Gizmos.DrawWireSphere(playerSpawns[0], antiCampRange);
        Gizmos.DrawWireSphere(playerSpawns[1], antiCampRange);
        Gizmos.DrawWireSphere(playerSpawns[2], antiCampRange);
        Gizmos.DrawWireSphere(playerSpawns[3], antiCampRange);
    }
}

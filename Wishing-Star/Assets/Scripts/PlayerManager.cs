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
    public Vector2 playerSpawn_1;
    public Vector2 playerSpawn_2;
    public Vector2 playerSpawn_3;
    public Vector2 playerSpawn_4;

    private float campTimer = 0;
    private float campCooldownTimer = 0.2f;

    public int[] playerOrders;

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
        catch { }

        player1 = player_1.GetComponent<PlayerController>();
        player2 = player_2.GetComponent<PlayerController>();
        player3 = player_3.GetComponent<PlayerController>();
        player4 = player_4.GetComponent<PlayerController>();

        playerSpawn_1 = player_1.transform.position;
        playerSpawn_2 = player_2.transform.position;
        playerSpawn_3 = player_3.transform.position;
        playerSpawn_4 = player_4.transform.position;
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
        //Anti-Player Camping

        //Player camping refresh timer
        if (playerCamping)
        {
            if (campTimer < campCooldownTimer)
            {
                campTimer += Time.deltaTime;
            }
            else if (campTimer >= campCooldownTimer)
            {
                campTimer = 0;
                playerCamping = false;
            }
        }

        //Player1
        Collider2D[] hitPlayers_1 = Physics2D.OverlapCircleAll(playerSpawn_1, antiCampRange, playerLayers);

        foreach (Collider2D player in hitPlayers_1)
        {
            Physics2D.IgnoreCollision(player_1.GetComponent<BoxCollider2D>(), player);

            //Debug.Log("Hit " + player.name);

            if (player.name == "2_Player" || player.name == "3_Player" || player.name == "4_Player")
            {
                playerCamping = true;
                /*
                if (player1.died)
                {
                    if (player1.ranNum == 1)
                    {
                        player1.orderInLayer = playerOrders[1];
                        player1.Order(playerOrders[1]);
                    }
                    else if (player1.ranNum == 2)
                    {
                        player1.orderInLayer = playerOrders[2];
                        player1.Order(playerOrders[2]);
                    }
                    else if (player1.ranNum == 3)
                    {
                        player1.orderInLayer = playerOrders[3];
                        player1.Order(playerOrders[3]);
                    }
                }
                */
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
                /*
                if (player2.died)
                {
                    if (player2.ranNum == 1)
                    {
                        player2.orderInLayer = playerOrders[0];
                        player2.Order(playerOrders[0]);
                    }
                    else if (player2.ranNum == 2)
                    {
                        player2.orderInLayer = playerOrders[2];
                        player2.Order(playerOrders[2]);
                    }
                    else if (player2.ranNum == 3)
                    {
                        player2.orderInLayer = playerOrders[3];
                        player2.Order(playerOrders[3]);
                    }
                }
                */
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
                /*
                if (player3.died)
                {
                    if (player3.ranNum == 1)
                    {
                        player3.orderInLayer = playerOrders[1];
                        player3.Order(playerOrders[1]);
                    }
                    else if (player3.ranNum == 2)
                    {
                        player3.orderInLayer = playerOrders[0];
                        player3.Order(playerOrders[0]);
                    }
                    else if (player3.ranNum == 3)
                    {
                        player3.orderInLayer = playerOrders[3];
                        player3.Order(playerOrders[3]);
                    }
                }
                */
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
                /*
                if (player4.died)
                {
                    if (player4.ranNum == 1)
                    {
                        player4.orderInLayer = playerOrders[1];
                        player4.Order(playerOrders[1]);
                    }
                    else if (player4.ranNum == 2)
                    {
                        player4.orderInLayer = playerOrders[2];
                        player4.Order(playerOrders[2]);
                    }
                    else if (player4.ranNum == 3)
                    {
                        player4.orderInLayer = playerOrders[0];
                        player4.Order(playerOrders[0]);
                    }
                }
                */
            }
        }

        //Making sure players don't respawn at the same spot
        if (player1.died)
        {
            if (player1.respawn == player2.respawn || player1.respawn == player3.respawn || player1.respawn == player4.respawn)
            {
                player1.playerRespawnShuffle();
            }
        }
        if (player2.died)
        {
            if (player2.respawn == player1.respawn || player2.respawn == player3.respawn || player2.respawn == player4.respawn)
            {
                player2.playerRespawnShuffle();
            }
        }
        if (player3.died)
        {
            if (player3.respawn == player2.respawn || player3.respawn == player1.respawn || player3.respawn == player4.respawn)
            {
                player3.playerRespawnShuffle();
            }
        }
        if (player4.died)
        {
            if (player4.respawn == player2.respawn || player4.respawn == player3.respawn || player4.respawn == player1.respawn)
            {
                player4.playerRespawnShuffle();
            }
        }

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
        if (playerSpawn_1 == null)
            return;

        Gizmos.DrawWireSphere(playerSpawn_1, antiCampRange);
        Gizmos.DrawWireSphere(playerSpawn_2, antiCampRange);
        Gizmos.DrawWireSphere(playerSpawn_3, antiCampRange);
        Gizmos.DrawWireSphere(playerSpawn_4, antiCampRange);
    }
    /*
    private void PlayerPointsTimer()
    {
        if(p1PointsRewarded)
        {
            if (pointsTimer < pointsCooldownTimer)
            {
                pointsTimer += Time.deltaTime;
            }
            else if (pointsTimer >= pointsCooldownTimer)
            {
                pointsTimer = 0;
                p1PointsRewarded = false;
            }
        }

        if(p2PointsRewarded)
        {
            if (pointsTimer < pointsCooldownTimer)
            {
                pointsTimer += Time.deltaTime;
            }
            else if (pointsTimer >= pointsCooldownTimer)
            {
                pointsTimer = 0;
                p2PointsRewarded = false;
            }
        }

        if(p3PointsRewarded)
        {
            if (pointsTimer < pointsCooldownTimer)
            {
                pointsTimer += Time.deltaTime;
            }
            else if (pointsTimer >= pointsCooldownTimer)
            {
                pointsTimer = 0;
                p3PointsRewarded = false;
            }
        }

        if(p4PointsRewarded)
        {
            if (pointsTimer < pointsCooldownTimer)
            {
                pointsTimer += Time.deltaTime;
            }
            else if (pointsTimer >= pointsCooldownTimer)
            {
                pointsTimer = 0;
                p4PointsRewarded = false;
            }
        }
    }
    */

}

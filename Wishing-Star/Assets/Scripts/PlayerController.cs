using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public int player = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRen;
    Animator anim;

    private PlayerManager playerManager;

    public GameObject[] players;
    public GameObject[] numPlayers;

    //Rewarding players with points
    public string killerName;

    public int points;

    public enum Directions
    {
        Up,
        Down,
        Right,
        Left
    }
    public Directions characterFacing = Directions.Right;
    private int Players;

    //Health
    Vector2 respawn;
    public int maxHealth = 10;
    public int health;
    private bool damaged;
    public float healthTimer = 0;
    public float healthCooldownTime = 0.5f;
    private int damageTaken = 0;
    //Death
    private int ranNum;
    public float deathTimer = 0;
    private float deathCooldownTime = 5;
    private Vector2 diedPos = new Vector2(123, 456);
    public bool died = false;

    //Movement
    public float movementSpeedMax = 7.5f;
    public float movementSpeed = 0;
    Vector2 tempVel;
    Vector2 moveinput = Vector2.zero;

    //Attacking
    Vector2 movingTo;
    public float angle;
    public float rotationSpeed;
    bool shieldUp = false;

    Vector2 scale = new Vector2(1,1);
    Vector2 iScale = new Vector2(-1, 1);

    [SerializeField]GameObject attackPoint;
    [SerializeField]GameObject shieldPoint;
    public float attackRange = 0.5f;

    public LayerMask playerLayers;
    public int basicSwordDamage = 2;

    //Mana 
    public int manaMax = 10;
    public int mana;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        //shieldCollider = shieldPoint.GetComponent<CircleCollider2D>();

        health = maxHealth;

        respawn = transform.position;

        mana = 10;

        points = 0;

        //movementSpeed = 0.1f;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;

    }

    void Update()
    {

        //numPlayers = GameObject.FindGameObjectWithTag

        //Movement
        tempVel = rb.velocity;

        tempVel.x = moveinput.x * movementSpeed;
        tempVel.y = moveinput.y * movementSpeed;

        rb.velocity = tempVel;
        anim.SetFloat("Velocity", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y));

        //Anti-Player Spawn Camping
        if(playerManager.playerCamping == true)
        {
            playerRespawnShuffle();
        }

        //Rotating the ShieldPoint and AttackPoint
        if (moveinput.y > 0)
        {
            characterFacing = Directions.Up;
            anim.SetInteger("Direction", 1);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (moveinput.y < 0)
        {
            characterFacing = Directions.Down;
            anim.SetInteger("Direction", 0);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if(moveinput.x > 0)
        {
            characterFacing = Directions.Right;
            anim.SetInteger("Direction", 2);
            attackPoint.transform.localScale = iScale;
            shieldPoint.transform.localScale = iScale;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if(moveinput.x < 0)
        {
            characterFacing = Directions.Left;
            anim.SetInteger("Direction", 2);
            attackPoint.transform.localScale = scale;
            shieldPoint.transform.localScale = scale;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        if (shieldUp)
        {
            //attackPoint.SetActive(false);
            //shieldPoint.SetActive(true);
            //swordNshield.isTrigger = false;
            //Debug.Log("Shield is UP");
        }
        else
        {
            //shieldPoint.SetActive(false);
            //Debug.Log("Shield is Down");
        }



        //Respawning
        if (died)
        {
            

            if (deathTimer < deathCooldownTime)
            {
                deathTimer += Time.deltaTime;
                //Debug.LogError(deathTimer);
            }
            else
            {
                deathTimer = 0;
                died = false;
                transform.SetPositionAndRotation(respawn, Quaternion.identity);
                health = maxHealth;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveinput = context.ReadValue<Vector2>();

        /*
        if(context.performed && movementSpeed < movementSpeedMax)
        {
        }
        */
        //Debug.Log(moveinput);
    }

    public void Sword(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void Shield(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetBool("Shield", true);
            movementSpeed = 0;
        }
        if (context.canceled)
        {
            anim.SetBool("Shield", false);
            movementSpeed = 7.5f;
        }
    }

    public void ShieldBlocked(int damage)
    {
        damageTaken += damage;
        damageTaken -= 1;

        Damaged(damageTaken);
    }

    public void Damaged(int damage)
    {
        health -= damage;
        damaged = true;
        Debug.Log(name + " was hit by " + killerName);

        //Brief invencibility after getting hit with a attack
        if (damaged)
        {
            if (healthTimer < healthCooldownTime)
            {
                healthTimer += Time.deltaTime;
            }

            if (healthTimer >= healthCooldownTime)
            {
                healthTimer = 0;
                damaged = false;
            }
        }

        //Reaspawn
        if (health <= 0 && !died)
        {
            Debug.Log("Player " + name[0] + " was killed by Player " + killerName[0]);
            died = true;
            transform.SetPositionAndRotation(diedPos, Quaternion.identity);
            //Debug.LogWarning("TELEPORTED!!!");
        }
        
    }
    
    private void playerRespawnShuffle()
    {
        RandomNum();

        if (name == "Player_1")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_2.x, playerManager.playerSpawn_2.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_3.x, playerManager.playerSpawn_3.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_4.x, playerManager.playerSpawn_4.y);
        }
        
        if (name == "Player_2")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_1.x, playerManager.playerSpawn_1.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_3.x, playerManager.playerSpawn_3.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_4.x, playerManager.playerSpawn_4.y);
        }
        
        if (name == "Player_3")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_2.x, playerManager.playerSpawn_2.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_1.x, playerManager.playerSpawn_1.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_4.x, playerManager.playerSpawn_4.y);
        }
        
        if (name == "Player_4")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_2.x, playerManager.playerSpawn_2.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_3.x, playerManager.playerSpawn_3.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_1.x, playerManager.playerSpawn_1.y);
        }
    }


    void RandomNum()
    {
        ranNum = Random.Range(1,3);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            killerName = collision.gameObject.transform.parent.name;
            Damaged(basicSwordDamage);
        }

        if (collision.gameObject.tag == "Sword" && shieldUp )
        {
            var nuller = (collision.transform.position - transform.position).normalized;
            var shield = (shieldPoint.transform.position - transform.position).normalized;
            //var angleallowed = 10;

            ShieldBlocked(basicSwordDamage);
        }

    }
}

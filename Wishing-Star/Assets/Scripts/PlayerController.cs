using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //public int player = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRen;
    Animator anim;
    [SerializeField]AnimatorOverrideController[] skin;
    public int activeSkin;
    PlayerInput input;
    public InputDevice inputDevice;
    public int user;

    //Map Layering
    public TilemapRenderer tileRen;
    public int orderInLayer;
    public string layerName;
    private bool movingFloors = false;
    [SerializeField] Collider2D mapCollider;

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
    public bool damaged;
    public float healthTimer = 0;
    public float healthCooldownTime = 0.5f;
    //private int damageTaken = 0;
    //Death
    private int ranNum;
    public float deathTimer = 0;
    private float deathCooldownTime = 5;
    private Vector2 diedPos = new Vector2(123, 456);
    public bool died = false;

    //Movement
    public float movementSpeedMax = 7.5f;
    public float movementSpeed = 3.75f;
    private float movementSpeedStart = 0f;
    private float acceleration = 0.1f;
    Vector2 tempVel;
    Vector2 moveinput = Vector2.zero;
    public bool moving = false;
    private bool moveAble = true;

    //Attacking
    Vector2 movingTo;
    public float angle;
    public float rotationSpeed;
    bool shieldUp = false;
    private bool attacked = false;
    public float attackTimer = 0;
    public float attackCooldownTime = 1f;
    Vector2 scale = new Vector2(1,1);
    Vector2 iScale = new Vector2(-1, 1);

    [SerializeField]GameObject attackPoint;
    [SerializeField]GameObject shieldPoint;
    public float attackRange = 0.5f;

    public LayerMask playerLayers;
    public int basicSwordDamage = 2;
    private int damageReduction = 1;

    //Mana 
    public int manaMax = 10;
    public int mana;

    //Items
    private ItemLibary itemLib;
    public string itemTag;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();

        user = input.user.index;

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        itemLib = GameObject.Find("GameManager").GetComponent<ItemLibary>();

        health = maxHealth;
        respawn = transform.position;
        mana = 10;
        points = 0;

        orderInLayer = 1;
        spriteRen.sortingOrder = orderInLayer;

        //movementSpeed = 0.1f;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;

    }

    void Update()
    {

        //numPlayers = GameObject.FindGameObjectWithTag

        //Movement
        tempVel = rb.velocity;

        if(moveAble)
        {
            tempVel.x = moveinput.x * movementSpeed;
            tempVel.y = moveinput.y * movementSpeed;
        }
        if(!moveAble)
        {
            tempVel = new Vector2(0,0);
        }

        if (movementSpeed <= movementSpeedStart)
            movementSpeed = movementSpeedStart;
        
        if (movementSpeed >= movementSpeedMax)
            movementSpeed = movementSpeedMax;
        if (tempVel == new Vector2(0, 0))
            moving = false;

        if (moving)
            movementSpeed += acceleration;
        else if (!moving)
            movementSpeed -= acceleration;

        rb.velocity = tempVel;
        anim.SetFloat("Velocity", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y));

        //Map Layering
        spriteRen.sortingOrder = orderInLayer;
        //Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), mapCollider);

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

        //Attack delay
        if(attacked)
        {
            if (attackTimer < attackCooldownTime)
            {
                attackTimer += Time.deltaTime;
            }
            else
            {
                attackTimer = 0;
                attacked = false;
            }
        }

        //Respawning
        if (died)
        {
            if (deathTimer < deathCooldownTime)
            {
                deathTimer += Time.deltaTime;
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

        if (context.performed)
            moving = true;
        if (context.canceled)
            moving = false;
        /*
        if(context.performed && movementSpeed < movementSpeedMax)
        {
        }
        */
        //Debug.Log(moveinput);
    }

    public void Sword(InputAction.CallbackContext context)
    {
        if(context.performed && !attacked)
        {
            anim.SetTrigger("Attack");
            moveAble = false;
            attacked = true;
            basicSwordDamage = 2;
        }
        
        if (context.canceled)
        {
            moveAble = true;
            //movementSpeed = movementSpeedStart;
        }
        
    }

    public void Shield(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetBool("Shield", true);
            moveAble = false;
            //movementSpeed = 0;
        }
        if (context.canceled)
        {
            anim.SetBool("Shield", false);
            moveAble = true;
            //movementSpeed = movementSpeedStart;
        }
    }

    public void ShieldBlocked(int damage)
    {
        Debug.Log(damage);
        damageReduction = 1;
        damage -= damageReduction;
        Debug.Log(damage);
        Damaged(damage);
    }

    public void Damaged(int damage)
    {
        Debug.Log(health);
        health -= damage;
        Debug.Log(health);
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

        if (name == "1_Player")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_2.x, playerManager.playerSpawn_2.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_3.x, playerManager.playerSpawn_3.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_4.x, playerManager.playerSpawn_4.y);
        }
        
        if (name == "2_Player")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_1.x, playerManager.playerSpawn_1.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_3.x, playerManager.playerSpawn_3.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_4.x, playerManager.playerSpawn_4.y);
        }
        
        if (name == "3_Player")
        {
            if (ranNum == 1)
                respawn = new Vector2(playerManager.playerSpawn_2.x, playerManager.playerSpawn_2.y);
            else if (ranNum == 2)
                respawn = new Vector2(playerManager.playerSpawn_1.x, playerManager.playerSpawn_1.y);
            else if (ranNum == 3)
                respawn = new Vector2(playerManager.playerSpawn_4.x, playerManager.playerSpawn_4.y);
        }
        
        if (name == "4_Player")
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

    public void Item1(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            itemLib.ItemLibFind(itemTag);
        }
    }

    public void Item2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            itemLib.ItemLibFind(itemTag);
        }
    }

    void Sort()
    {
        spriteRen.sortingLayerName = layerName;
        spriteRen.sortingOrder = orderInLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword" && !damaged)
        {
            killerName = collision.gameObject.transform.parent.name;
            Damaged(basicSwordDamage);
        }
        else if (collision.gameObject.tag == "Sword" && shieldUp && !damaged)
        {
            var nuller = (collision.transform.position - transform.position).normalized;
            var shield = (shieldPoint.transform.position - transform.position).normalized;
            //var angleallowed = 10;
            killerName = collision.gameObject.transform.parent.name;
            //ShieldBlocked(basicSwordDamage);
        }

        if(collision.gameObject.tag == "Top Stair")
        {
            if (!movingFloors)
                movingFloors = true;
            else
            {
                movingFloors = false;
                orderInLayer++;
            }
        }

        if (collision.gameObject.tag == "Bottom Stair")
        {
            if (!movingFloors)
                movingFloors = true;
            else
            {
                movingFloors = false;
                orderInLayer--;
            }
        }
    }

    public void RePair()
    {
        InputUser.PerformPairingWithDevice(inputDevice, input.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        anim.runtimeAnimatorController = skin[activeSkin];
    }
}

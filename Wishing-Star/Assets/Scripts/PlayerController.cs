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

    //Map Layering
    public TilemapRenderer tileRen;
    [SerializeField]int orderInLayer = 1;
    public string layerName;
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
    float healthCooldownTime = 0.5f;
    //private int damageTaken = 0;
    //Death
    private int ranNum;
    public float deathTimer = 0;
    private float deathCooldownTime = 5;
    private Vector2 diedPos = new Vector2(123, 456);
    public bool died;

    //Movement
    public float movementSpeedMax = 7.5f;
    public float movementSpeed = 3.75f;
    private float movementSpeedStart = 0f;
    private float shieldMoveSpeed = 2f;
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

    //Shield Stuff
    public LayerMask playerLayers;
    public int basicSwordDamage = 2;
    private int damageReduction = 1;
    private GameObject attacker;
    private bool validBlock;

    //Mana 
    public int manaMax = 10;
    public int mana;

    //Items
    private ItemLibary itemLib;
    public string itemTag1;
    public string itemTag2;
    public Vector2 facing;
    public Vector2 pos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();


        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        itemLib = GameObject.Find("GameManager").GetComponent<ItemLibary>();

        health = maxHealth;
        respawn = transform.position;
        mana = 10;
        points = 0;

        Order(orderInLayer);

        //movementSpeed = 0.1f;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;

    }

    void Update()
    {
        //Item stuff
        pos = transform.position;
        //Facing (for items)
        switch (characterFacing)
        {
            case Directions.Up:
                facing = new Vector2 (0, 1);
                break;

            case Directions.Down:
                facing = new Vector2(0, -1);
                break;

            case Directions.Right:
                facing = new Vector2(1, 0);
                break;

            case Directions.Left:
                facing = new Vector2(-1, 0);
                break;

            default:
                Debug.Log("You are facing the wrong way, and currently looking into the 4th dimintion");
                break;
        }

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
        if (shieldUp)
            tempVel = moveinput * shieldMoveSpeed;

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

        //Brief invencibility after getting hit with a attack
        if (damaged)
        {
            if (healthTimer < healthCooldownTime)
            {
                healthTimer += Time.deltaTime;
            }
            else if (healthTimer >= healthCooldownTime)
            {
                healthTimer = 0;
                damaged = false;
            }
        }

        //Shield collision
        //disToAttackerX = Mathf.Abs(attacker.transform.position.x - pos.x);
        //disToAttackerY = Mathf.Abs(attackerPos.y - pos.y);
        angle = Mathf.Atan2(Mathf.Abs(attacker.transform.position.y - pos.y), Mathf.Abs(attacker.transform.position.x - pos.x)) * Mathf.Rad2Deg;

        if(characterFacing == Directions.Up && shieldUp && angle > 0 && angle < 180)
            validBlock = true;
        if (characterFacing == Directions.Down && shieldUp && angle < 0 && angle > 180)
            validBlock = true;
        if (characterFacing == Directions.Left && shieldUp && angle > 90 && angle < 270)
            validBlock = true;
        if (characterFacing == Directions.Right && shieldUp && angle < 90 && angle > 270)
            validBlock = true;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveinput = context.ReadValue<Vector2>();

        if (context.performed)
            moving = true;
        if (context.canceled)
            moving = false;

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
            shieldUp = true;
            //moveAble = false;
            //movementSpeed = 0;
        }
        if (context.canceled)
        {
            anim.SetBool("Shield", false);
            shieldUp = false;
            //moveAble = true;
            //movementSpeed = movementSpeedStart;
        }
    }

    public void Item1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            itemTag1 = "Bow";
            itemLib.ItemLibFind(itemTag1, facing, pos);
        }
    }

    public void Item2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            itemLib.ItemLibFind(itemTag2, facing, pos);
        }
    }

    public void ShieldBlocked(int damage)
    {
        //Debug.Log(damage);
        damageReduction = 1;
        damage -= damageReduction;
        //Debug.Log(damage);
        
        //ADD KNOCKBACK FORCE!!!!

        Damaged(damage);
    }

    public void Damaged(int damage)
    {

        //Debug.Log(health);
        health -= damage;
        //Debug.Log(health);
        damaged = true;
        //Debug.Log(name + " was hit by " + killerName);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword" && !damaged)
        {
            killerName = collision.gameObject.transform.parent.name;
            attacker = collision.gameObject;

            //if Shield Blocking
            if (shieldUp && validBlock)
                ShieldBlocked(basicSwordDamage);

            //No Shield Blocking
            else
                Damaged(basicSwordDamage);
        }
        /*
        else if (collision.gameObject.tag == "Sword" && shieldUp && !damaged)
        {
            var nuller = (collision.transform.position - transform.position).normalized;
            var shield = (shieldPoint.transform.position - transform.position).normalized;
            //var angleallowed = 10;
            killerName = collision.gameObject.transform.parent.name;
            //ShieldBlocked(basicSwordDamage);
        }
        */
        if(collision.gameObject.tag == "Layer 1")
        {
            Order(1);
        }

        if (collision.gameObject.tag == "Layer 2")
        {
            Order(2);
        }
    }

    public void RePair()
    {
        InputUser.PerformPairingWithDevice(inputDevice, input.user, InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        input.SwitchCurrentControlScheme(inputDevice);
        anim.runtimeAnimatorController = skin[activeSkin];
    }

    void Order(int layer)
    {
        switch(layer)
        {
            case 1:
                spriteRen.sortingOrder = 1;
                Physics2D.IgnoreLayerCollision(6, 7, false);
                Physics2D.IgnoreLayerCollision(6, 8);
                break;
            case 2:
                spriteRen.sortingOrder = 3;
                Physics2D.IgnoreLayerCollision(6, 8, false);
                Physics2D.IgnoreLayerCollision(6, 7);
                break;
            default:
                spriteRen.sortingOrder = 1;
                Physics2D.IgnoreLayerCollision(6, 8);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //public int player = 0;
    private Rigidbody2D rb;
    public Collider2D col;
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
    int playerLayer;
    [SerializeField] Collider2D mapCollider;

    private PlayerManager playerManager;

    public GameObject[] players;
    public GameObject[] numPlayers;

    //Rewarding players with points
    public string killerName;
    public int points;

    //Leaderboard help
    public bool winner;
    public bool second;
    public bool third;
    public bool fourth;

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
    public Vector2 respawn;
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
    private float movementSpeedStart = 0.1f;
    private float shieldMoveSpeed = 2f;
    private float acceleration = 0.1f;
    Vector2 tempVel;
    private float maxVelocityX = 7.60f;
    private float maxVelocityY = 7.60f;
    [SerializeField] Vector2 moveinput = Vector2.zero;
    public bool moving = false;
    private bool moveAble = true;
   // private bool moveMaxX = false;
   // private bool moveMaxY = false;

    //Attacking
    Vector2 movingTo;
    public float angle;
    public float rotationSpeed;
    public bool shieldUp = false;
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
    public bool validBlock;
    private Vector2 eDirection;
    private Vector2 pDirection;
    private float knockbackForce = 20000;
    private Vector2 eKnockback;
    private Vector2 pKnockback;
    private GameObject attacker;

    //Mana 
    public int manaMax = 100;
    public float mana;
    public float tempMana;
    public bool manaUsed = false;
    public bool gainingMana = false;
    public float manaTimer = 0;
    private float manaCooldownTime = 5;

    //Items
    private ItemLibary itemLib;
    public string itemTag1;
    public string itemTag2;
    public Vector2 facing;
    public Vector2 pos;
    public bool canUse1;
    public bool canUse2;

    //private bool protection = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();


        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        itemLib = GameObject.Find("GameManager").GetComponent<ItemLibary>();

        playerLayer = gameObject.layer;
        anim.runtimeAnimatorController = skin[activeSkin];

        health = maxHealth;
        respawn = transform.position;
        manaMax = 100;
        mana = manaMax;
        tempMana = mana;
        manaUsed = false;
        points = 0;
        canUse1 = true;
        canUse2 = true;

        winner = false;
        second = false;
        third = false;
        fourth = false;

        Order(orderInLayer);

        //movementSpeed = 0.1f;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;

    }

    void Update()
    {
        //Item stuff
        pos = transform.position;

        //Mana Regeneration
        if(mana != tempMana)
        {
            if(gainingMana)
            {
                gainingMana = false;
            }

            manaUsed = true;
            manaTimer = 0;
            mana = tempMana;
        }

        if(manaUsed)
        {
            if (manaTimer < manaCooldownTime)
            {
                manaTimer += Time.deltaTime;
            }
            else
            {
                manaTimer = 0;
                manaUsed = false;
            }
        }

        //This doesn't work, right now while it is gaining mana you have infinate mana and can't stop it
        if(!manaUsed && mana != manaMax)
        {
            mana += (5 * Time.deltaTime);
            tempMana = mana;
            gainingMana = true;
        }

        if(mana > manaMax )
        {
            mana = manaMax;
            tempMana = mana;
            gainingMana = false;
        }
    
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
        //tempVel = rb.velocity;

        if (moving)
        {
            movementSpeed += acceleration;
            //Debug.Log(rb.velocity);
        }
        else if (!moving )
        {
            movementSpeed -= acceleration;
            //tempVel.x -= acceleration;
            //tempVel.y -= acceleration;
        }

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
        {
            tempVel = moveinput * shieldMoveSpeed;
            maxVelocityX = 3.4f;
            maxVelocityY = 3.4f;
        }
        if(!shieldUp)
        {
            maxVelocityX = 7.60f;
            maxVelocityY = 7.60f;
        }
        if (movementSpeed <= movementSpeedStart && !moving)
        {
            movementSpeed = movementSpeedStart;
        }

        
        if (movementSpeed >= movementSpeedMax)
            movementSpeed = movementSpeedMax;

        if (tempVel == new Vector2(0, 0))
            moving = false;

        //rb.velocity.x = Mathf.Clamp(tempVel.x, -maxVelocityX, maxVelocityX);
        rb.velocity += tempVel;
        rb.velocity = new Vector2 (Mathf.Clamp(rb.velocity.x, -maxVelocityX, maxVelocityX), Mathf.Clamp(rb.velocity.y, -maxVelocityY, maxVelocityY));
        //tempVel.y = Mathf.Clamp(tempVel.y, -maxVelocityY, maxVelocityY);
        /*
        if (moveinput.y == 0 && moveinput.x == 0)
        {
            tempVel.x = 0;
            tempVel.y = 0;
            Debug.Log(tempVel);
        }
        */
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
                attacker.GetComponent<PlayerController>().points += 1;
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


    }

    public void Move(InputAction.CallbackContext context)
    {
        WallCollision();
        moveinput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            moving = true;
        }

        if (context.canceled)
        {
            moving = false;
        }

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
            if(moveinput.y != 0 || moveinput.x != 0)
            {
                moving = true;
            }
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
            if(canUse1)
            {
                itemLib.ItemLibFind(itemTag1, facing, pos, mana, col, name);
            }

            //Mana Cost for items:
            if(itemTag1 == "Bow" && tempMana >= 10)
            {
                tempMana -= 10;
                canUse1 = true;
            }
            else if(itemTag1 == "Bomb" && tempMana >= 20 && itemLib.bombPlaced)
            {
                tempMana -= 20;
                canUse1 = true;
            }
            else
            {
                canUse1 = false;
            }
        }
    }

    public void Item2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            itemTag2 = "Bomb";
            if(canUse2)
            {
                itemLib.ItemLibFind(itemTag2, facing, pos, mana, col, name);
            }

            //Mana Cost for items:
            if(itemTag2 == "Bow" && tempMana >= 10)
            {
                tempMana -= 10;
                canUse2 = true;
            }
            else if(itemTag2 == "Bomb" && tempMana >= 20 && !itemLib.bombPlaced)
            {
                tempMana -= 20;
                canUse2 = true;
            }
            else
            {
                canUse2 = false;
            }
        }
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 4);
    }
    */
    public void ShieldBlocked(int damage, GameObject otherPlayer)
    {
        //Debug.Log(damage);
        damageReduction = 1;
        damage -= damageReduction;
        //Debug.Log(damage);

        Damaged(damage, otherPlayer);
    }

    public void Damaged(int damage, GameObject otherPlayer)
    {

        //Debug.Log(health);
        health -= damage;
        //Debug.Log(health);
        damaged = true;
        //Debug.Log(name + " was hit by " + killerName);

        //Maybe add a knockbackAble bool?
        eDirection = new Vector2(otherPlayer.transform.position.x - pos.x, otherPlayer.transform.position.y - pos.y);
        pDirection = new Vector2(pos.x - otherPlayer.transform.position.x, pos.y - otherPlayer.transform.position.y);

        eKnockback = eDirection * knockbackForce;
        pKnockback = pDirection * knockbackForce;

        otherPlayer.GetComponent<Rigidbody2D>().AddForce(eKnockback, ForceMode2D.Impulse);
        Debug.Log(eKnockback);
        rb.AddForce(pKnockback, ForceMode2D.Impulse);

        //Reaspawn
        if (health <= 0 && !died)
        {
            Debug.Log("Player " + name[0] + " was killed by Player " + killerName[0]);
            died = true;
            transform.SetPositionAndRotation(diedPos, Quaternion.identity);
            //Debug.LogWarning("TELEPORTED!!!");
        }
        
    }
    
    public void playerRespawnShuffle()
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
        if (collision.gameObject.tag == "Layer 1")
        {
            Order(1);
        }

        if (collision.gameObject.tag == "Layer 2")
        {
            Order(2);
        }

        if (collision.gameObject.tag == "Layer 3")
        {
            Order(3);
        }

        if (collision.gameObject.tag == "Layer 4")
        {
            Order(4);
        }

        if (collision.gameObject.tag == "Bow")
        {
            killerName = collision.gameObject.transform.name.Substring(0,8);

            attacker = collision.gameObject;
            Debug.Log(name + " " + attacker + " " +killerName);
        }

        else if (collision.gameObject.transform.parent.tag == "Player")
        {
            if(collision.gameObject.tag == "Sword")
            {
                killerName = collision.gameObject.transform.parent.name;

                attacker = collision.gameObject.transform.parent.gameObject;
            }
        }

        if ((attacker.transform.position.y - transform.position.y) > 0 && (attacker.transform.position.x - transform.position.x) > 0)
        {
            //quadrant 1
            angle = Mathf.Atan(Mathf.Abs((attacker.transform.position.y - transform.position.y) / (attacker.transform.position.x - transform.position.x))) * Mathf.Rad2Deg;
        }
        else if ((attacker.transform.position.y - transform.position.y) > 0 && (attacker.transform.position.x - transform.position.x) < 0)
        {
            //quadrant 2
            angle = Mathf.Atan(Mathf.Abs((attacker.transform.position.x - transform.position.x) / (attacker.transform.position.y - transform.position.y))) * Mathf.Rad2Deg + 90;
        }
        else if ((attacker.transform.position.y - transform.position.y) < 0 && (attacker.transform.position.x - transform.position.x) < 0)
        {
            //quadrant 3
            angle = Mathf.Atan(Mathf.Abs((attacker.transform.position.y - transform.position.y) / (attacker.transform.position.x - transform.position.x))) * Mathf.Rad2Deg + 180;
        }
        else if ((attacker.transform.position.y - transform.position.y) < 0 && (attacker.transform.position.x - transform.position.x) > 0)
        {
            //quadrant 4
            angle = Mathf.Atan(Mathf.Abs((attacker.transform.position.x - transform.position.x) / (attacker.transform.position.y - transform.position.y))) * Mathf.Rad2Deg + 270;
        }

        if (characterFacing == Directions.Up && shieldUp && angle >= 22.5 && angle <= 157.5)
        {
            validBlock = true;
        }

        else if (characterFacing == Directions.Down && shieldUp && angle <= 292.5 && angle >= 202.5)
        {
            validBlock = true;
        }

        else if (characterFacing == Directions.Left && shieldUp && angle >= 112.5 && angle <= 247.5)
        {
            validBlock = true;
        }

        else if (characterFacing == Directions.Right && shieldUp && angle <= 67.5 || angle >= 112.5 && characterFacing == Directions.Right && shieldUp)
        {
            validBlock = true;
        }

        if (damaged)
        {
            validBlock = false;
        }

        //Sword
        if (collision.gameObject.tag == "Sword" && !damaged)
        {
            //if Shield Blocking
            if (shieldUp && validBlock)
            {
                ShieldBlocked(basicSwordDamage, attacker);
            }

            //No Shield Blocking
            else
            {
                Damaged(basicSwordDamage, attacker);
            }
        }

        //Bow and Arrow
        if (collision.gameObject.tag == "Bow" && !damaged && collision.name.Substring(0, 8) != name)
        {
            Debug.Log("AHHHHH!");
            //if Shield Blocking
            if (shieldUp && validBlock)
            {
                Debug.Log("BILL!");
                ShieldBlocked(2, attacker);
            }

            //No Shield Blocking
            else
            {
                Damaged(2, attacker);
            }
        }
    }

    private void WallCollision()
    {
        float rayDis = 10f;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, facing, rayDis);
        if(hit.transform.gameObject.tag == "Player")
        {
            if(hit.transform.gameObject.GetComponent<PlayerController>().characterFacing == characterFacing && moveinput == new Vector2(0, 0))
            {

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            //UP
            if (collision.gameObject.GetComponent<PlayerController>().characterFacing == Directions.Up && moveinput.y != 0)
            {

            }
            //Down
            else if (collision.transform.position.y < pos.y)
            {

            }
            //Left
            else if (collision.transform.position.x < pos.x)
            {

            }
            //Right
            else if (collision.transform.position.x > pos.x)
            {

            }
            //Debug.Log("YOU ARE IN MY WAY");
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
                Physics2D.IgnoreLayerCollision(playerLayer, 6, false);
                Physics2D.IgnoreLayerCollision(playerLayer, 7);
                Physics2D.IgnoreLayerCollision(playerLayer, 8);
                Physics2D.IgnoreLayerCollision(playerLayer, 9);
                break;
            case 2:
                spriteRen.sortingOrder = 3;
                Physics2D.IgnoreLayerCollision(playerLayer, 6);
                Physics2D.IgnoreLayerCollision(playerLayer, 7, false);
                Physics2D.IgnoreLayerCollision(playerLayer, 8);
                Physics2D.IgnoreLayerCollision(playerLayer, 9);
                break;
            case 3:
                spriteRen.sortingOrder = 4;
                Physics2D.IgnoreLayerCollision(playerLayer, 6);
                Physics2D.IgnoreLayerCollision(playerLayer, 7);
                Physics2D.IgnoreLayerCollision(playerLayer, 8, false);
                Physics2D.IgnoreLayerCollision(playerLayer, 9);
                break;
            case 4:
                spriteRen.sortingOrder = 5;
                Physics2D.IgnoreLayerCollision(playerLayer, 6);
                Physics2D.IgnoreLayerCollision(playerLayer, 7);
                Physics2D.IgnoreLayerCollision(playerLayer, 8);
                Physics2D.IgnoreLayerCollision(playerLayer, 9, false);
                break;
            default:
                spriteRen.sortingOrder = 1;
                Physics2D.IgnoreLayerCollision(playerLayer, 7);
                break;
        }
    }
}

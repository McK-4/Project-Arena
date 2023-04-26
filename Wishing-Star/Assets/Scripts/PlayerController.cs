using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

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
    public bool moveAble = true;
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
    //private float knockbackForce = 20000;
    private float knockbackForce = 20;
    private Vector2 eKnockback;
    private Vector2 pKnockback;
    public GameObject attacker;

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
    private bool holding1;
    private bool holding2;
    private bool swapping1;
    private bool swapping2;
    private bool pickingup1;
    private bool pickingup2;

    [SerializeField] float pickUptimer = 0;
    [SerializeField] float pickUpCooldownTime = 2f;

    public bool leeched;
    [SerializeField] float leechtimer = 0;
    [SerializeField] float leechCooldownTime = 1f;

    private bool charging = false;
    [SerializeField] float chargetimer = 0;
    [SerializeField] float chargeCooldownTime = 3f;
    public int powerLvl = 0;
    public int boltDmg;

    //End Game (For switching scenes back to the menu)
    private GameManager gameManager;

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
        leeched = false;

        winner = false;
        second = false;
        third = false;
        fourth = false;

        Order(orderInLayer);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //movementSpeed = 0.1f;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;

    }

    void Update()
    {
        //Item stuff
        pos = transform.position;

        //Item Pickup
        if(itemTag1 != "")
        {
            holding1 = true;
        }
        if(itemTag1 == "")
        {
            holding1 = false;
        }
        if (itemTag2 != "")
        {
            holding2 = true;
        }
        if (itemTag2 == "")
        {
            holding2 = false;
        }

        if (swapping1)
        {
            if (pickUptimer < pickUpCooldownTime)
            {
                pickUptimer += Time.deltaTime;
            }
            else if (pickUptimer >= pickUpCooldownTime)
            {
                pickUptimer = 0;
                pickingup1 = true;
            }
        }

        //Mana Regeneration
        if (mana != tempMana)
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
                GameObject.Find(killerName).GetComponent<PlayerController>().points += 1;
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

        //Dark Leech CoolDown Timer
        if(leeched)
        {
            if (leechtimer < leechCooldownTime)
            {
                leechtimer += Time.deltaTime;
            }
            else if (leechtimer >= leechCooldownTime)
            {
                leechtimer = 0;
                leeched = false;
            }
        }

        //Glove of Thunder charge up
        if(charging && powerLvl < 2)
        {
            if (chargetimer < chargeCooldownTime)
            {
                chargetimer += Time.deltaTime;
            }
            else if (chargetimer >= chargeCooldownTime)
            {
                chargetimer = 0;
                powerLvl += 1;
            }
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        if(moveAble)
        {
            moveinput = context.ReadValue<Vector2>();
        }

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

        if (gameManager.readyForScene)
        {
            moveAble = false;
            SceneManager.LoadScene(0);
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
            if(!holding1)
            {
                pickingup1 = true;
            }
            if(holding1)
            {
                swapping1 = true;
            }
            if(canUse1 && itemTag1 != "Glove of Thunder")
            {
                itemLib.ItemLibFind(itemTag1, facing, pos, mana, col, name, powerLvl);
            }

            if(canUse1 && itemTag1 == "Glove of Thunder")
            {
                charging = true;
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
            else if(itemTag1 == "Dark Leech" && tempMana >= 50)
            {
                tempMana -= 50;
                canUse1 = true;
            }
            else if(itemTag1 == "Tome of Ash" && tempMana >= 25)
            {
                tempMana -= 25;
                canUse1 = true;
            }
            else if(itemTag1 == "Glove of Thunder" && tempMana >= 5)
            {
                tempMana -= 5;
                canUse1 = true;
            }
            else
            {
                canUse1 = false;
            }
        }

        if(context.canceled)
        {
            if(pickingup1)
            {
                pickingup1 = false;
            }
            if (swapping1)
            {
                swapping1 = false;
            }
            if (canUse1 && itemTag1 == "Glove of Thunder" && charging)
            {
                charging = false;
                chargetimer = 0f;
                itemLib.ItemLibFind(itemTag1, facing, pos, mana, col, name, powerLvl);
            }
            if(itemTag1 == "Glove of Thunder")
            {
                if(powerLvl == 0)
                {
                    boltDmg = 1;
                    Debug.Log("Bolt Damage is " + boltDmg);
                }
                else if(powerLvl == 1)
                {
                    //5 mana is being added becuase it was taken away for the instant charge (also it's less confusing than taking away 5 from the end cost)
                    tempMana += 5;

                    tempMana -= 20;
                    powerLvl = 0;
                    boltDmg = 4;
                }
                else if(powerLvl == 2)
                {
                    //5 mana is being added becuase it was taken away for the instant charge
                    tempMana += 5;

                    tempMana -= 40;
                    powerLvl = 0;
                    boltDmg = 7;
                }
            }
        }
    }

    public void Item2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!holding2)
            {
                pickingup2 = true;
            }
            Debug.Log(itemTag2);
            if(canUse2)
            {
                itemLib.ItemLibFind(itemTag2, facing, pos, mana, col, name, powerLvl);
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
            else if(itemTag2 == "Dark Leech" && tempMana >= 50)
            {
                tempMana -= 50;
                canUse2 = true;
            }
            else if(itemTag2 == "Tome of Ash" && tempMana >= 25)
            {
                tempMana -= 25;
                canUse2 = true;
            }
            else
            {
                canUse2 = false;
            }
        }
        if(context.canceled)
        {
            if (pickingup2)
            {
                pickingup2 = false;
            }
            if (swapping2)
            {
                swapping2 = false;
            }
        }
    }
    /* area test for bomb and dark leech
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
        
        if(pickingup1 && collision.gameObject.tag == "Pick Up")
        {
            Debug.Log("LOU LOU");
            itemTag1 = collision.gameObject.name;
            Destroy(collision.gameObject);
        }

        if (pickingup2 && collision.gameObject.tag == "Pick Up")
        {
            Debug.Log("BAW???");
            itemTag2 = collision.gameObject.name;
            Destroy(collision.gameObject);
        }

        //Getting item extra info
        if (collision.gameObject.tag == "Bow" && name != collision.gameObject.transform.name.Substring(0,8) && collision.gameObject.transform.name.Substring(1,1) == "_")
        {
            killerName = collision.gameObject.transform.name.Substring(0,8);

            attacker = collision.gameObject;
            Destroy(collision.gameObject);
            Debug.Log(name + " " + attacker + " " +killerName);
        }

        if (collision.gameObject.tag == "Tome of Ash" && name != collision.gameObject.transform.name.Substring(0,8) && collision.gameObject.transform.name.Substring(1, 1) == "_")
        {
            killerName = collision.gameObject.transform.name.Substring(0,8);

            attacker = collision.gameObject;
            Destroy(collision.gameObject);
            Debug.Log(name + " " + attacker + " " +killerName);
        }

        if (collision.gameObject.tag == "Glove of Thunder" && name != collision.gameObject.transform.name.Substring(0,8) && collision.gameObject.transform.name.Substring(1, 1) == "_")
        {
            killerName = collision.gameObject.transform.name.Substring(0,8);

            attacker = collision.gameObject;
            Destroy(collision.gameObject);
            Debug.Log(name + " " + attacker + " " +killerName);
        }

        if(collision.gameObject.tag == "Sword")
        {
            killerName = collision.gameObject.transform.parent.name;

            attacker = collision.gameObject.transform.parent.gameObject;
        }
        
        //Making sure that there is an attacker before getting a angle
        if(attacker != null)
        {
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
        if (collision.gameObject.tag == "Sword" && !damaged && gameObject.GetComponent<SpriteRenderer>().sortingOrder == collision.gameObject.transform.parent.GetComponent<SpriteRenderer>().sortingOrder)
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
        if (collision.gameObject.tag == "Bow" && !damaged && collision.name.Substring(0, 8) != name && collision.gameObject.transform.name.Substring(1, 1) == "_")
        {
            //Debug.Log("AHHHHH!");
            //if Shield Blocking
            if (shieldUp && validBlock)
            {
                //Debug.Log("BILL!");
                ShieldBlocked(2, attacker);
            }

            //No Shield Blocking
            else
            {
                Damaged(2, attacker);
            }
        }

        //Tome of Ash
        if (collision.gameObject.tag == "Tome of Ash" && !damaged && collision.name.Substring(0, 8) != name && collision.gameObject.transform.name.Substring(1, 1) == "_")
        {
            //Debug.Log("AHHHHH!");
            //if Shield Blocking
            if (shieldUp && validBlock)
            {
                //Debug.Log("BILL!");
                ShieldBlocked(4, attacker);
            }

            //No Shield Blocking
            else
            {
                Damaged(4, attacker);
            }
        }

        //Glove of Thunder
        if (collision.gameObject.tag == "Glove of Thunder" && !damaged && collision.name.Substring(0, 8) != name && collision.gameObject.transform.name.Substring(1, 1) == "_")
        {
            //Debug.Log("Power level is " + powerLvl);
            //Debug.Log("My Bolt Damage is " + boltDmg);
            Debug.Log("Your Bolt Damage is " + GameObject.Find(killerName).GetComponent<PlayerController>().boltDmg);
            /*
            if(boltDmg == 0)
            {
                boltDmg = 1;
            }
            */
            //Debug.Log("AHHHHH!");
            //if Shield Blocking
            if (shieldUp && validBlock)
            {
                //Debug.Log("BILL!");
                ShieldBlocked(GameObject.Find(killerName).GetComponent<PlayerController>().boltDmg, attacker);
            }

            //No Shield Blocking
            else
            {
                Damaged(GameObject.Find(killerName).GetComponent<PlayerController>().boltDmg, attacker);
            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (pickingup1 && collision.gameObject.tag == "Pick Up")
        {
            Debug.Log("LOU LOU");
            itemTag1 = collision.gameObject.name;
            Destroy(collision.gameObject);
        }

        if (pickingup2 && collision.gameObject.tag == "Pick Up")
        {
            Debug.Log("BAW???");
            itemTag2 = collision.gameObject.name;
            Destroy(collision.gameObject);
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
        Debug.Log("Pairing" + inputDevice);
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
                spriteRen.sortingOrder = 5;
                Physics2D.IgnoreLayerCollision(playerLayer, 6);
                Physics2D.IgnoreLayerCollision(playerLayer, 7);
                Physics2D.IgnoreLayerCollision(playerLayer, 8, false);
                Physics2D.IgnoreLayerCollision(playerLayer, 9);
                break;
            case 4:
                spriteRen.sortingOrder = 7;
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

    public IEnumerator Teleported(int layer)
    {
        Order(layer);
        rb.velocity = Vector2.zero;
        moveAble = false;
        rb.AddForce(Vector2.down * 20, ForceMode2D.Impulse);
        gameObject.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.2f);
        moveAble = true;
        characterFacing = Directions.Down;
        anim.SetInteger("Direction", 0);
    }
}

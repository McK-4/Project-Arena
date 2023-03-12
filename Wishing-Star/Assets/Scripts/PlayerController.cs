using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    Animator anim;
    //private InputActionMap playerControls;
    private PlayerManager playerManager;

    public GameObject[] players;
    public GameObject[] numPlayers;

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

    //Movement
    public float movementSpeed = 7.5f;
    Vector2 tempVel;
    Vector2 moveinput = Vector2.zero;

    //Attacking
    Vector2 movingTo;
    public float angle;
    public float rotationSpeed;
    bool shieldUp = false;

    [SerializeField]GameObject attackPoint;
    [SerializeField]GameObject shieldPoint;
    private CircleCollider2D shieldCollider;
    public float attackRange = 0.5f;

    public LayerMask playerLayers;
    public int basicSwordDamage = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        attackPoint = GameObject.Find("AttackPoint");
        shieldPoint = GameObject.Find("ShieldPoint");

        //shieldCollider = shieldPoint.GetComponent<CircleCollider2D>();

        health = maxHealth;

        respawn = transform.position;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;

    }

    // Update is called once per frame
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
            if(shieldPoint.transform.position != new Vector3(transform.position.x, transform.position.y + 0.345f) && attackPoint.transform.position != new Vector3(transform.position.x, transform.position.y + 0.345f))
            {
                shieldPoint.transform.position = new Vector3(transform.position.x, transform.position.y + 0.345f);
                attackPoint.transform.position = new Vector2(transform.position.x, transform.position.y + 0.345f);
            }
        }
        if (moveinput.y < 0)
        {
            characterFacing = Directions.Down;
            anim.SetInteger("Direction", 0);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            if (shieldPoint.transform.position != new Vector3(transform.position.x, transform.position.y - 0.345f) && attackPoint.transform.position != new Vector3(transform.position.x, transform.position.y - 0.345f))
            {
                shieldPoint.transform.position = new Vector3(transform.position.x, transform.position.y - 0.345f);
                attackPoint.transform.position = new Vector3(transform.position.x, transform.position.y - 0.345f);
            }
        }
        if(moveinput.x > 0)
        {
            characterFacing = Directions.Right;
            anim.SetInteger("Direction", 2);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            if (shieldPoint.transform.position != new Vector3(transform.position.x + 0.345f, transform.position.y) && attackPoint.transform.position != new Vector3(transform.position.x + 0.345f, transform.position.y))
            {
                shieldPoint.transform.position = new Vector3(transform.position.x + 0.345f, transform.position.y);
                attackPoint.transform.position = new Vector2(transform.position.x + 0.345f, transform.position.y);
            }
        }
        if(moveinput.x < 0)
        {
            characterFacing = Directions.Left;
            anim.SetInteger("Direction", 2);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            if (shieldPoint.transform.position != new Vector3(transform.position.x - 0.345f, transform.position.y) && attackPoint.transform.position != new Vector3(transform.position.x - 0.345f, transform.position.y))
            {
                shieldPoint.transform.position = new Vector3(transform.position.x - 0.345f, transform.position.y);
                attackPoint.transform.position = new Vector2(transform.position.x - 0.345f, transform.position.y);
            }
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
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveinput = context.ReadValue<Vector2>();
        Debug.Log(moveinput);
    }

    public void Sword(InputAction.CallbackContext context)
    {

        StartCoroutine(Sword());

        /*
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayers)
        {
            //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player);
            Debug.Log("Hit " + player.name);
        }
        */
    }

    IEnumerator Sword()
    {
        attackPoint.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        attackPoint.SetActive(false);
    }

    public void Shield(InputAction.CallbackContext context)
    {
        shieldUp = context.ReadValueAsButton();

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
        Debug.Log(name + " was hit");

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
        if (health <= 0)
        {
            transform.SetPositionAndRotation(respawn, Quaternion.identity);
            health = maxHealth;
            Debug.Log("Player "+ name +" died!");
        }
    }
    
    private void playerRespawnShuffle()
    {
        if (name == "Player_1")
        {
            //respawn = new Vector2(PlayerManager.player_1.transform.position.x, PlayerManager.playerSpawn_2.y);
        }
        
        if (name == "Player_2")
        {
            respawn = new Vector2(2, 2);
        }
        
        if (name == "Player_3")
        {
            respawn = new Vector2(2, 2);
        }
        
        if (name == "Player_4")
        {
            respawn = new Vector2(2, 2);
        }
    }

    /*
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
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

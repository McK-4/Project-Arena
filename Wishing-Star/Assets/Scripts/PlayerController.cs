using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private InputActionReference movement, sword, shield;

    public GameObject[] players;
    public GameObject[] numPlayers;
    public GameObject[] attackPoints;

    private int Players;

    //Health
    //Vector2 Respawn;
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
    float angle;
    bool shieldUp = false;

    private GameObject attackPoint;
    private GameObject shieldPoint;
    private CircleCollider2D shieldCollider;
    public float attackRange = 0.5f;

    public LayerMask playerLayers;
    public int basicSwordDamage = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        attackPoint = GameObject.Find("AttackPoint");
        shieldPoint = GameObject.Find("ShieldPoint");

        shieldCollider = shieldPoint.GetComponent<CircleCollider2D>();

        attackPoint.SetActive(false);
        shieldPoint.SetActive(false);

        health = maxHealth;

        // Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
        // name = "Player: "+ i;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Make the attackpoint rotate to where the player is moving

        //numPlayers = GameObject.FindGameObjectWithTag

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) ;

        //Movement
        tempVel = rb.velocity;
        tempVel.x = moveinput.x * movementSpeed;
        tempVel.y = moveinput.y * movementSpeed;
        rb.velocity = tempVel;

        if (shieldUp)
        {
            attackPoint.SetActive(false);
            shieldPoint.SetActive(true);

            //swordNshield.isTrigger = false;
        }
        else
        {
            shieldPoint.SetActive(false);
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        //moveinput = movement.action.ReadValue<Vector2>;
    }

    public void Sword(InputAction.CallbackContext context)
    {

        StartCoroutine(Sword());
        //GameObject s = Instantiate(attackPoint, attackPoint.transform.position, Quaternion.identity);
        //Destroy(s, 0.4f);

        //Debug.Log("Attacked");

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

        if (health <= 0)
        {
            transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);
            health = maxHealth;
            Debug.Log("Player "+ name +" died!");
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
            var angleallowed = 10;

            ShieldBlocked(basicSwordDamage);
        }

    }
}

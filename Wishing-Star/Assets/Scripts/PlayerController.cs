using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject[] players;
    public GameObject[] attackPoints;

    //Health
    public int maxHealth = 3;
    public int health;

    //Movement
    public float movementSpeed = 7.5f;
    Vector2 tempVel;
    Vector2 moveinput = Vector2.zero;

    //Attacking
    bool attack = false;
    public GameObject attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;
    public int attackDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;

        for(int i = 0;i < 4;i++)
            Physics2D.IgnoreCollision(players[i].GetComponent<BoxCollider2D>(), attackPoints[i].GetComponent<CircleCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        tempVel = rb.velocity;
        tempVel.x = moveinput.x * movementSpeed;
        tempVel.y = moveinput.y * movementSpeed;
        rb.velocity = tempVel;

    }

    public void Move(InputAction.CallbackContext context)
    {
        moveinput = context.ReadValue<Vector2>();
    }

    public void Sword(InputAction.CallbackContext context)
    {
        attack = context.ReadValueAsButton();

        GameObject s = Instantiate(attackPoint, attackPoint.transform.position, Quaternion.identity);
        Destroy(s, 0.4f);
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

    public void Damage(int damge)
    {
        health -= damge;
        Debug.Log(name+" was hit");
        if (health <= 0)
            Debug.Log("Player died!");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            Damage(attackDamage);
        }
    }
}

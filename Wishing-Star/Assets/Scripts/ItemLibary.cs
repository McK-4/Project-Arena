using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibary : MonoBehaviour
{
    //PlayerController pC;

    public string itemTag;

    //Bow
    [SerializeField] GameObject arrow;
    private float angle;

    //Bomb
    [SerializeField] GameObject bomb;
    [SerializeField] float bombtimer = 0;
    [SerializeField] float bombCooldownTime = 3f;
    public bool bombPlaced = false;
    [SerializeField] bool exploded = false;
    [SerializeField] string bombName;
    private GameObject bombSummoned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Timer until bomb explodes
        if(bombPlaced)
        {
            if (bombtimer < bombCooldownTime)
            {
                bombtimer += Time.deltaTime;
            }
            else if (bombtimer >= bombCooldownTime)
            {
                bombtimer = 0;
                exploded = true;
            }
        }

        //When Bomb explodes
        if(exploded)
        {
            
            bombSummoned = GameObject.Find(bombName);

            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(GameObject.Find(bombName).transform.position, 4);
            
            foreach(Collider2D player in hitPlayers)
            {
                if(player.gameObject.tag == "Player")
                {
                    if(player.gameObject.GetComponent<PlayerController>().shieldUp && player.gameObject.GetComponent<PlayerController>().validBlock)
                    {
                        player.gameObject.GetComponent<PlayerController>().ShieldBlocked(6, bombSummoned);
                        exploded = false;
                        Destroy(bombSummoned);
                        bombPlaced = false;
                    }
                    else
                    {
                        player.gameObject.GetComponent<PlayerController>().Damaged(6, bombSummoned);
                        exploded = false;
                        Destroy(bombSummoned);
                        bombPlaced = false;
                    }
                }
            }
            
        }
    }

    public void ItemLibFind(string tag, Vector2 direction, Vector2 pos, float mana, Collider2D col, string name)
    {
        //attacker = who is shooting the arrow

        switch (tag)
        {
            case "Bow":
                Bow(direction, pos, col, name);
                break;

            case "Bomb":
                Bomb(pos, direction, name);
                break;
            
            case "Dark Leech":
                break;
            
            case "Invisibility Mask":
                break;

            case "Tome of Ash":
                break;

            case "Glove of Thunder":
                break;

            default:
                Debug.Log("Nothing");
                break;
        }
        
    }
    
    private void Bow(Vector2 direction, Vector2 pos, Collider2D col, string attacker)
    {
        //The bow needs a "drawback" delay so you can't spam it

        //Getting the angle:
        //Up
        if(direction == new Vector2 (0, 1))
        {
            angle = 0;
        }
        //Down
        else if (direction == new Vector2(0, -1))
        {
            angle = 180;
        }
        //Right
        else if (direction == new Vector2(1, 0))
        {
            angle = 270;
        }
        //Left
        else if (direction == new Vector2(-1, 0))
        {
            angle = 90;
        }

        GameObject a = Instantiate(arrow, pos, Quaternion.Euler(0, 0, angle) );
        Physics2D.IgnoreCollision(col, a.GetComponent<Collider2D>());
        a.GetComponent<Rigidbody2D>().velocity = direction * 10;
        Destroy(a, 2f);

        a.name = (attacker + "'s arrow");

    }
    
    private void Bomb(Vector2 pos, Vector2 direction, string attacker)
    {
        GameObject b = Instantiate(bomb, pos + direction, Quaternion.identity);
        bombPlaced = true;

        b.name = (attacker + "'s bomb");
        bombName = b.name;
        //Timer until it explodes
        /*
        bombtimer = 0;
        if (bombtimer < bombCooldownTime)
        {
            bombtimer += Time.deltaTime;
        }
        else if (bombtimer >= bombCooldownTime)
        {
            bombtimer = 0;
            exploded = true;
            Destroy(b);
        }
        */
        //When Bomb explodes
        /*
        if(exploded)
        {
            
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(b.transform.position, 4);
            
            foreach(Collider2D player in hitPlayers)
            {
                if(player.gameObject.tag == "Player")
                {
                    bombSummoned = gameObject.Find(bombName);
                    if(player.gameObject.GetComponent<PlayerController>().shieldUp && player.gameObject.GetComponent<PlayerController>().validBlock)
                    {
                        player.gameObject.GetComponent<PlayerController>().ShieldBlocked(6, bombSummoned);
                    }
                    else
                    {
                        player.gameObject.GetComponent<PlayerController>().Damaged(6, bombSummoned);
                    }
                }
            }
            
            
        }
        */
    }

}

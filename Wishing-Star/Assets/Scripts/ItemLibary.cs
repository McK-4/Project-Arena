using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibary : MonoBehaviour
{
    //PlayerController pC;

    public string itemTag;

    private float angle;

    //Bow
    [SerializeField] GameObject arrow;

    //Bomb
    [SerializeField] GameObject bomb;
    [SerializeField] float bombtimer = 0;
    [SerializeField] float bombCooldownTime = 3f;
    public bool bombPlaced = false;
    [SerializeField] bool exploded = false;
    [SerializeField] string bombName;
    private GameObject bombSummoned;

    //Dark Leech
    [SerializeField] GameObject leech;
    private string leechName;
    private bool leechThrown = false;

    //Tome of Ash
    [SerializeField] GameObject ash;

    //Glove of Thunder
    [SerializeField] GameObject bolt;

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

        if(leechThrown && GameObject.Find(leechName) != null)
        {
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(GameObject.Find(leechName).transform.position, 4);

            foreach(Collider2D player in hitPlayers)
            {
                if(player.gameObject.tag == "Player")
                {
                    if(!player.gameObject.GetComponent<PlayerController>().leeched)
                    {
                        player.gameObject.GetComponent<PlayerController>().tempMana -= 10;
                        player.gameObject.GetComponent<PlayerController>().leeched = true;
                    }
                }
            }
        }

    }

    public void ItemLibFind(string tag, Vector2 direction, Vector2 pos, out int minusMana, Collider2D col, string name, int powerLvl)
    {
        //name = who is shooting the arrow
        
        Debug.Log(tag);
        minusMana = 0;
        switch (tag)
        {
            case "Bow":
                Bow(direction, pos, col, name);
                minusMana = 10;
                break;

            case "Bomb":
                Bomb(pos, direction, name);
                minusMana = 20;
                break;

            case "Dark Leech":
                DarkLeech(pos, direction, name);
                minusMana = 50;
                break;
            case "Glove of Thunder":
                Debug.Log("properStatment");
                GloveOfThunder(pos, direction, name);
                if (powerLvl == 1)
                {
                    //returning 5 less than the actual cost because it is accounting for the instant take away
                    minusMana = 20;
                }
                else if (powerLvl == 2)
                {
                    //returning 5 less than the actual cost because it is accounting for the instant take away
                    minusMana = 40;
                }
                else
                {
                    minusMana = 5;
                }
                break;
            case "Tome of Ash":
                TomeOfAsh(pos, direction, name);
                minusMana = 25;
                break;
            case "Invisibility Mask":
                minusMana = 0;
                break;
        }
    }
    
    private void Bow(Vector2 direction, Vector2 pos, Collider2D col, string attacker)
    {
        //The bow needs a "drawback" delay so you can't spam it

        //Getting the angle:
        Angle(direction);

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
    }

    private void DarkLeech(Vector2 pos, Vector2 direction, string attacker)
    {
        //Getting the angle:
        Angle(direction);

        GameObject dl = Instantiate(leech, pos + direction, Quaternion.Euler(0,0, angle));
        dl.GetComponent<Rigidbody2D>().velocity = direction * 2.5f;
        leechThrown = true;
        dl.name = (attacker + "'s dark leech");
        leechName = dl.name;
        Destroy(dl, 7f);
    }
    
    private void GloveOfThunder(Vector2 pos, Vector2 direction, string attacker)
    {
        //Getting the angle: 
        Angle(direction);

        //lb = lighting bolt
        GameObject lb = Instantiate(bolt, pos + direction, Quaternion.Euler(0,0, angle));
        lb.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        lb.name = (attacker + "'s lighting bolt");
        Destroy(lb, 2f);

    }

    private void TomeOfAsh(Vector2 pos, Vector2 direction, string attacker)
    {
        //Getting the angle: 
        Angle(direction);

        GameObject ta = Instantiate(ash, pos + direction, Quaternion.Euler(0,0, angle));
        ta.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        ta.name = (attacker + "'s ash");
        Destroy(ta, 2f);
    }

    private void Angle(Vector2 direction)
    {
        //Getting the angle: 
        //Up
        if (direction == new Vector2(0, 1))
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
    }
}

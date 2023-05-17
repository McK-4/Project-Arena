using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibary : MonoBehaviour
{
    //PlayerController pC;

    public string itemTag;

    private float angle;

    Animator anim;

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
    GameObject fireballSummoned;
    public bool fireballHit;
    float fireballTimer;
    float fireballTime = 1.5f;
    bool fireShoot;

    //Glove of Thunder
    [SerializeField] GameObject bolt;
    private bool adjusted = false;

    //Invisiblity
    Color invisColor = new Vector4(255,255,255,80);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //fireball despawn
        if (fireShoot)
        {
            if (fireballTimer < fireballTime)
            {
                fireballTimer += Time.deltaTime;
            }
            else if (fireballTimer >= fireballTime)
            {
                fireShoot = false;
                fireballTimer = 0;
                fireballSummoned.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                StartCoroutine(fireballExplotion());
            }
            else if (fireballHit)
            {
                fireShoot = false;
                fireballTimer = 0;
                fireballSummoned.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                StartCoroutine(fireballExplotion());
            }
        }

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

            bombSummoned.GetComponent<AudioSource>().Play();

            bombSummoned.transform.GetChild(0).gameObject.SetActive(true);

            exploded = false;
            bombPlaced = false;

            StartCoroutine(bombExplotion());


            /*
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(GameObject.Find(bombName).transform.position, 4);

            foreach(Collider2D player in hitPlayers)
            {
                if(player.gameObject.tag == "Player")
                {
                    if(player.gameObject.GetComponent<PlayerController>().shieldUp && player.gameObject.GetComponent<PlayerController>().validBlock)
                    {
                        player.gameObject.GetComponent<PlayerController>().ShieldBlocked(6, bombSummoned);
                        player.gameObject.GetComponent<PlayerController>().killerName = bombName.Substring(0, 8);
                        exploded = false;
                        Destroy(bombSummoned);
                        bombPlaced = false;
                    }
                    else
                    {
                        player.gameObject.GetComponent<PlayerController>().Damaged(6, bombSummoned);
                        player.gameObject.GetComponent<PlayerController>().killerName = bombName.Substring(0, 8);
                        exploded = false;
                        Destroy(bombSummoned);
                        bombPlaced = false;
                    }
                }
            }
            
            exploded = false;
            Destroy(bombSummoned);
            bombPlaced = false;
            */
        }

        //Leeching player's magic
        if (leechThrown && GameObject.Find(leechName) != null)
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

    public void ItemLibFind(string tag, Vector2 direction, Vector2 pos, out int minusMana, Collider2D col, string name, int powerLvl, GameObject player, out bool invisible)
    {
        //name = who is shooting the arrow
        
        //Debug.Log(tag);
        minusMana = 0;
        invisible = false;

        switch (tag)
        {
            //Items
            case "Bow":
                Bow(direction, pos, col, name);
                minusMana = 10;
                invisible = false;
                break;

            case "Bomb":
                Bomb(pos, direction, name);
                minusMana = 20;
                invisible = false;
                break;

            case "Dark Leech":
                DarkLeech(pos, direction, name);
                minusMana = 50;
                invisible = false;
                break;
            case "Glove of Thunder":
                Debug.Log("properStatment");
                GloveOfThunder(pos, direction, name, powerLvl);
                if (powerLvl == 1)
                {
                    //returning 5 less than the actual cost because it is accounting for the instant take away (This may not be needed)
                    minusMana = 100;
                }
                else if (powerLvl == 2)
                {
                    //returning 5 less than the actual cost because it is accounting for the instant take away (This may not be needed)
                    minusMana = 100;
                }
                else
                {
                    minusMana = 100;
                }
                invisible = false;
                break;
            case "Tome of Ash":
                Debug.Log("correct statment");
                TomeOfAsh(pos, direction, name);
                minusMana = 25;
                invisible = false;
                break;
            case "Invisibility Mask":
                InvisibilityMask(player, invisible);
                if (!invisible)
                {
                    invisible = true;
                    minusMana = 10;
                }
                else if(invisible)
                {
                    invisible = false;
                    minusMana = 0;
                }
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
    
    private void GloveOfThunder(Vector2 pos, Vector2 direction, string attacker, int powerLvl)
    {
        //Getting the angle: 
        //Angle(direction);

        //lb = lighting bolt
        if(powerLvl == 1)
        {
            //Quaternion.Euler(0, 0, angle)
            GameObject lb1 = Instantiate(bolt, pos + direction, Quaternion.identity);

            lb1.name = (attacker + "'s lighting bolt");

            Destroy(lb1, 2f);
        }
        else if (powerLvl == 2)
        {
            GameObject lb1 = Instantiate(bolt, pos + direction, Quaternion.identity);

            if (direction == new Vector2(0, 1) || direction == new Vector2(0, -1))
            {
                direction.y *= 2f;
            }
            else if (direction == new Vector2(1, 0) || direction == new Vector2(-1, 0))
            {
                direction.x *= 2f;
            }

            GameObject lb2 = Instantiate(bolt, pos + direction, Quaternion.identity);

            lb1.name = (attacker + "'s lighting bolt");
            lb2.name = (attacker + "'s lighting bolt");

            Destroy(lb1, 2f);
            Destroy(lb2, 2f);
        }
        else if (powerLvl == 3)
        {
            GameObject lb1 = Instantiate(bolt, pos + direction, Quaternion.identity);

            if (direction == new Vector2(0, 1) || direction == new Vector2(0, -1))
            {
                direction.y *= 2f;
            }
            else if (direction == new Vector2(1, 0) || direction == new Vector2(-1, 0))
            {
                direction.x *= 2f;
            }

            GameObject lb2 = Instantiate(bolt, pos + direction, Quaternion.identity);

            if (direction == new Vector2(0, 1) || direction == new Vector2(0, -1))
            {
                direction.y *= 1.5f;
            }
            else if (direction == new Vector2(1, 0) || direction == new Vector2(-1, 0))
            {
                direction.x *= 1.5f;
            }

            GameObject lb3 = Instantiate(bolt, pos + direction, Quaternion.identity);

            lb1.name = (attacker + "'s lighting bolt");
            lb2.name = (attacker + "'s lighting bolt");
            lb3.name = (attacker + "'s lighting bolt");

            Destroy(lb1, 2f);
            Destroy(lb2, 2f);
            Destroy(lb3, 2f);
        }
        /*
        GameObject lb = Instantiate(bolt, pos + direction, Quaternion.Euler(0,0, angle));
        lb.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        lb.name = (attacker + "'s lighting bolt");
        */
        //Destroy(lb, 2f);

    }

    private void TomeOfAsh(Vector2 pos, Vector2 direction, string attacker)
    {
        //Getting the angle: 
        Angle(direction);

        fireballSummoned = Instantiate(ash, pos + direction, Quaternion.Euler(0,0, angle));
        fireballSummoned.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        anim = fireballSummoned.GetComponent<Animator>();
        fireballSummoned.name = (attacker + "'s ash");
        fireShoot = true;
    }

    private void InvisibilityMask(GameObject player, bool invisible)
    {
        if(invisible)
        {
            player.GetComponent<SpriteRenderer>().color = invisColor;
        }
        else if(!invisible)
        {
            player.GetComponent<SpriteRenderer>().color = Color.white;
        }
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
    private void lightingPos(Vector2 direction)
    {
        if (!adjusted)
        {
            if (direction == new Vector2(0, 1) || direction == new Vector2(0, -1))
            {

            }
            else if (direction == new Vector2(1, 0) || direction == new Vector2(-1, 0))
            {

            }
        }
    }
    IEnumerator bombExplotion()
    {
        bombSummoned.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        yield return new WaitForSeconds(1);

        Destroy(bombSummoned);
    }

    IEnumerator fireballExplotion()
    {
        anim.SetTrigger("Explode");
        yield return new WaitForSeconds(0.5f);
        Destroy(fireballSummoned);
        fireballHit = false;
    }
}

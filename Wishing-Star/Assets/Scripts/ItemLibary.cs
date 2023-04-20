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
    private float bombtimer = 0;
    private float bombCooldownTime = 3f;
    private bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                Bomb(pos, direction);
                break;

            case "Blizzard Storm":
                break;

            case "Blazing Storm":
                break;
            
            case "Dark Leech":
                break;
            
            case "Invisibility Mask":
                break;
            
            case "Glove of Thunder":
                break;

            case "Tome of Ash":
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
    
    private void Bomb(Vector2 pos, Vector2 direction)
    {
        //GameObject b = Instantiate(bomb, pos + direction, Quaternion.identity);

        //Timer until it explodes
        if (bombtimer < bombCooldownTime)
        {
            bombtimer += Time.deltaTime;
        }
        else if (bombtimer >= bombCooldownTime)
        {
            bombtimer = 0;
            exploded = true;
        }

        //When Bomb explodes
        if(exploded)
        {

        }

    }

}

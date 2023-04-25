using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExtra : MonoBehaviour
{
    [SerializeField] string playerName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(tag == "Glove of Thunder")
        {

            //Something in here doesn't work (maybe playerName?)
            playerName = name.Substring(0, 8);
            if (tag == "Glove of Thunder" && GameObject.Find(playerName).GetComponent<PlayerController>().powerLvl == 2 && collision.name != playerName)
            {
                Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 4);
                foreach (Collider2D player in hitPlayers)
                {
                    Debug.Log("You Hit " + player.name);
                    if (player.gameObject.tag == "Player")
                    {
                        if (player.gameObject.GetComponent<PlayerController>().shieldUp && player.gameObject.GetComponent<PlayerController>().validBlock)
                        {
                            player.gameObject.GetComponent<PlayerController>().ShieldBlocked(GameObject.Find(playerName).GetComponent<PlayerController>().boltDmg, GameObject.Find(playerName).GetComponent<PlayerController>().attacker);
                        }
                        else
                        {
                            player.gameObject.GetComponent<PlayerController>().Damaged(GameObject.Find(playerName).GetComponent<PlayerController>().boltDmg, GameObject.Find(playerName).GetComponent<PlayerController>().attacker);
                        }
                    }
                }
                Destroy(gameObject);
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [SerializeField] GameObject[] items;

    public GameObject[] pos;
    private Vector2[] itemSpawnPos;
    int positions;

    private int ranIndex;

    [SerializeField] float spawntimer = 30f;
    [SerializeField] float spawnCooldownTime = 30f;

    // Start is called before the first frame update
    void Start()
    {
        itemSpawnPos = new Vector2[pos.Length];
        foreach (GameObject position in pos)
        {
            itemSpawnPos[positions] = position.transform.position;
            positions++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //May not be nessesary:
        //currentScene = SceneManager.GetActiveScene();

        if (spawntimer < spawnCooldownTime)
        {
            spawntimer += Time.deltaTime;
        }
        else if (spawntimer >= spawnCooldownTime)
        {
            spawntimer = 0;
            foreach (Vector2 itemSpawn in itemSpawnPos)
            {
                ItemSpawn();
                //PowerUpSpawn();
            }
        }
    }

    private void ItemSpawn()
    {
        //getting randomItems
        int ranItem = RandomNum(0, items.Length);

        //getting all objects in scene
        GameObject[] itemsInScene = GameObject.FindGameObjectsWithTag("Pick Up").Concat(GameObject.FindGameObjectsWithTag("Power Up")).ToArray();

        //finding possible spawn locations
        List<Vector2> possibleSpots = new List<Vector2>(itemSpawnPos);
        /*
        Debug.Log(possibleSpots[0]);
        Debug.Log(possibleSpots[1]);
        Debug.Log(possibleSpots[2]);
        Debug.Log(possibleSpots[3]);
        Debug.Log(possibleSpots[4]);
        Debug.Log(possibleSpots[5]);
        */
        for (int i = 0; i < possibleSpots.Count; i++)
        {
            //Debug.Log(i);
            bool remove = false;
            foreach (GameObject item in itemsInScene)
            {
                if (new Vector2(item.transform.position.x, item.transform.position.y) == possibleSpots[i])
                {
                    remove = true;
                }
            }
            if (remove)
            {
                possibleSpots.RemoveAt(i);
                i--;
            }
        }

        if (possibleSpots.Count > 0)
        {
            //getting random location out of possible locations
            ranIndex = RandomNum(0, possibleSpots.Count - 1);

            //spawning object
            GameObject ip;
            ip = Instantiate(items[ranItem], possibleSpots[ranIndex], Quaternion.identity);
            //NameChange(ip);
            //spawned[ranIndex] = true;
        }
    }
    /*
    private void PowerUpSpawn()
    {
        //getting randomItems
        int ranPowerUp = RandomNum(0, powerUps.Length);

        //getting all objects in scene
        GameObject[] powerUpsInScene = GameObject.FindGameObjectsWithTag("PowerUp");

        //finding possible spawn locations
        List<Vector2> possibleSpots = new List<Vector2>(powerUpSpawnPos);

        for (int i = 0; i < possibleSpots.Count; i++)
        {
            //Debug.Log(i);
            bool remove = false;
            foreach (GameObject powerUp in powerUpsInScene)
            {
                if (new Vector2(powerUp.transform.position.x, powerUp.transform.position.y) == possibleSpots[i])
                {
                    remove = true;
                }
            }
            if (remove)
            {
                possibleSpots.RemoveAt(i);
                i--;
            }
        }

        if (possibleSpots.Count > 0)
        {
            //getting random location out of possible locations
            ranIndex = RandomNum(0, possibleSpots.Count - 1);

            //spawning object
            GameObject ip;
            ip = Instantiate(powerUps[ranPowerUp], possibleSpots[ranPowerUp], Quaternion.identity);
            NameChange(ip);
            //spawned[ranIndex] = true;
        }
    }
    */
    private int RandomNum(int min, int max)
    {
        return Random.Range(min, max);
    }
    /*
    private void NameChange(GameObject ip)
    {
        //Items name change
        if(ip.name.Substring(0, 3) == "Bow")
        {
            ip.name = "Bow";
        }
        else if(ip.name.Substring(0, 4) == "Bomb")
        {
            ip.name = "Bomb";
        }
        else if(ip.name.Substring(0, 10) == "Dark Leech")
        {
            ip.name = "Dark Leech";
        }
        else if(ip.name.Substring(0, 16) == "Glove of Thunder")
        {
            ip.name = "Glove of Thunder";
        }

        //Powerup name change
        else if(ip.name.Substring(0, 11) == "Hasty Boots")
        {
            ip.name = "Hasty Boots";
        }
        else if (ip.name.Substring(0, 9) == "Health Up")
        {
            ip.name = "Health Up";
        }
        else if (ip.name.Substring(0, 6) == "Mana Up")
        {
            ip.name = "Mana Up";
        }
        else if (ip.name.Substring(0, 12) == "Mystic Blade")
        {
            ip.name = "Mystic Blade";
        }
        else if (ip.name.Substring(0, 13) == "Mystic Shield")
        {
            ip.name = "Mystic Shield";
        }
    }
    */
}
